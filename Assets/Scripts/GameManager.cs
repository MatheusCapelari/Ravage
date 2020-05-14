using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    //public static IList<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);            
        }
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation, int _hairType, int _hairColor, int _skinColor)
    {
        
        GameObject _player;
        //Debug.Log($"Chegou o tipo de cabelo: {_hairType}, a cor: {_hairColor}, cor da pele: {_skinColor}");

        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
            
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
            
        }
        

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);

        //colocar em uma lista para mudar sempre que necessario 
        players.Add(_id, _player.GetComponent<PlayerManager>());
        _player.GetComponent<LoadPrefab>().SetCustomization(_hairType, _hairColor, _skinColor);
    }   
}
