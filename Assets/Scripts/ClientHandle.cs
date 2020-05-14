using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {        
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        //customizationtest
        int _hairtype = _packet.ReadInt();
        int _hairColor = _packet.ReadInt();
        int _skinColor = _packet.ReadInt();


        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation, _hairtype, _hairColor, _skinColor);
      //  PositionSync.AddPlayerInList(_id, _position);
    }

    public static void PlayerPosition(Packet _packet)
    {        
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].transform.position = _position;
        //PositionSync.Interpolate(_id, _position);
    }
    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        
       // GameManager.players[_id].transform.rotation = _rotation;
        PositionSync.SyncRot(_id, _rotation);        

    }
    public static void ReceiveAnimation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool anima = _packet.ReadBool();

        
        GameManager.players[_id].animator.SetBool("isRunning", anima);        
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        //destroy o objeto quando desconectado e remove da lista
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }
}
