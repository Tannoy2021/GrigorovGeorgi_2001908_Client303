using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        GameClient.instance.tcpClient.SendTcpData(_packet);
    }
    public static void UserConnected()
    {
        using (Packet _packet = new Packet((int)ClientPackets.userConnected))
        {
            _packet.Write(GameClient.instance.myId);
            SendTCPData(_packet);
        }
    }
    public static void PlayerMovement(bool[] _clientInputs)
    {
        using(Packet packet = new Packet((int)ClientPackets.playerMovement))
        {
            packet.Write(_clientInputs.Length);
            foreach (bool _clientInput in _clientInputs) 
            {
                packet.Write(_clientInput);
            }
            packet.Write(GameManager.players[GameClient.instance.myId].transform.rotation);
            SendTCPData(packet);
        }
    }
}
