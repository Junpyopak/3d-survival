using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("카메라 관련")]
    public Transform playerCamera;
    public float mouseSensitivity = 1f;
    public float minLookAngle = -30f;
    public float maxLookAngle = 30f;

    [Header("이동 관련")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float sitSpeed = 1.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("카메라 높이")]
    public float standHeight = 1.72f;
    public float sitHeight = 1.15f;
    public float cameraSpeed = 5f;

    [Header("카메라 숨쉬기")]
    public float sitBreathAmplitude = 0.05f;
    public float sitBreathSpeed = 2f;

    private float initialCameraY;
    public bool isSitting = false;

    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerStats stats;

    private float xRotation = 0f;
    private Vector3 velocity;
    private float targetCameraHeight;
    private IPlayerState currentState;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
        if (stats == null) Debug.LogWarning("PlayerStats가 없습니다!");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        initialCameraY = playerCamera.localPosition.y;
        targetCameraHeight = standHeight;
        SetState(new IdleState(this));
    }

    void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool runInput = Input.GetKey(KeyCode.LeftShift);
        bool sitInput = Input.GetKeyDown(KeyCode.C);

        currentState.KeyInput(moveInput, runInput, sitInput);
        currentState.Move();

        RotateCamera();
        UpdateCameraHeight();
        ApplySitBreath();
    }

    public void SetState(IPlayerState newState)
    {
        currentState = newState;
    }

    void ApplySitBreath()
    {
        if (!isSitting) return;
        Vector3 camPos = playerCamera.localPosition;
        camPos.y = targetCameraHeight + Mathf.Sin(Time.time * sitBreathSpeed) * sitBreathAmplitude;
        playerCamera.localPosition = camPos;
    }

    public void MovePlayer(float speed)
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void UpdateCameraHeight()
    {
        Vector3 camPos = playerCamera.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCameraHeight, Time.deltaTime * cameraSpeed);
        playerCamera.localPosition = camPos;
    }

    public void Sit()
    {
        controller.height = sitHeight;
        controller.center = new Vector3(0, sitHeight / 2f, 0);

        Vector3 pos = transform.position;
        pos.y = 0 + controller.height / 2f - controller.center.y;
        transform.position = pos;

        targetCameraHeight = sitHeight;
    }

    public void Stand()
    {
        controller.height = standHeight;
        controller.center = new Vector3(0, standHeight / 2f, 0);

        Vector3 pos = transform.position;
        pos.y = 0 + controller.height / 2f - controller.center.y;
        transform.position = pos;

        targetCameraHeight = standHeight;
    }
}
