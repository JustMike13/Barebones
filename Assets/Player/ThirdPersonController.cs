using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : BaseContoller
{
    private CharacterController controller;
    private CharacterManager characterManager;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    Transform characterModel;
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
    //Roll
    [SerializeField]
    float rollSpeed;
    [SerializeField]
    float rollDistance;
    [SerializeField]
    float rollDelay;
    float rollDistanceLeft;
    float lastRollEnd = 0;
    Vector3 rollStartingPoint;
    Vector3 rollDirection;
    bool isRolling;
    [SerializeField]
    AudioSource footsteps;

    float moveHorizontal;
    float moveVertical;

    // Start is called before the first frame update
    void Start()
    {
        characterManager = GetComponent<CharacterManager>(); 
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
        if (menu.IsGameON())
        {
            ProcessMovement();
            ProcessCameraRotation();
            ProcessAttacking();
            ProcessBlocking();
            ProcessHealing();
            ProcessRolling();
        }
    }

    private void ProcessMovement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -1.0f;
        }

        if (isStunned || IsBusy || isBlocking)
        {
            speedUpTimeElapsed = 0;
            return;
        }

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        //float sprint = 1f;//Input.GetAxis("Shift") > 0 && groundedPlayer ? sprintMultiplier : 1.0f;
        float movementSum = (moveHorizontal >= 0 ? moveHorizontal : -moveHorizontal) + (moveVertical >= 0 ? moveVertical : -moveVertical);
        if (movementSum > 0)
        {
            moveHorizontal = moveHorizontal / movementSum;
            moveVertical = moveVertical / movementSum;
        }

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
            footsteps.enabled = true;
            animator.SetBool("WalkingStraight", moveVertical != 0);
            //animator.SetFloat("WalkVertical", moveVertical);
            //animator.SetFloat("WalkHorizontal", moveHorizontal);
            animator.SetFloat("WalkSpeed", moveVertical < 0 ? speedMultiplier * -1 : speedMultiplier);
            animator.SetFloat("StrafeSpeed", moveHorizontal < 0 ? speedMultiplier * -1 : speedMultiplier * 1);
            controller.Move((transform.forward * moveVertical + transform.right * moveHorizontal) * Time.deltaTime * playerSpeed * speedMultiplier);
            //controller.Move(transform.right * moveHorizontal * Time.deltaTime * strafeSpeed * speedMultiplier);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            footsteps.enabled = false;
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
        healingAbility.IsHealing = Input.GetKey(KeyCode.F) && !isBlocking;
    }

    private void ProcessRolling()
    {
        if (!isRolling && Input.GetKeyDown(KeyCode.LeftShift) && !IsBusy && Time.time - lastRollEnd > rollDelay)
        {
            SetRollDirection(moveHorizontal, moveVertical);
            rollDirection = transform.forward * moveVertical + transform.right * moveHorizontal;
            rollStartingPoint = transform.position;
            IsBusy = true;
            animator.SetBool("IsRolling", true);
            animator.Play("Roll");
            isRolling = true;
            isInvulnerable = true;
        }
        if (isRolling)
        {
            if (Vector3.Distance(transform.position, rollStartingPoint) < rollDistance)
            {
                controller.Move(rollDirection * Time.deltaTime * rollSpeed);
            }
            else
            {
                lastRollEnd = Time.time;
                rollDistanceLeft = 0;
                animator.SetBool("IsRolling", false);
                characterModel.transform.rotation = transform.rotation;
                IsBusy = false;
                isRolling = false;
                isInvulnerable = false;
            }
        }
    }

    private void SetRollDirection(float horizontal, float vertical)
    {
        if (moveHorizontal == moveVertical && moveVertical == 0)
        {
            moveHorizontal = UnityEngine.Random.Range(0, 1) * 2 - 1; // -1 or 1
        }
        if (horizontal == 0 && vertical < 1)
        {
            characterModel.transform.Rotate(0, 180, 0);
        }
        else if (horizontal > 0)
        {
            characterModel.transform.Rotate(0, 90, 0);
        }
        else if(horizontal < 0)
        {
            characterModel.transform.Rotate(0, 270, 0);
        }
    }

    public override void Interrupt()
    {
        //animator.Play("Interrupt");
        if (healingAbility.IsHealing)
        {
            healingAbility.Interrupt();
        }
    }
}
