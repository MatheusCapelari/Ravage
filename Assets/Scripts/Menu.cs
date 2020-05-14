using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static Menu instance;

    public GameObject loginMenu;
    public GameObject RegisterMenu;
    public GameObject Loading;
    public GameObject CharacterSelection;
    public GameObject imageDrag;
    public GameObject characterGrid;
    public GameObject GeneralMenu;
    public Text statusText;
    public Text CharacSelected;

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
            
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject);
                SelectCharacter(hit.transform.gameObject);
            }
        }
        
    }
    private void SelectCharacter(GameObject go)
    {
        //Seleciona o personagem
        CharacSelected.text = go.gameObject.transform.Find("Name").GetComponent<Text>().text;
        UserInfo.instance.charactername = CharacSelected.text;
        UserInfo.instance.SelectedCharacter(CharacSelected.text);
    }
    public void MainMenu()
    {
        Loading.SetActive(false);
        imageDrag.SetActive(true);
        statusText.text = null;
        RegisterMenu.SetActive(false);
        loginMenu.SetActive(true);
        CharacterSelection.SetActive(false);
        Web.instance.RegnameField = null;
        Web.instance.RegpasswordField = null;
    }
    public void CharSelectionMenu()
    {
        Loading.SetActive(false);
        imageDrag.SetActive(false);
        statusText.text = null;
        CharacterSelection.SetActive(true);
    }
    public void ConnectToServer()
    {

        Client.instance.ConnectToServer();

        Destroy(this.gameObject);
        Destroy(Web.instance.gameObject);



        //TROCAR DE SCENA
        SceneManager.LoadScene("World");
        
    }
    public void LoadMenu()
    {
        Loading.SetActive(true);
        imageDrag.SetActive(true);
        CharacterSelection.SetActive(false);
    }
    public void ReloadingStatus()
    {
        GeneralMenu.SetActive(true);
        Loading.SetActive(true);
        imageDrag.SetActive(true);
        CharacterSelection.SetActive(false);        
        Web.instance.Load();
    }
    public void CharacterCreation()
    {
        UserInfo.instance.ClearList();
        GeneralMenu.SetActive(false);       

        
        SceneManager.LoadScene("CharacterCreation");
    }
}
