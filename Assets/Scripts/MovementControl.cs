using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float jumpSpeed;

    [SerializeField]
    private Transform cameraTransform;

    private CharacterController characterController;
    private float ySpeed;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 movementDirection = new Vector3(horizontalInput,0,verticalInput); 
        float magnitude  = Mathf.Clamp01(movementDirection.magnitude) *speed;

        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y,Vector3.up)*movementDirection;

        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if(Input.GetButtonDown("Jump")){
            ySpeed  = jumpSpeed;
        }



        //transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

        Vector3 velocity = movementDirection*magnitude;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        if(movementDirection!= Vector3.zero){
            animator.SetBool("isMoving",true);

            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotation,rotationSpeed*Time.deltaTime);

        } 
        else{
            animator.SetBool("isMoving",false);
        }
    }

    private void onApplicationFocus(bool focus){
        if(focus){
            Cursor.lockState = CursorLockMode.Locked;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
