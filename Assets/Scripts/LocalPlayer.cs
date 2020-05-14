using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
    public Camera playerCam;
    int floorMask;

    public float runSpeed = 10;
    public float smoothRotate = 15;

    public PlayerState pState;

    private Animator _animator;
    private CharacterController _characterController;
    private Rigidbody rb;
    private Vector3 moveToEnemy;

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
                    var rayA = playerCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(rayA, out hit, 1000f))
                    {
                        if (hit.collider.CompareTag("Enemy"))
                        {
                            moveToEnemy = hit.point;
                            pState = PlayerState.HoldAttack;                            
                            Debug.Log("Achou enemy.");                            
                            break;
                        }
                    }
                    if (Physics.Raycast(rayA, out hit, 1000f, floorMask))
                    {
                        pState = PlayerState.Movement;
                    }
                        
                }
                break;

            case PlayerState.Movement:

                var rayB = playerCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(rayB, out hit, 1000f, floorMask))
                {
                    Animating(true);
                    movedirection = hit.point;
                    CharacterMove(movedirection);
                }
                //Mouse está em cima do monstro e 
                if (Physics.Raycast(rayB, out hit, 1000f))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {                        
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
                Animating(false);
                Attack();
                if (Input.GetMouseButtonUp(0))
                {
                    pState = PlayerState.Idle;
                    Animating(false);
                }
                break;

            case PlayerState.HoldAttack:
                CharacterMove(new Vector3(moveToEnemy.x, 0, moveToEnemy.z));
                Animating(true);
                var rayD = playerCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(rayD, out hit, 1000f))
                {                    
                    if (hit.collider.CompareTag("Enemy"))
                    {                        
                        if (isOnRange)
                        {
                            FaceNPC(hit.point);
                            Animating(false);
                            pState = PlayerState.Attack;

                        }
                    }
                }
                if (isOnRange)
                {
                    pState = PlayerState.Attack;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    pState = PlayerState.Idle;
                    Animating(false);
                }
                break;

            case PlayerState.OnWait:
                Debug.Log("entrou na espera");
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
        
    }
    void Animating(bool _anim)
    {
        if (_anim)
        {
            _animator.SetBool("isRunning", _anim);
           
        }
        else
        {
            _animator.SetBool("isRunning", false);
           
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

    IEnumerator Attack()
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
        OnWait,
        HoldAttack
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
