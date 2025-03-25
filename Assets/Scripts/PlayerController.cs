using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float rotationSpeed = 1f;

    private InputActions inputActions;
    private Vector2 moveInput;
    private bool isRunning;
    private Rigidbody rb;
    public Animator animator;

    public static PlayerController activePlayer;

    #region LifeCycle
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (animator == null)
        {
            Debug.LogError("Animator가 연결되지 않았습니다!");
        }
        inputActions = new InputActions();

        activePlayer = this;

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Run.performed += ctx => { isRunning = true; animator.SetTrigger("Run"); }; // 즉시 Run
        inputActions.Player.Run.canceled += ctx => { isRunning = false; animator.SetTrigger("Idle"); }; // 즉시 Idle
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

    void FixedUpdate()
    {
        if (this == activePlayer)
        {
            Move();
            // UpdateAnimation 제거: 트리거로 직접 제어
        }
    }

    void OnDestroy()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        inputActions.Player.Run.performed -= ctx => { isRunning = true; animator.SetTrigger("Run"); };
        inputActions.Player.Run.canceled -= ctx => { isRunning = false; animator.SetTrigger("Idle"); };
        inputActions.Player.Jump.performed -= ctx => HandleJump();
        inputActions.Player.Interact.performed -= ctx => HandleInteract();
        inputActions.Player.CameraSwitch.performed -= ctx => HandleSwitch();
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

        if (moveInput.x != 0 && moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(sideMovement.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void HandleJump()
    {
        if (this == activePlayer && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
            Debug.Log("Jump!");
        }
    }

    private void HandleInteract()
    {
        if (this == activePlayer)
        {
            animator.SetTrigger("Interact");
            Debug.Log("Interact pressed! (F 키)");
        }
    }

    private void HandleSwitch()
    {
        Debug.Log("Switch pressed! (Tab 키)");
    }
}