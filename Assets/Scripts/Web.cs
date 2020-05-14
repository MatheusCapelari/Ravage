using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Web : MonoBehaviour
{
    public static Web instance;

    public InputField LognameField;
    public InputField LogpasswordField;

    public InputField RegnameField;
    public InputField RegpasswordField;

   
    public Text loadStatus;
    int userId = 0;

    

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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Login();
        }
    }

    IEnumerator LoginPlayer(string _username, string _password)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", _username);
        form.AddField("password", _password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.8/Server/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //string a = www.downloadHandler.text;
                 GetStatus(www.downloadHandler.text);
               
            }
        }
    }
    IEnumerator Registration(string _username, string _password)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", _username);
        form.AddField("password", _password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.8/Server/registration.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                GetStatus(www.downloadHandler.text);
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    IEnumerator GetLoginData(string _username)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", _username);        

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.8/Server/getloginid.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {                
                UserInfo.instance.ReadUserID(www.downloadHandler.text);
            }
        }
    }
    //Query
    IEnumerator GetCharactersData(int _userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", _userId);
        Debug.Log(_userId);

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.8/Server/characters.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {                
                UserInfo.instance.ReadCharacters(www.downloadHandler.text);
            }
        }
    }
    IEnumerator RegNewCharacter(int _userId, string _name, int _hairtype, int _haircolor, int _skincolor)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", _userId);
        form.AddField("CharName", _name);
        form.AddField("HairType", _hairtype);
        form.AddField("HairColor", _haircolor);
        form.AddField("SkinColor", _skincolor);

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.8/Server/newcharacters.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                CharacterCreation.instance.ReceiveMsg(www.downloadHandler.text);
            }
        }
    }
    IEnumerator Loading()
    {
        
      // Menu.instance.loginMenu.SetActive(false);
      // Menu.instance.RegisterMenu.SetActive(false);
      // Menu.instance.Loading.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(0.7f);
            loadStatus.text = "Loading Characters.";
            yield return new WaitForSeconds(0.7f);
            loadStatus.text = "Loading Characters..";
            yield return new WaitForSeconds(0.7f);
            loadStatus.text = "Loading Characters...";
            if(UserInfo.instance.loginOK == true)
            {
                UserInfo.instance.LoadCharacters();               
            }
            else
            {
                loadStatus.text = "TIME OUT!";
            }
            break;
        }
        
    }
    #region Routines
    public void Login()
    {
        StartCoroutine(LoginPlayer(LognameField.text, LogpasswordField.text));
    }
    public void Registration()
    {
        StartCoroutine(Registration(RegnameField.text, RegpasswordField.text));
    }    
    #endregion
    public void GetStatus(string _data)
    {
        if (_data == "6")
        {
            Menu.instance.statusText.text = "Wrong Password";
        }
        if (_data == "5")
        {
            Menu.instance.statusText.text = "Invalid Username";
        }
        if (_data == "7")
        {
            Menu.instance.statusText.text = "Login Sucessfull!";
            StartCoroutine(GetLoginData(LognameField.text));
            // StartCoroutine(Loading());
            Load();
        }
        if (_data == "3")
        {
            Menu.instance.statusText.text = "Name Already Taken!";
        }
        if (_data == "9")
        {
            Menu.instance.statusText.text = "Registration Complete!";
        }
    }
    //Buttons
    public void RegisterButton()
    {
        Menu.instance.loginMenu.SetActive(false);
        Menu.instance.RegisterMenu.SetActive(true);
    }
    public void MenuButton()
    {
        //Menu.instance.RegisterMenu.SetActive(false);
        //Menu.instance.loginMenu.SetActive(true);
    }
    public void getChar(int _id)
    {
        userId = _id;
        StartCoroutine(GetCharactersData(_id));
    }
    public void NewCharacter(string _name, int _hairtype, int _haircolor, int _skincolor)
    {
        StartCoroutine(RegNewCharacter(userId, _name, _hairtype, _haircolor, _skincolor));        
    }
    public void Load()
    {
        Menu.instance.loginMenu.SetActive(false);
        Menu.instance.LoadMenu();
        StartCoroutine(Loading());
    }
}
