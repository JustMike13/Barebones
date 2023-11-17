using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    private CharacterController controller;
    private GameObject mainCamera;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 10.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -18.0f;
    private float sensitivity = 2.0f;
    private float sprintMultiplier = 1.7f;
    private GameObject swordObject;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = GameObject.FindWithTag("MainCamera");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        swordObject = GameObject.FindWithTag("Sword");
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovement();
        ProcessCameraRotation();
        swordObject.GetComponent<Sword>().ProcessAttack();
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


        //Vector3 move = new Vector3(moveHorizontal, 0, moveVertical);
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            controller.Move(transform.forward * moveVertical * Time.deltaTime * playerSpeed * sprint);
            controller.Move(transform.right * moveHorizontal * Time.deltaTime * playerSpeed);
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
