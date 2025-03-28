using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Fields

    /* Reference field */
    public static PlayerController activePlayer;


    /* Player Status */
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float rotationSpeed = 20f;
    
    private float cameraVerticalAngle = 0f;
    private float interactRange = 3f;

    private GameObject heldItem;

    [SerializeField] private float speed;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool isInteracting; // 상호작용 중인지 추적
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isTouchingWall = false;


    /* Inputs */
    private InputActions inputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    

    /* Components */
    private Rigidbody rb;


    /* Assign on inspector */
    
    public Camera firstPersonCamera;
    public Animator bodyAnimator;
    public Animator chestAnimator;
    public Renderer chestRenderer;
    public Image targetDot;
    public Transform handTransform;

    #endregion



    #region Initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputActions();
        AddInputActions();

        NullErrorLog();
        
        activePlayer = this;
    }

    private void NullErrorLog()
    {
        if (bodyAnimator == null) Debug.LogError("bodyAnimator가 연결되지 않았습니다!");
        if (chestAnimator == null) Debug.LogError("chestAnimator가 연결되지 않았습니다!");
        if (firstPersonCamera == null) Debug.LogError("카메라가 연결되지 않았습니다!");
        if (chestRenderer == null) Debug.LogError("chestRenderer가 연결되지 않았습니다!");
        if (handTransform == null) Debug.LogError("handTransform이 연결되지 않았습니다!");
        if (targetDot == null) Debug.LogError("targetDot이 연결되지 않았습니다!");
    }

    private void AddInputActions()
    {
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.canceled += ctx => isRunning = false;
        inputActions.Player.Jump.performed += ctx => HandleJump();
        inputActions.Player.Interact.performed += ctx => HandleInteract();
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
        inputActions.Player.Die.performed += ctx => HandleDie();
    }

    private void OnDestroy()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        inputActions.Player.Run.performed -= ctx => isRunning = true;
        inputActions.Player.Run.canceled -= ctx => isRunning = false;
        inputActions.Player.Jump.performed -= ctx => HandleJump();
        inputActions.Player.Interact.performed -= ctx => HandleInteract();
        inputActions.Player.Look.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled -= ctx => lookInput = Vector2.zero;
        inputActions.Player.Die.performed -= ctx => HandleDie();
    }

    void OnEnable() => inputActions.Player.Enable();
    void OnDisable() => inputActions.Player.Disable();

    #endregion


    #region Update

    private void LateUpdate()
    {
        if (this == activePlayer) // 상호작용 중이 아니면 카메라 회전
        {
            if (!isInteracting && !isDead)
            {
                Move();
                Look();
                UpdateAnimation();
                UpdateTargetDot();
            }
            else
            {
                UpdateAnimation(); // 상호작용 중에도 애니메이션은 업데이트
                UpdateTargetDot(); // 하얀 점도 유지
            }
        }
    }

    // 충돌 감지로 지면 체크
    private void OnCollisionStay(Collision collision)
    {
        // "Ground" 태그가 있는 오브젝트와 충돌 중이면 접지 상태로 간주
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 contactNormal = collision.contacts[0].normal;
            if (Vector3.Dot(contactNormal, Vector3.up) < 0.5f)
            {
                Debug.Log("Wall detected");
                isTouchingWall = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 지면에서 떨어지면 접지 해제
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
        }
    }


    #endregion


    #region Player Control 
    private void Move()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 forwardMovement = transform.forward * moveInput.y;
        Vector3 sideMovement = transform.right * moveInput.x;
        Vector3 moveDirection = (forwardMovement + sideMovement).normalized;

        Vector3 moveVelocity = moveDirection * currentSpeed;

        // 벽에 닿았을 때 속도 조정
        if (isTouchingWall)
        {
            // 옵션 1: 수평 속도를 0으로
            // moveVelocity = Vector3.zero;

            // 옵션 2: 벽을 향한 속도 성분 제거 (더 자연스러움)
            RaycastHit hit;
            if (Physics.Raycast(transform.position, moveDirection, out hit, 1f) && hit.collider.CompareTag("Wall"))
            {
                Debug.Log("Wall raycast Checked");
                Vector3 wallNormal = hit.normal;
                moveVelocity = Vector3.ProjectOnPlane(moveVelocity, wallNormal).normalized * currentSpeed;
            }

            if (!isGrounded) // 지면에 닿지 않은 경우에만
            {
                //rb.AddForce(Vector3.down * 0.5f, ForceMode.Impulse);
            }
        }

        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);

        //bodyAnimator.SetFloat("Speed", moveVelocity.magnitude);
        //chestAnimator.SetFloat("Speed", moveVelocity.magnitude);

        if (currentSpeed > 0.1f)
        {
            bodyAnimator.SetBool("Run", isRunning);
            chestAnimator.SetBool("Run", isRunning);
        }
    }

    private void Look()
    {
        float horizontalRotation = lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, horizontalRotation, 0);

        cameraVerticalAngle -= lookInput.y * rotationSpeed * Time.deltaTime;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -90f, 63f);
        firstPersonCamera.transform.localRotation = Quaternion.Euler(cameraVerticalAngle, 0, 0);
    }

    private void UpdateAnimation()
    {
        speed = rb.linearVelocity.magnitude;
        bodyAnimator.SetFloat("Speed", speed);
        chestAnimator.SetFloat("Speed", speed);

        //if (isDead)
        //{
        //    HandleDie();
        //}
    }

    #endregion


    #region Action Handler

    public void HandleDie()
    {
        if (this == activePlayer)
        {
            isDead = true;
            GameManager.Instance.isPlayer1Dead = true;
            GameManager.Instance.isPlayer2Dead = true;
            bodyAnimator.SetTrigger("Die");
            chestAnimator.SetTrigger("Die");
            chestRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }

    public void HandleAlive()
    {
        if (this == activePlayer)
        {
            isDead = false;
            bodyAnimator.SetTrigger("Alive");
            chestAnimator.SetTrigger("Alive");
            bodyAnimator.Play("Idle", 0, 0f); // 즉시 Idle로 전환
            chestAnimator.Play("Idle", 0, 0f);
            chestRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    private void HandleJump()
    {
        if (this == activePlayer && !isInteracting && !isDead && isGrounded) // 지면에 닿아 있을 때만 점프
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            bodyAnimator.SetTrigger("Jump");
            chestAnimator.SetTrigger("Jump");
            Debug.Log("Jump!");
            isGrounded = false; // 점프 후 즉시 접지 해제 (중복 점프 방지)
        }
        else if (!isGrounded)
        {
            Debug.Log("Cannot jump: Not grounded!");
        }
    }

    private void HandleInteract()
    {
        if (this != activePlayer || isDead) return;

        if (heldItem != null)
        {
            DropItem(); // 아이템 버리기 (정지 없음)
            return;
        }

        Ray ray = firstPersonCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Pickup"))
            {
                PickUpItem(hit.collider.gameObject); // 아이템 줍기 (정지 없음)
            }
            //else if (hit.collider.CompareTag("HorizontalButton")) // WallButton
            else if (hit.collider.CompareTag("WallButton")) // WallButton
            {
                isInteracting = true; // 버튼 상호작용 시 정지 시작
                bodyAnimator.SetTrigger("Press");
                chestAnimator.SetTrigger("Press");
                hit.collider.gameObject.GetComponent<WallButton>().PressButton(); // 추가
                Debug.Log("Pressed HorizontalButton!");

                // 여기서 버튼 누르는 함수 호출

                Invoke(nameof(ResetInteracting), 0.5f); // 0.5초 후 해제
            }
            else if (hit.collider.CompareTag("VerticalButton")) {
                isInteracting = true; // 버튼 상호작용 시 정지 시작
                bodyAnimator.SetTrigger("Punch");
                chestAnimator.SetTrigger("Punch");
                Debug.Log("Punched VerticalButton!");
                Invoke(nameof(ResetInteracting), 0.5f); // 0.5초 후 해제
            }
            else
            {
                /* Do Nothing */
            }
        }
    }

    private void ResetInteracting()
    {
        isInteracting = false; // 상호작용 종료
    }

    private void PickUpItem(GameObject item)
    {
        heldItem = item;
        Rigidbody itemRb = heldItem.GetComponent<Rigidbody>();
        Collider itemCollider = heldItem.GetComponent<Collider>();

        itemRb.isKinematic = true;
        itemCollider.isTrigger = true;

        heldItem.transform.SetParent(handTransform);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;

        bodyAnimator.SetTrigger("PickUp");
        chestAnimator.SetTrigger("PickUp");
        Debug.Log("Picked up: " + item.name);
    }

    private void DropItem()
    {
        if (heldItem != null)
        {
            Rigidbody itemRb = heldItem.GetComponent<Rigidbody>();
            Collider itemCollider = heldItem.GetComponent<Collider>();

            heldItem.transform.SetParent(null);
            itemRb.isKinematic = false;
            itemCollider.isTrigger = false;

            heldItem = null;

            bodyAnimator.SetTrigger("Drop");
            chestAnimator.SetTrigger("Drop");
            Debug.Log("Dropped item");
        }
    }


    private void UpdateTargetDot()
    {
        Ray ray = firstPersonCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Pickup") ||
                hit.collider.CompareTag("HorizontalButton") ||
                hit.collider.CompareTag("WallButton") ||
                hit.collider.CompareTag("VerticalButton"))
            {
                targetDot.enabled = true;
            }
            else
            {
                targetDot.enabled = false;
            }
        }
        else
        {
            targetDot.enabled = false;
        }
    }

    #endregion
}