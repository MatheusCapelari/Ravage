using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public Camera playerCam;
    int floorMask;

    public float runSpeed = 10;
    public float smoothRotate = 15;

    public PlayerState pState;

    private Animator _animator;
    private CharacterController _characterController;
    private Rigidbody rb;

    private bool isOnRange = false;
    private bool isAttackin = false;
    


    private Vector3 movedirection;
    

    public float turnSmoothTime = 0.2f;
    

    private void Awake()
    {
        
        floorMask = LayerMask.GetMask("Floor");
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        pState = PlayerState.Idle;
    }
    private void Update()
    {
       
        RaycastHit hit;

        switch (pState)
        {
            case PlayerState.Idle:                

                if (Input.GetMouseButtonDown(0))
                {
                    var rayB = playerCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(rayB, out hit, 1000f))
                    {
                        if (hit.collider.CompareTag("Enemy") && isOnRange)
                        {
                            pState = PlayerState.Attack;
                        }
                    }
                    pState = PlayerState.Movement;
                }

                break;

            case PlayerState.Movement:

                var ray = playerCam.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out hit, 1000f, floorMask))
                {
                    Animating(true);
                    movedirection = hit.point;
                    CharacterMove(movedirection);
                }
                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        Debug.Log("Entra aqui?");
                        if (isOnRange)
                        {
                            FaceNPC(hit.point);
                            Animating(false);
                            pState = PlayerState.Attack;
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    pState = PlayerState.Idle;
                    Animating(false);
                }

                break;

            case PlayerState.Attack:
                var rayA = playerCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(rayA, out hit, 1000f))
                {
                    FaceNPC(hit.point);
                    if (hit.collider.CompareTag("Enemy") && isOnRange && !isAttackin)
                    {
                       isAttackin = true;
                       Debug.Log("isOnRange");
                       
                       Animating(false);
                       //_animator.SetTrigger("isAttack2");
                        StartCoroutine(Attack(hit.point));
                       pState = PlayerState.OnWait;
                    }
                    else
                    {
                        pState = PlayerState.Movement;
                        Debug.Log("voltou a andar");
                    }                    
                }
                if (Input.GetMouseButtonUp(0))
                {
                    pState = PlayerState.Idle;
                }
                break;

            case PlayerState.OnWait:
                if (!isAttackin)
                {        
                    pState = PlayerState.Attack;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    pState = PlayerState.Idle;
                }

                break;
        }
    }

    void CharacterMove(Vector3 _dir)
    {
        Vector2 inputDir = _dir.normalized;

        //Smooth Rotation
        if (inputDir != Vector2.zero)
        {
            Vector3 lookDir = _dir - transform.position;
            lookDir.y = 0;
            Quaternion newRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * smoothRotate);
        }

        //NewPosition // transform.position = Vector3.MoveTowards(transform.position, _dir, runSpeed * Time.deltaTime);   
        // if(offset.magnitude > .1f) 
        Vector3 offSet = _dir - transform.position;
        offSet = offSet.normalized * runSpeed;
        _characterController.Move(offSet * Time.deltaTime);
        ClientSend.SendMovement();
    }
    void Animating(bool _anim)
    {
        if (_anim)
        {
            _animator.SetBool("isRunning", _anim);
            ClientSend.SendAnimation(true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
             ClientSend.SendAnimation(false);
        }
    }
    void FaceNPC(Vector3 _dir)
    {
        Vector2 inputDir = _dir.normalized;

        //Smooth Rotation
        if (inputDir != Vector2.zero)
        {
            Vector3 lookDir = _dir - transform.position;
            lookDir.y = 0;
            Quaternion newRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
        }
    }

    IEnumerator Attack(Vector3  dir)
    {        
        float random = Random.Range(0, 3);
        Debug.Log(random);
        if (random == 1)
        {            
            _animator.SetTrigger("isAttack1");
        }
        if (random == 2)
        {
            _animator.SetTrigger("isAttack1");
        }
        if (random == 0)
        {
            _animator.SetTrigger("isAttack2");
        }
        //_animator.SetTrigger("isAttack2");
        //yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(2).length + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitForSeconds(0.65f);
        Debug.Log("aguardando");
        isAttackin = false;
    }

    public enum PlayerState
    {
        Idle,
        Movement,
        Attack,
        OnWait
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("Colidiu");
            other.GetComponent<Renderer>().material.color = Color.red;
            isOnRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("Saiu da colisão");
            other.GetComponent<Renderer>().material.color = Color.yellow;
            isOnRange = false;
        }
    }    
}