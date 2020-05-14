using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{    
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UserInfo.instance.charactername);
            //mandar o id do meu personagem que quero logar

            //mandando teste de customization
            _packet.Write(UserInfo.instance.hairTypeSet);
            _packet.Write(UserInfo.instance.hairColorSet);
            _packet.Write(UserInfo.instance.skinColorSet);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }      

    public static void SendMovement()
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendMovement))
        {            
            _packet.Write(GameManager.players[Client.instance.myId].transform.position);
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);               
        }
    }
    public static void SendAnimation(bool _anima)
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendAnimation))
        {
            _packet.Write(_anima);
            SendTCPData(_packet);    
        }
    }
    #endregion
}
