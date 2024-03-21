using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed , gravityModifier,jumpPower, runSpeed = 12f;
    public CharacterController CharCon;
    private Vector3 moveInput;
    public Transform camTrans;
    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    private bool canjump , canDoubleJump;
    public Transform groudCheckPoint;
    public LayerMask whatIsGround;

    public Animator anim;

    void Start()
    {
        
    }

    void Update()
    {
        //moveInput.x = Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime;
        //moveInput.z = Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime;
        float yStore = moveInput.y;
        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = horiMove + vertMove;
        moveInput.Normalize();                        
        moveInput = moveInput * MoveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveInput = moveInput * runSpeed;
        }
        else
        {
            moveInput = moveInput * MoveSpeed;
        }


        moveInput.y = yStore;
        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        if (CharCon.isGrounded)
        {
            moveInput.y  = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        //chặn nhảy nhiều lần 

        canjump = Physics.OverlapSphere(groudCheckPoint.position, .25f ,whatIsGround).Length > 0;


        if(canjump)
        {
            canDoubleJump = false;
        }



        //Handle jumping nhảy

        if(Input.GetKeyDown(KeyCode.Space) && canjump) 
        {
            moveInput.y = jumpPower;
            canDoubleJump = true;
        }else if(canDoubleJump && Input.GetKeyDown(KeyCode.Space) )
        {
            moveInput.y = jumpPower;
            canDoubleJump = false;
        }

        CharCon.Move(moveInput * Time.deltaTime);

        //ControCamera
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X") , Input.GetAxisRaw("Mouse Y")) * mouseSensitivity ;
        if (invertX)
        {
            mouseInput.x= -mouseInput.x; 
        }
        if (invertY )
        {
            mouseInput.y= -mouseInput.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3( -mouseInput.y, 0f, 0f));

        anim.SetFloat("moveSpeed" ,moveInput.magnitude);
        anim.SetBool("Onround", canjump);

      
    }
}
 