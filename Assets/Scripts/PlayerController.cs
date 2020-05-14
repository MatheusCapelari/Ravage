using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;

    private CharacterController _characterController;    

    public float runSpeed = 10;
    public float maxJetpack = 50f;

    public float gravity = -12;
    public float jumpHeight = 1;  

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;
    bool isStopped;

    int pktNumber = 1; 

    //customizationtest
  

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
        isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Move(input);
        Animating(input);
    }    

    void Move(Vector2 dir)
    {
        Vector2 inputDir = dir.normalized;

        //Smooth Rotation
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        float targetSpeed = runSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);        

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        _characterController.Move(velocity * Time.deltaTime);

        if (dir != Vector2.zero)
        {            
            ClientSend.SendMovement();
            AutoIncrement();
        }

    }

    void AutoIncrement()
    {
        if(pktNumber <= 1000)
        {
            pktNumber++;
        }
        else
        {
            pktNumber = 1;
        }
    }

    void Animating(Vector2 dir)
    {        
        bool walking = dir.x != 0f || dir.y != 0f;

        _animator.SetBool("isRunning", walking);

        if(walking == true && isStopped == true)
        {
            isStopped = false; 
            ClientSend.SendAnimation(walking);
            Debug.Log("Foi enviado a animacao de correr");
        }       
       
       if(walking == false && isStopped == false)
        {
             isStopped = true;
             ClientSend.SendAnimation(false);
            Debug.Log("Foi enviado a animacao de parar");
        }

    }
}
