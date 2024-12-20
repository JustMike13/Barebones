using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonControllerCopy : MonoBehaviour
{
    private CharacterController controller;
    private GameObject mainCamera;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 10.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -18.0f;
    private float sensitivity = 2.0f;
    private float sprintMultiplier = 1.7f;
    [SerializeField] 
    float speedUpTime = 0.3f;
    float speedUpTimeElapsed = 0;
    [SerializeField] 
    float minSpeedMultiplier = 0.2f;
    float maxSpeedMultiplier = 1f;

    bool isStunned;
    public bool IsStunned { get { return isStunned; } }
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = GameObject.FindWithTag("MainCamera");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isStunned = false;
        animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //ProcessMovement();
        //ProcessCameraRotation();
    }

    private void ProcessMovement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -1.0f;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float sprint = Input.GetAxis("Shift") > 0 && groundedPlayer ? sprintMultiplier : 1.0f;

        if (moveHorizontal != 0 || moveVertical  != 0)
        {
            speedUpTimeElapsed += Time.deltaTime;
            if (speedUpTimeElapsed > speedUpTime)
            {
                speedUpTimeElapsed = speedUpTime;
            }
        }
        else
        {
            speedUpTimeElapsed = 0f;
        }

        float speedMultiplier = Mathf.InverseLerp(0f, speedUpTime, speedUpTimeElapsed);
        speedMultiplier = Mathf.Clamp(speedMultiplier, minSpeedMultiplier, maxSpeedMultiplier);

        //Vector3 move = new Vector3(moveHorizontal, 0, moveVertical);
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            controller.Move(transform.forward * moveVertical * Time.deltaTime * playerSpeed * speedMultiplier * sprint);
            controller.Move(transform.right * moveHorizontal * Time.deltaTime * playerSpeed * speedMultiplier);
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime); 
    }

    private void ProcessCameraRotation()
    {
        //camera rotation by mouse
        float mouse = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0, mouse * sensitivity, 0));
        mouse = Input.GetAxis("Mouse Y");
        mainCamera.transform.Rotate(new Vector3(-mouse * sensitivity, 0, 0));
    }
}
