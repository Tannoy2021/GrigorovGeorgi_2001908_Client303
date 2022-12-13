using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class ClientHandler : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        int _myId = _packet.ReadInt();
        GameClient.instance.myId = _myId;
        ClientSend.UserConnected();
        GameClient.instance.udpClient.Connect(((IPEndPoint)GameClient.instance.tcpClient.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        GameManager.instance.SpawnPlayer(_id,_position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        GameManager.players[_id].SetLerpPosition(_position);
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void CreateBuffSpawner(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        Vector3 _spawnerPosition = _packet.ReadVector3();
        bool _hasItem = _packet.ReadBool();
        GameManager.instance.CreateBuffSpawn(_spawnerId, _spawnerPosition, _hasItem);
    }  
    public static void BuffSpawned(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        GameManager.buffSpawners[_spawnerId].BuffSpawned();
    }
    public static void BuffPickedUp(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _byPlayer = _packet.ReadInt();
        GameManager.buffSpawners[_spawnerId].BuffPickedUp();
    }
    public static void CreateFinishLine(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        bool _hasItem = _packet.ReadBool();
    }
    public static void FinishCollected(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _byPlayer = _packet.ReadInt();
        GameManager.FinishLines[_spawnerId].FinishCollected();
    }
}
