using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThirdPersonController : MonoBehaviour
{
    int IDLE = 0;
    int ATTACKING = 1;
    int RETRIEVING = 2;
    private CharacterController controller;
    private GameObject mainCamera;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 10.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -18.0f;
    private float sensitivity = 2.0f;
    private float sprintMultiplier = 1.7f;
    public int swordState;
    private GameObject swordObject;
    private Quaternion swordIdleRotation;
    private Quaternion swordEndRotation = Quaternion.Euler(-64.487f, -127.796f, -134.9f);
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = GameObject.FindWithTag("MainCamera");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        swordObject = GameObject.FindWithTag("Sword");
        swordIdleRotation = swordObject.transform.rotation;
        swordState = IDLE;
        originalPosition = swordObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovement();
        ProcessCameraRotation();
        ProcessAttack();
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

    private void ProcessAttack()
    {
        float zRotation = swordObject.transform.localEulerAngles.z < 0 ? swordObject.transform.localEulerAngles.z + 360 : swordObject.transform.localEulerAngles.z;
        if (swordState == IDLE)
        {
            if (Input.GetAxis("Fire1")  != 0)
            {
                swordState = ATTACKING;
            }
        }
        if (swordState == RETRIEVING)
        {
            if (zRotation < 35 || zRotation > 225)
            {
                swordObject.transform.Rotate(new Vector3(0, 0, 2000) * Time.deltaTime);
                swordObject.transform.Translate(Vector3.left * 20 * Time.deltaTime);
            }
            else
            {
                swordObject.transform.localRotation = swordIdleRotation;
                swordObject.transform.localPosition = originalPosition;
                swordState = IDLE;
            }
        }
        if (swordState == ATTACKING)
        {
            if (zRotation < 35 || zRotation > 225)
            {
                swordObject.transform.Rotate(new Vector3(0, 0, -2000) * Time.deltaTime);
                swordObject.transform.Translate(Vector3.right * 20 * Time.deltaTime);
            }
            else
            {
                swordObject.transform.localRotation = swordEndRotation;
                swordState = RETRIEVING;
            }
        }
    }
}
