using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
    public static CharacterCreation instance;

    public InputField characterName;
    public Text msgText;

    public GameObject[] hairs;
    public Material[] hairColor;
    public Material[] skinColor;

    private int currentHair;
    private int currentHairColor;
    private int currentSkinColor;

    public GameObject curHair;
    public Material curHairMate;
    public Material curSkinMate;
    public GameObject body;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;            
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exist, destroying object!");

            Destroy(this.gameObject);
        }
    }


    private void Update()
    {
        for (int i = 0; i < hairs.Length; i++)
        {
            if(i == currentHair)
            {
                hairs[i].SetActive(true);
                curHair = hairs[i];                
            }
            else
            {
                hairs[i].SetActive(false);
            }            
        }

        for (int i = 0; i < hairColor.Length; i++)
        {
            if(i == currentHairColor)
            {
                curHairMate = hairColor[i];
                curHair.GetComponent<SkinnedMeshRenderer>().material = curHairMate;
            }
        }

        for (int i = 0; i < skinColor.Length; i++)
        {
            if (i == currentSkinColor)
            {
                curSkinMate = skinColor[i];
                body.GetComponent<SkinnedMeshRenderer>().material = curSkinMate;
            }
        }
    }

    public void switchHair()
    {
        if(currentHair == hairs.Length -1)
        {
            currentHair = 0;
        }
        else
        {
            currentHair++;
        }        
    }
    public void switchHairColor()
    {
        if (currentHairColor == hairColor.Length - 1)
        {
            currentHairColor = 0;
        }
        else
        {
            currentHairColor++;
        }
    }
    public void switchSkinColor()
    {
        if (currentSkinColor == skinColor.Length - 1)
        {
            currentSkinColor = 0;
        }
        else
        {
            currentSkinColor++;
        }
    }

    public void BackToCharacterSelection()
    {
        Menu.instance.ReloadingStatus();
        SceneManager.LoadScene("MainMenu");
    }

    public void CreateNewCharButton()
    {
        Web.instance.NewCharacter(characterName.text, currentHair, currentHairColor, currentSkinColor);              
    }

    public void ReceiveMsg(string _msg)
    {
        if(_msg == "0")
        {
            msgText.text = "Character Created!";
        }
        if (_msg == "3")
        {
            msgText.text = "This name already exist!";
        }
    }
  
}
