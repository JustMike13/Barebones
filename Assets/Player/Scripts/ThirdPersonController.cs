using AdvancedController;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;

public class ThirdPersonController : BaseContoller
{
    // Inspector variables
    [Header("Object References")]
    //[SerializeField]
    //private CinemachineVirtualCamera mainCamera;
    [SerializeField]
    SwordAttack sword;
    [SerializeField]
    OverlayMenu menu;
    [SerializeField]
    Transform characterModel;
    [SerializeField]
    TurnTowardController turnController;
    //[Header("Character values")]
    //[SerializeField]
    //private float playerSpeed = 10.0f;
    //[SerializeField]
    //float speedUpTime = 1f;
    //[SerializeField]
    //float minSpeedMultiplier = 0f;
    //[SerializeField]
    //float rollSpeed;
    //[SerializeField]
    //float rollDistance;
    //[SerializeField]
    //float rollDelay;
    //[Header("Camera")]
    //[SerializeField]
    //private float x_sensitivity = 2.0f;
    //[SerializeField]
    //private float y_sensitivity = 0.2f;
    //[SerializeField]
    //private float minCameraY = 2.5f;
    //[SerializeField]
    //private float maxCameraY = 4.5f;

    private AdvancedController.PlayerController movementController;
    private CharacterController controller;
    //private Vector3 playerVelocity;
    //private bool groundedPlayer;
    //private float jumpHeight = 1.0f;
    //private float gravityValue = -18.0f;
    //float speedUpTimeElapsed = 0;
    //float maxSpeedMultiplier = 1f; 
    HealingAbility healingAbility;
    //bool lockRotation = false;
    //public bool LockRotation {  get { return lockRotation; } set {  lockRotation = value; } }
    //float lastRollEnd = 0;
    //Vector3 rollStartingPoint;
    //Vector3 rollDirection;
    //bool isRolling;

    //float moveHorizontal;
    //float moveVertical;

    // Start is called before the first frame update
    void Start()
    {
        characterManager = GetComponent<CharacterManager>();
        movementController = GetComponent<AdvancedController.PlayerController>();
        controller = GetComponent<CharacterController>();
        healingAbility = GetComponent<HealingAbility>();
        animator = characterManager.Animator;
        isStunned = false;
        sword.Controller = this;
        healingAbility.Controller = this;
    }

    // Update is called once per frame
    void Update()
    {
        //if (menu.IsGameON())
        //{
            //ProcessMovement();
            //ProcessCameraRotation();
            ProcessAttacking();
            ProcessBlocking();
            ProcessHealing();
            //ProcessRolling();
        //}
        movementController.SetAllowMovement(!(isStunned || IsBusy || isBlocking));
    }

    //private void ProcessMovement()
    //{
    //    groundedPlayer = controller.isGrounded;
    //    if (groundedPlayer && playerVelocity.y < 0)
    //    {
    //        playerVelocity.y = -1.0f;
    //    }

    //    if (isStunned || IsBusy || isBlocking)
    //    {
    //        speedUpTimeElapsed = 0;
    //        return;
    //    }

    //    moveHorizontal = Input.GetAxis("Horizontal");
    //    moveVertical = Input.GetAxis("Vertical");
    //    //float sprint = 1f;//Input.GetAxis("Shift") > 0 && groundedPlayer ? sprintMultiplier : 1.0f;
    //    float movementSum = (moveHorizontal >= 0 ? moveHorizontal : -moveHorizontal) + (moveVertical >= 0 ? moveVertical : -moveVertical);
    //    if (movementSum > 0)
    //    {
    //        moveHorizontal = moveHorizontal / movementSum;
    //        moveVertical = moveVertical / movementSum;
    //    }

    //    if (moveHorizontal != 0 || moveVertical  != 0)
    //    {
    //        speedUpTimeElapsed += Time.deltaTime;
    //        if (speedUpTimeElapsed > speedUpTime)
    //        {
    //            speedUpTimeElapsed = speedUpTime;
    //        }
    //    }
    //    else
    //    {
    //        speedUpTimeElapsed = 0f;
    //    }

    //    float speedMultiplier = Mathf.InverseLerp(0f, speedUpTime, speedUpTimeElapsed);
    //    speedMultiplier = speedMultiplier * (maxSpeedMultiplier - minSpeedMultiplier) + minSpeedMultiplier;

