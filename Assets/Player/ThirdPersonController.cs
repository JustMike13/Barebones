using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : BaseContoller
{
    private CharacterController controller;
    [SerializeField]
    private GameObject mainCamera;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 10.0f;
    [SerializeField]
    private float strafeSpeed = 5.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -18.0f;
    private float sensitivity = 2.0f;
    private float sprintMultiplier = 1.7f;
    [SerializeField]
    float speedUpTime = 1f;
    float speedUpTimeElapsed = 0;
    [SerializeField]
    float minSpeedMultiplier = 0f;
    float maxSpeedMultiplier = 1f;
    [SerializeField]
    SwordAttack sword;
    HealingAbility healingAbility;
    bool lockRotation = false;
    public bool LockRotation {  get { return lockRotation; } set {  lockRotation = value; } }
    [SerializeField]
    GameObject deathCameraPrefab;
    [SerializeField]
    OverlayMenu menu;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        healingAbility = GetComponent<HealingAbility>();
        isStunned = false;
        sword.Controller = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (menu.IsGameON())
        {
            ProcessMovement();
            ProcessCameraRotation();
            ProcessAttacking();
            ProcessBlocking();
            ProcessHealing();
        }
    }

    private void ProcessMovement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -1.0f;
        }

        if (isStunned || IsBusy)
        {
            speedUpTimeElapsed = 0;
            return;
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
        speedMultiplier = speedMultiplier * (maxSpeedMultiplier - minSpeedMultiplier) + minSpeedMultiplier;

        //Vector3 move = new Vector3(moveHorizontal, 0, moveVertical);
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("WalkingStraight", moveVertical != 0);
            animator.SetFloat("WalkSpeed", moveVertical < 0 ? speedMultiplier * -1 : speedMultiplier);
            animator.SetFloat("StrafeSpeed", moveHorizontal < 0 ? speedMultiplier * -2 : speedMultiplier * 2);
            controller.Move(transform.forward * moveVertical * Time.deltaTime * playerSpeed * speedMultiplier * sprint);
            controller.Move(transform.right * moveHorizontal * Time.deltaTime * strafeSpeed * speedMultiplier);
        }
        else
        {
            animator.SetBool("IsWalking", false);
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
        if (lockRotation)
        {
            return;
        }
        //camera rotation by mouse
        float mouse = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0, mouse * sensitivity, 0));
        mouse = Input.GetAxis("Mouse Y");
        mainCamera.transform.Rotate(new Vector3(-mouse * sensitivity, 0, 0));
    }
    
    private void ProcessAttacking()
    {
        if (Input.GetButtonDown("Fire1") && !isBlocking)
        {
            sword.UseAttack();
        }
    }

    private void ProcessBlocking()
    {
        bool oldIsBlocking = isBlocking;
        isBlocking = Input.GetKey(KeyCode.Mouse1) && !IsAttacking ? true : false;
        if (isBlocking && !oldIsBlocking) 
        { 
            StartBlockingTime = Time.time;
        }
        else if (!isBlocking && oldIsBlocking)
        {
            StartBlockingTime = Mathf.Infinity;
        }
    }

    private void ProcessHealing()
    {
        healingAbility.IsHealing = Input.GetKey(KeyCode.E) && !isBlocking;
    }

    public override void Interrupt()
    {
        if (sword.IsAttacking)
        {
            sword.Interrupt();
        }
        if (healingAbility.IsHealing)
        {
            healingAbility.Interrupt();
        }
    }
}
