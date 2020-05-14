using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSync : MonoBehaviour
{

    private static Quaternion lastRot;
    public static PositionSync instance;

    public static List<LastPosition> lastPosList = new List<LastPosition>();

    public static LastPosition last;
   
    public class LastPosition
    {
        public int clientID;
        public Vector3 lastPos;

    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        last = new LastPosition();        
    }



    public static void Interpolate(int _id, Vector3 _to)
    {

        Debug.Log($"Jogador: {_id} posicao: {_to}");

       if (GameManager.players[_id].position == _to)
       {
           if (GameManager.players[_id].position != _to)
           {
               GameManager.players[_id].transform.position = Vector3.Lerp(GameManager.players[_id].position, _to, Time.deltaTime * 30);
               GameManager.players[_id].position = _to;
           }
           return;
       }
       GameManager.players[_id].transform.position = Vector3.LerpUnclamped(GameManager.players[_id].position, _to, Time.deltaTime * 30);
       GameManager.players[_id].position = _to;
    }
    public static void SyncRot(int _id, Quaternion _rot)
    {
        GameManager.players[_id].transform.rotation = Quaternion.Lerp(lastRot, _rot, Time.deltaTime * 30);
        lastRot = GameManager.players[_id].transform.rotation;
    }
}
