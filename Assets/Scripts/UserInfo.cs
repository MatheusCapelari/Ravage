using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class UserInfo : MonoBehaviour
{
    public static UserInfo instance;

    public int id = 0;
    public string username = null;
    public bool loginOK = false;
    public bool isOnMenu = true;
    public GameObject charGrid;    
    public string charactername = null;

    //customizationtest
    public int hairTypeSet;
    public int hairColorSet;
    public int skinColorSet;

    public static List<Characters> playerchars = new List<Characters>();
    public static List<GameObject> charGridinList = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exist, destroying object!");
            Destroy(this);
        }
    }
    public class Characters
    {        
        public string name;
        public int level;
        public int gold;
        //customizationtest
        public int hairType;
        public int hairColor;
        public int skinColor;
    }

    public void ReadUserID(string _str)
    {
        JSONArray userinfo = JSON.Parse(_str) as JSONArray;

        instance.id = userinfo[0].AsObject["id"];
        instance.username = userinfo[0].AsObject["name"];
        loginOK = true;
    }
    public void ReadCharacters(string _str)
    {
        Debug.Log(_str);
        Menu.instance.CharSelectionMenu();


        if (_str == "10")
        {
            _str = null;
        }
        else
        {
            Debug.Log("ta entrando aqui?");
            Debug.Log(_str);
            JSONArray userinfo = JSON.Parse(_str) as JSONArray;

            for (int i = 0; i < userinfo.Count; i++)
            {
                Characters charlist = new Characters();

                charlist.name = userinfo[i].AsObject["name"];
                charlist.level = userinfo[i].AsObject["level"];
                
                //customizationtest
                charlist.hairType = userinfo[i].AsObject["hairtype"];
                charlist.hairColor = userinfo[i].AsObject["haircolor"];
                charlist.skinColor = userinfo[i].AsObject["skincolor"];

                //instantiate a character in a grid
                GameObject character = Instantiate(charGrid);
                character.transform.SetParent(Menu.instance.characterGrid.transform);
                character.transform.localPosition = Vector3.zero;
                character.transform.localScale = new Vector3(1, 1, 1);

                character.transform.Find("Name").GetComponent<Text>().text = userinfo[i].AsObject["name"];
                character.transform.Find("Level").GetComponent<Text>().text = userinfo[i].AsObject["level"];
                playerchars.Add(charlist);
                charGridinList.Add(character);
            }
        }
    }
    public void LoadCharacters()
    {        
        Web.instance.getChar(id);        
    }

    public void ClearList()
    {
        foreach(GameObject g in charGridinList)
        {
            Destroy(g);
        }
        charactername = null;
        Menu.instance.CharacSelected.text = null;
        charGridinList.Clear();
        playerchars.Clear();
    }
    public void SelectedCharacter(string _charactername)
    {
        foreach(Characters c in playerchars)
        {
            if(c.name == _charactername)
            {
                hairColorSet = c.hairColor;
                hairTypeSet = c.hairType;
                skinColorSet = c.skinColor;
            }
        }
    }
}
