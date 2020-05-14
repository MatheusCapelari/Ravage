using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{    
    public int id;
    public string username;
    public Animator animator;
    public Vector3 position;
    
    TextMesh charactername;
    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        animator = GetComponentInChildren<Animator>();
        charactername = GetComponentInChildren<TextMesh>();
        
       
        charactername.text = _username;
    }
}
