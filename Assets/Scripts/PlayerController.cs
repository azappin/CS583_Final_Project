using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    //Variables that control speed, force and direction
    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;
    public float bounceForce = 8f;

    public float rotateSpeed;
    private Vector3 moveDirection;

    //Variables for Controllers, Animators and Objects
    public CharacterController charController;
    private Camera cam;
    public GameObject playerModel;
    public Animator anim;

    //Damage Reference Variables
    public bool isKnocking;
    public float knockbackLength = .5f;
    private float knockbackCounter;
    public Vector2 knockbackPower;

    //Player Pieces
    public GameObject[] playerPieces;

    public bool stopMove;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        //Getting the main camera
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (isKnocking == false && stopMove == false)
        {

            //Update the Player's movement and direction
            float yStore = moveDirection.y;
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
            moveDirection.Normalize();
            moveDirection = moveDirection * moveSpeed;
            moveDirection.y = yStore;

            //Update for if the Player has jumped.
            if (charController.isGrounded)
            {

                moveDirection.y = -.75f;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                }
            }

            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
            charController.Move(moveDirection * Time.deltaTime);


            //Update if the Player has moved left or right
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {

                transform.rotation = Quaternion.Euler(0f, cam.transform.rotation.eulerAngles.y, 0f);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));

                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);

            }

        }

        if(isKnocking == true)
        {
            knockbackCounter -= Time.deltaTime;

            float yStore = moveDirection.y;
            moveDirection = playerModel.transform.forward * -knockbackPower.x;
            moveDirection.y = yStore;

            //Update for if the Player has jumped.
            if (charController.isGrounded)
            {
                moveDirection.y = -.75f;
            }

            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

            charController.Move(moveDirection * Time.deltaTime);

            if (knockbackCounter <= 0)
            {
                isKnocking = false;
            }

        }

        if(stopMove == true)
        {
            moveDirection = Vector3.zero;
            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
            charController.Move(moveDirection);
        }

        //Update animinations based on Player movement
         anim.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
         anim.SetBool("Grounded", charController.isGrounded);

        
    }

    public void Knockback()
    {
        isKnocking = true;
        knockbackCounter = knockbackLength;
        moveDirection.y = knockbackPower.y;
        charController.Move(moveDirection * Time.deltaTime);

    }

    public void Bounce()
    {
        moveDirection.y = bounceForce;
        charController.Move(moveDirection * Time.deltaTime);
    }
}
