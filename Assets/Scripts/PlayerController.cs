using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float rotationSpeed = 20f;
    public Camera firstPersonCamera;

    [SerializeField] private float speed;

    private InputActions inputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    [SerializeField] private bool isRunning;
    private Rigidbody rb;
    public Animator bodyAnimator;
    public Animator chestAnimator;
    private float cameraVerticalAngle = 0f;

    public static PlayerController activePlayer;

    #region LifeCycle
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (bodyAnimator == null) Debug.LogError("bodyAnimator가 연결되지 않았습니다!");
        if (chestAnimator == null) Debug.LogError("chestAnimator가 연결되지 않았습니다!");
        if (firstPersonCamera == null) Debug.LogError("카메라가 연결되지 않았습니다!");
        inputActions = new InputActions();

        activePlayer = this;

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Run.performed += ctx => { isRunning = true; UpdateSpeedImmediately(); }; // 구독 추가
        inputActions.Player.Run.canceled += ctx => { isRunning = false; UpdateSpeedImmediately(); }; // 구독 추가
        inputActions.Player.Jump.performed += ctx => HandleJump();
        inputActions.Player.Interact.performed += ctx => HandleInteract();
        inputActions.Player.CameraSwitch.performed += ctx => HandleSwitch();
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable() => inputActions.Player.Enable();
    void OnDisable() => inputActions.Player.Disable();

    void FixedUpdate()
    {
        if (this == activePlayer)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        if (this == activePlayer)
        {
            Look();
            UpdateAnimation();
        }
    }

    void OnDestroy()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        inputActions.Player.Run.performed -= ctx => { isRunning = true; UpdateSpeedImmediately(); };
        inputActions.Player.Run.canceled -= ctx => { isRunning = false; UpdateSpeedImmediately(); };
        inputActions.Player.Jump.performed -= ctx => HandleJump();
        inputActions.Player.Interact.performed -= ctx => HandleInteract();
        inputActions.Player.CameraSwitch.performed -= ctx => HandleSwitch();
        inputActions.Player.Look.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled -= ctx => lookInput = Vector2.zero;
    }
    #endregion

    private void Move()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 forwardMovement = transform.forward * moveInput.y;
        Vector3 sideMovement = transform.right * moveInput.x;
        Vector3 moveDirection = (forwardMovement + sideMovement).normalized;

        Vector3 moveVelocity = moveDirection * currentSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }

    private void UpdateSpeedImmediately()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 forwardMovement = transform.forward * moveInput.y;
        Vector3 sideMovement = transform.right * moveInput.x;
        Vector3 moveDirection = (forwardMovement + sideMovement).normalized;

        Vector3 moveVelocity = moveDirection * currentSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
        bodyAnimator.SetFloat("Speed", moveVelocity.magnitude); // Speed로 통일
        chestAnimator.SetFloat("Speed", moveVelocity.magnitude); // Speed로 통일
        
        if (currentSpeed > 0.1f)
        {
            bodyAnimator.SetBool("Run", isRunning);
            chestAnimator.SetBool("Run", isRunning);
        }
    }

    private void Look()
    {
        float horizontalRotation = lookInput.x * rotationSpeed * Time.deltaTime; // LateUpdate이니 deltaTime 사용
        transform.Rotate(0, horizontalRotation, 0);

        cameraVerticalAngle -= lookInput.y * rotationSpeed * Time.deltaTime;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -90f, 63f); // 위 90도, 아래 50도
        firstPersonCamera.transform.localRotation = Quaternion.Euler(cameraVerticalAngle, 0, 0);
    }

    private void UpdateAnimation()
    {
        speed = rb.linearVelocity.magnitude;
        bodyAnimator.SetFloat("Speed", speed);
        chestAnimator.SetFloat("Speed", speed);
    }

    private void HandleJump()
    {
        if (this == activePlayer && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            bodyAnimator.SetTrigger("Jump");
            chestAnimator.SetTrigger("Jump");
            Debug.Log("Jump!");
        }
    }

    private void HandleInteract()
    {
        if (this == activePlayer)
        {
            bodyAnimator.SetTrigger("Interact");
            chestAnimator.SetTrigger("Interact");
            Debug.Log("Interact pressed! (F 키)");
        }
    }

    private void HandleSwitch()
    {
        Debug.Log("Switch pressed! (Tab 키)");
    }
}