    //    if (moveHorizontal != 0 || moveVertical != 0)
    //    {
    //        // Movement
    //        controller.Move((transform.forward * moveVertical + transform.right * moveHorizontal) * Time.deltaTime * playerSpeed * speedMultiplier);
    //        // Sound
    //        footsteps.enabled = true;
    //        // Animation
    //        animator.SetBool("IsWalking", true);
    //        animator.SetBool("WalkingStraight", moveVertical != 0);
    //        animator.SetFloat("WalkSpeed", moveVertical < 0 ? -1 : 1);
    //        animator.SetFloat("StrafeSpeed", moveHorizontal < 0 ? -1 : 1);
    //        animator.SetBool("Strafing", moveHorizontal != 0);
    //        //if (moveVertical == 0 || moveHorizontal == 0)
    //        //{
    //        //    animator.SetLayerWeight(animator.GetLayerIndex("Base Layer"), 1f);
    //        //    animator.SetLayerWeight(animator.GetLayerIndex("Strafing"), 0f);
    //        //}
    //        //else
    //        //{
    //        //    animator.SetLayerWeight(animator.GetLayerIndex("Base Layer"), moveVertical < 0 ? moveVertical * -1 : moveVertical);
    //        //    animator.SetLayerWeight(animator.GetLayerIndex("Strafing"), moveHorizontal < 0 ? moveHorizontal * -1: moveHorizontal);
    //        //}
    //    }
    //    else
    //    {
    //        animator.SetBool("IsWalking", false);
    //        footsteps.enabled = false;
    //    }



    //    // Changes the height position of the player..
    //    if (Input.GetButtonDown("Jump") && groundedPlayer)
    //    {
    //        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    //    }

    //    playerVelocity.y += gravityValue * Time.deltaTime;
    //    controller.Move(playerVelocity * Time.deltaTime);
    //}

    //private void ProcessCameraRotation()
    //{
    //    if (lockRotation)
    //    {
    //        return;
    //    }
    //    //camera rotation by mouse
    //    float mouse = Input.GetAxis("Mouse X");
    //    transform.Rotate(new Vector3(0, mouse * x_sensitivity, 0));
    //    mouse = Input.GetAxis("Mouse Y");
    //    //mainCamera.transform.Rotate(new Vector3(-mouse * sensitivity, 0, 0));
    //    Cinemachine3rdPersonFollow camera = mainCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    //    camera.ShoulderOffset.y = Mathf.Clamp(camera.ShoulderOffset.y - mouse * y_sensitivity, minCameraY, maxCameraY);

    //}
    
    private void ProcessAttacking()
    {
        if (Input.GetButtonDown("Fire1") && !isBlocking)
        {
            RotateCharacterToCamera();
            sword.UseAttack();
        }
    }

    private void ProcessBlocking()
    {
        bool oldIsBlocking = isBlocking;
        isBlocking = Input.GetKey(KeyCode.Mouse1) && !IsAttacking ? true : false;
        if (isBlocking && !oldIsBlocking) 
        {
            RotateCharacterToCamera();
            StartBlockingTime = Time.time;
        }
        else if (!isBlocking && oldIsBlocking)
        {
            StartBlockingTime = Mathf.Infinity;
        }
    }

    private void ProcessHealing()
    {
        healingAbility.IsHealing = Input.GetKey(KeyCode.F) && !isBlocking;
    }

    //private void ProcessRolling()
    //{
    //    if (!isRolling && Input.GetKeyDown(KeyCode.LeftShift) && !IsBusy && Time.time - lastRollEnd > rollDelay)
    //    {
    //        SetRollDirection(moveHorizontal, moveVertical);
    //        rollDirection = transform.forward * moveVertical + transform.right * moveHorizontal;
    //        rollStartingPoint = transform.position;
    //        IsBusy = true;
    //        animator.SetBool("IsRolling", true);
    //        animator.Play("Roll");
    //        isRolling = true;
    //        isInvulnerable = true;
    //    }
    //    if (isRolling)
    //    {
    //        if (Vector3.Distance(transform.position, rollStartingPoint) < rollDistance)
    //        {
    //            controller.Move(rollDirection * Time.deltaTime * rollSpeed);
    //        }
    //        else
    //        {
    //            lastRollEnd = Time.time;
    //            animator.SetBool("IsRolling", false);
    //            characterModel.transform.rotation = transform.rotation;
    //            IsBusy = false;
    //            isRolling = false;
    //            isInvulnerable = false;
    //        }
    //    }
    //}

    //private void SetRollDirection(float horizontal, float vertical)
    //{
    //    if (moveHorizontal == moveVertical && moveVertical == 0)
    //    {
    //        moveHorizontal = UnityEngine.Random.Range(0, 1) * 2 - 1; // -1 or 1
    //    }
    //    if (horizontal == 0 && vertical < 1)
    //    {
    //        characterModel.transform.Rotate(0, 180, 0);
    //    }
    //    else if (horizontal > 0)
    //    {
    //        characterModel.transform.Rotate(0, 90, 0);
    //    }
    //    else if(horizontal < 0)
    //    {
    //        characterModel.transform.Rotate(0, 270, 0);
    //    }
    //}

    public override void Interrupt()
    {
        //animator.Play("Interrupt");
        if (healingAbility.IsHealing)
        {
            healingAbility.Interrupt();
        }
        if (sword.IsAttacking)
        {
            sword.Interrupt();
        }
    }

    void RotateCharacterToCamera()
    {
        turnController.RotateToCamera();
    }
}
