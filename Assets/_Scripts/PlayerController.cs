using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public CharacterController characterController;

    [Header("Movement settings")]
    public float crouchSpeed = 3;
    public float walkSpeed = 5;
    public float runSpeed = 7;
    [Space(5)]
    public float jumpForce = 5;
    public float gravity = 9.81f;
    public float vSpeed;

    [Header("Ground Detection")]
    public GameObject groundCheck;
    public float groundCheckRadius;
    public LayerMask groundMask;

    [Header("Ceiling Detection")]
    public GameObject ceilingCheck;
    public float ceilingCheckRadius;
    private bool checkedCeiling;

    [Header("Booleans")]
    public bool isCrouching;
    public bool isWalking;
    public bool isRunning;
    public bool isGrounded;
    public bool isJumping;

    [Header("Input Variables")]
    public float Horizontal;
    public float Vertical;

    private Vector3 move;
    private Vector3 smoothMoveVelocity;
    private Vector3 currentMove;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        inputHandler();
    }

    private void FixedUpdate()
    {
        movementHandler();
    }

    public void movementHandler()
    {
        Vector3 targetMove = Vector3.zero;

        if (isCrouching)
        {
            targetMove = new Vector3(Horizontal, 0, Vertical).normalized * crouchSpeed;
            characterController.height = 1.25f;
            Camera.main.gameObject.transform.localPosition = new Vector3(0, Mathf.Lerp(Camera.main.gameObject.transform.localPosition.y, 0.5f, 25 * Time.fixedDeltaTime), 0);
        }
        else
        {
            characterController.height = 2f;
            Camera.main.gameObject.transform.localPosition = new Vector3(0, Mathf.Lerp(Camera.main.gameObject.transform.localPosition.y, 0.75f, 25 * Time.fixedDeltaTime), 0);
        }

        if (isWalking)
        {
            targetMove = new Vector3(Horizontal, 0, Vertical).normalized * walkSpeed;
        }

        if (isRunning)
        {
            targetMove = new Vector3(Horizontal, 0, Vertical).normalized * runSpeed;
        }

        currentMove = Vector3.SmoothDamp(currentMove, targetMove, ref smoothMoveVelocity, 0.1f);
        characterController.Move(transform.TransformDirection(currentMove) * Time.fixedDeltaTime);

        if (!isGrounded && !isJumping)
        {
            vSpeed -= gravity * Time.fixedDeltaTime;
        }
        else if (!isJumping && (vSpeed < 1 || isGrounded))
        {
            vSpeed = -2;
        }

        characterController.Move(new Vector3(0, vSpeed * Time.fixedDeltaTime, 0));
    }

    public void Jump()
    {
        StartCoroutine(jump());
    }

    public void MovePlayer(Vector3 position)
    {
        characterController.enabled = false;
        transform.position = position;
        characterController.enabled = true;
    }

    public void inputHandler()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckRadius, groundMask);

        if (Physics.CheckSphere(ceilingCheck.transform.position, ceilingCheckRadius, groundMask))
        {
            if (checkedCeiling == false)
            {
                vSpeed -= vSpeed;
                checkedCeiling = true;
            }
        }
        else
        {
            checkedCeiling = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isWalking = false;
            isRunning = true;
            isCrouching = false;
        }
        else if (!isRunning && Input.GetKey(KeyCode.LeftControl))
        {
            isWalking = false;
            isCrouching = true;
            isRunning = false;
        }
        else
        {
            isWalking = true;
            isRunning = false;
            isCrouching = false;
        }

        if (isGrounded && Input.GetButtonDown("Jump") && !isJumping)
        {
            Jump();
        }
    }

    IEnumerator jump()
    {
        isJumping = true;
        vSpeed += jumpForce + -vSpeed;
        yield return new WaitForSeconds(0.15f);
        isJumping = false;
    }
}