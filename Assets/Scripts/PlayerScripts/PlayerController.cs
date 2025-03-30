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

    private bool isCursorHintOffer = false; // TEST Hint Test

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
    public Image cursorDot;
    public RawImage cursorHint; // TEST Hint Test
    public Transform handTransform;


    /* Component in same Object */
    private PlayerCameraShake cameraShake;


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

    private void Start()
    {
        cameraShake = gameObject.GetComponent<PlayerCameraShake>();
    }

    private void NullErrorLog()
    {
        if (bodyAnimator == null) Debug.LogError("bodyAnimator가 연결되지 않았습니다!");
        if (chestAnimator == null) Debug.LogError("chestAnimator가 연결되지 않았습니다!");
        if (firstPersonCamera == null) Debug.LogError("카메라가 연결되지 않았습니다!");
        if (chestRenderer == null) Debug.LogError("chestRenderer가 연결되지 않았습니다!");
        if (handTransform == null) Debug.LogError("handTransform이 연결되지 않았습니다!");
        if (cursorDot == null) Debug.LogError("cursorDot이 연결되지 않았습니다!");
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
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    ShakeCameraDoorOpen();
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    ShakeCameraWallOpen();
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    isCursorHintOffer = !isCursorHintOffer;
        //}

        if (this == activePlayer) // 상호작용 중이 아니면 카메라 회전
        {
            if (!isInteracting && !isDead)
            {
                Move();
                Look();
                UpdateAnimation();
                UpdatecursorDot();
            }
            else
            {
                UpdateAnimation(); // 상호작용 중에도 애니메이션은 업데이트
                UpdatecursorDot(); // 하얀 점도 유지
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

        if (isTouchingWall)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, moveDirection, out hit, 5f) && hit.collider.CompareTag("Wall"))
            {
                Debug.Log("Wall raycast Checked");
                Vector3 wallNormal = hit.normal;
                // 수평 속도를 0으로 설정 (대신 ProjectOnPlane 제거 가능)
                moveVelocity = Vector3.zero;
                Debug.Log("MoveVelocity reset to zero: " + moveVelocity);
            }
        }

        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
        //Debug.Log("Velocity Y after move: " + rb.linearVelocity.y);

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
    }

    public void DisablePlayer()
    {
        //Debug.Log("Is it Called?1");

        // 애니메이션 강제로 Idle 설정
        bodyAnimator.Play("Idle", 0, 0f);
        chestAnimator.Play("Idle", 0, 0f);

        // Speed 파라미터 초기화
        bodyAnimator.SetFloat("Speed", 0f);
        chestAnimator.SetFloat("Speed", 0f);

        // 트리거 초기화 (필요 시)
        bodyAnimator.ResetTrigger("Jump");
        chestAnimator.ResetTrigger("Jump");
        // 다른 트리거도 초기화 필요 시 추가

        rb.linearVelocity = Vector3.zero;
        isInteracting = false;

        cursorDot.enabled = false;
        cursorHint.enabled = false;

        //Debug.Log("Is it Called?2");
        Debug.Log("Body Animator State: " + bodyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        Debug.Log("Chest Animator State: " + chestAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
    }

    public void ActivePlayerUI()
    {
        cursorDot.enabled = true;
    }

    #endregion


    #region Action Handler

    public void HandleDie()
    {
        if (this == activePlayer)
        {
            isDead = true;
            //GameManager.Instance.isPlayer1Dead = true;
            //GameManager.Instance.isPlayer2Dead = true;
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
        if (this != activePlayer || isDead || isInteracting) return;

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
                Invoke(nameof(ResetInteracting), 0.2f); // 0.5초 후 해제
            }
            //else if (hit.collider.CompareTag("VerticalButton")) {
            //    isInteracting = true; // 버튼 상호작용 시 정지 시작
            //    bodyAnimator.SetTrigger("Punch");
            //    chestAnimator.SetTrigger("Punch");
            //    Debug.Log("Punched VerticalButton!");
            //    Invoke(nameof(ResetInteracting), 0.5f); // 0.5초 후 해제
            //}
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


    private void UpdatecursorDot()
    {
        Ray ray = firstPersonCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Pickup") ||
                //hit.collider.CompareTag("HorizontalButton") ||
                hit.collider.CompareTag("WallButton")
                //hit.collider.CompareTag("VerticalButton"))
                )
            {
                //cursorDot.enabled = true;
                cursorDot.color = Color.green;

                //if (isCursorHintOffer)
                if (GameManager.Instance.StageData == 1) // 좀 하드코딩임. GameManager 통해서 직접 얻음.
                {
                    cursorHint.enabled = true; // TEST Hint Test
                }

            }
            else
            {
                //cursorDot.enabled = false;
                cursorDot.color = Color.white;
                cursorHint.enabled = false; // TEST Hint Test
            }
        }
        else
        {
            //cursorDot.enabled = false;
            cursorDot.color = Color.white;
            cursorHint.enabled = false; // TEST Hint Test
        }
    }

    #endregion


    #region Camera
    public void ShakeCameraDoorOpen()
    {
        cameraShake.ShakeCamera(0.2f, 0.02f, 1.0f);
    }

    public void ShakeCameraWallOpen()
    {
        cameraShake.ShakeCamera(0.5f, 0.1f, 1.0f);
    }

    #endregion
}