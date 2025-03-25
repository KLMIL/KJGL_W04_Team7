using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 이동 관련 변수
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float rotationSpeed = 1f; // 회전 속도 (부드럽게 회전하도록)

    private InputActions inputActions;
    private Vector2 moveInput;
    private bool isRunning;
    private Rigidbody rb;


    #region LifeCycle
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Input Actions 초기화
        inputActions = new InputActions();

        // 이벤트 연결
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.canceled += ctx => isRunning = false;
        inputActions.Player.Jump.performed += ctx => HandleJump();
        inputActions.Player.Interact.performed += ctx => HandleInteract();
        inputActions.Player.CameraSwitch.performed += ctx => HandleSwitch();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
    }

    void OnDestroy()
    {
        // 메모리 누수 방지
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        inputActions.Player.Run.performed -= ctx => isRunning = true;
        inputActions.Player.Run.canceled -= ctx => isRunning = false;
        inputActions.Player.Jump.performed -= ctx => HandleJump();
        inputActions.Player.Interact.performed -= ctx => HandleInteract();
        inputActions.Player.CameraSwitch.performed -= ctx => HandleSwitch();
    }

    #endregion


    private void Move()
    {
        // 이동 방향 계산
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 forwardMovement = transform.forward * moveInput.y; // W: 앞, S: 뒤
        Vector3 sideMovement = transform.right * moveInput.x;     // A: 왼쪽, D: 오른쪽
        Vector3 moveDirection = (forwardMovement + sideMovement).normalized;

        // 이동 적용
        Vector3 moveVelocity = moveDirection * currentSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);

        // 회전 (A/D 입력 시 이동 방향으로 회전)
        if (moveInput.x != 0 && moveDirection != Vector3.zero) // 좌우 이동 중일 때만 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(sideMovement.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void HandleJump()
    {
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jump!");
        }
    }

    private void HandleInteract()
    {
        Debug.Log("Interact pressed! (F 키)");
    }

    private void HandleSwitch()
    {
        Debug.Log("Switch pressed! (Tab 키)");
    }
}