using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPrefab : MonoBehaviour
{

    public static LoadPrefab instance;

    public GameObject[] hairs;
    public Material[] hairColor;
    public Material[] skinColor;

    public GameObject body;
    public GameObject currentHairType;
    public Material currentHairMate;
    public Material currentMateSkin;

  // public void Initialize(int _hairType, int _hairColor, int _skinColor)
  // {
  //     id = _id;
  //     username = _username;
  //     animator = GetComponentInChildren<Animator>();
  //     charactername = GetComponentInChildren<TextMesh>();
  //
  //
  //     charactername.text = _username;
  // }
    public void SetCustomization(int _hairType, int _hairColor, int _skinColor)
    {
        Debug.Log("entrou em set customization");
        currentHairType = hairs[_hairType];
        currentHairType.SetActive(true);

        currentHairMate = hairColor[_hairColor];
        currentHairType.GetComponent<SkinnedMeshRenderer>().material = currentHairMate;

        currentMateSkin = skinColor[_skinColor];
        body.GetComponent<SkinnedMeshRenderer>().material = currentMateSkin;
    }
}
