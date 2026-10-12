using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3.0f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public float mouseSensitivity = 0.02f; // New Input system delta is pixel-based
    public Transform playerCamera;

    private CharacterController controller;
    private float verticalRotation = 0f;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Mouse Delta (Supports both New and Legacy cleanly)
        Vector2 mouseDelta = Vector2.zero;

        #if ENABLE_INPUT_SYSTEM
        if (Mouse.current != null)
        {
            mouseDelta = Mouse.current.delta.ReadValue();
        }
        #else
        mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 10f;
        #endif

        float mouseX = mouseDelta.x * mouseSensitivity *0.1f;
        float mouseY = mouseDelta.y * mouseSensitivity *0.1f;

        // Up - Down Pitch
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);

        if (playerCamera != null)
        {
            playerCamera.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
        }

        // Left - Right Yaw
        transform.Rotate(Vector3.up * mouseX);

        // 2. Keyboard Movement
        float moveX = 0f;
        float moveZ = 0f;

        #if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) moveZ += 1f;
            if (Keyboard.current.sKey.isPressed) moveZ -= 1f;
            if (Keyboard.current.dKey.isPressed) moveX += 1f;
            if (Keyboard.current.aKey.isPressed) moveX -= 1f;
        }
        #else
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        #endif

        Vector3 moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;
        controller.Move(moveDirection * walkSpeed * Time.deltaTime);

        // 3. Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}