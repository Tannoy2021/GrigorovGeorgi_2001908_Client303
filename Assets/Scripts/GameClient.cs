using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using UnityEngine.UIElements;


public class GameClient : MonoBehaviour
{
    public static GameClient instance;
    public static int bufferSize = 4096;
    public string serverIp = "127.0.0.1";
    public int serverPort = 2000;
    public int myId = 0;
    private bool Connected = false;
    private delegate void Packets(Packet _packet);
    private static Dictionary<int, Packets> packets;
    public TCP tcpClient;
    public UDP udpClient;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        tcpClient = new TCP();
        udpClient = new UDP();
    }
    private void OnApplicationQuit()
    {
        Disconnect();
    }
    public void ConnectToServer()
    {
        InitializeClientData();
        Connected= true;
        tcpClient.Connect();
    }
    public class TCP
    {
        public TcpClient socket;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;
        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = bufferSize,    SendBufferSize = bufferSize
            };
            receiveBuffer = new byte[bufferSize];
            socket.BeginConnect(instance.serverIp, instance.serverPort, ConnectCallback, socket);
        }
        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);
            if (!socket.Connected)
            {
                return;
            }
            stream = socket.GetStream();
            receivedData = new Packet();
            stream.BeginRead(receiveBuffer, 0, bufferSize, ReceiveCallbackTcp, null);
        }
        public void SendTcpData(Packet _packet)
        {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
        }
        private void ReceiveCallbackTcp(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }
                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleTcpData(_data));
                stream.BeginRead(receiveBuffer, 0, bufferSize, ReceiveCallbackTcp, null);
            }
            catch
            {
                Disconnect();
            }
        }
        private bool HandleTcpData(byte[] _data)
        {
            int _packetLength = 0;
            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }
            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packets[_packetId](_packet);
                    }
                });
                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }
            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }
        private void Disconnect()
        {
            instance.Disconnect();
            stream = null;       receivedData = null;     receiveBuffer= null;   socket = null;
        }
    }
    public class UDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;
        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.serverIp), instance.serverPort);
        }
        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);
            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallbackUdp, null);
            using (Packet _packet = new Packet())
            {
                SendUdpData(_packet);
            }
        }
        private void Disconnect()
        {
            instance.Disconnect();
            endPoint = null;        socket = null;
        }
        public void SendUdpData(Packet _packet)
        {
                _packet.InsertInt(instance.myId);
                if (socket != null)
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }
        }
        private void ReceiveCallbackUdp(IAsyncResult _result)
        {
            try
            {
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallbackUdp, null);

                if (_data.Length < 4)
                {
                    instance.Disconnect();
                    return;
                }
                HandleUdpData(_data);
            }
            catch
            {
                Disconnect();
            }
        }
        private void HandleUdpData(byte[] _data)
        {
            using (Packet _packet = new Packet(_data))
            {
                int _packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(_packetLength);
                int _packetId = _packet.ReadInt();
                packets[_packetId](_packet);
            }
        }
    }
    private void InitializeClientData()
    {
        packets = new Dictionary<int, Packets>()
        {
            { (int)ServerPackets.welcome, ClientHandler.Welcome },
            { (int)ServerPackets.spawnPlayer, ClientHandler.SpawnPlayer },
            { (int)ServerPackets.playerPos, ClientHandler.PlayerPosition },
            { (int)ServerPackets.playerRotation, ClientHandler.PlayerRotation },
            { (int)ServerPackets.playerDisconnected, ClientHandler.PlayerDisconnected },
            { (int)ServerPackets.createBuffSpawner, ClientHandler.CreateBuffSpawner },
            { (int)ServerPackets.buffSpawned, ClientHandler.BuffSpawned },
            { (int)ServerPackets.buffPickedUp, ClientHandler.BuffPickedUp },
            { (int)ServerPackets.createFinishLine, ClientHandler.CreateFinishLine },
            { (int)ServerPackets.finishCollected, ClientHandler.FinishCollected },
        };
    }
    private void Disconnect()
    {
        if(Connected) 
        {
            Connected= false;
            tcpClient.socket.Close();         
            udpClient.socket.Close();
            Debug.Log("Disconnected from server");
        }
    }
}
