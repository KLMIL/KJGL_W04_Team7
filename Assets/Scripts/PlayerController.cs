using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [SerializeField] private bool tmp_die;
    public Renderer chestRenderer;

    private float interactRange = 3f;
    private GameObject heldItem;
    public Transform handTransform;

    public Image targetDot;

    #region LifeCycle
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (bodyAnimator == null) Debug.LogError("bodyAnimator가 연결되지 않았습니다!");
        if (chestAnimator == null) Debug.LogError("chestAnimator가 연결되지 않았습니다!");
        if (firstPersonCamera == null) Debug.LogError("카메라가 연결되지 않았습니다!");
        if (chestRenderer == null) Debug.LogError("chestRenderer가 연결되지 않았습니다!");
        if (handTransform == null) Debug.LogError("handTransform이 연결되지 않았습니다!");
        if (targetDot == null) Debug.LogError("targetDot이 연결되지 않았습니다!");
        inputActions = new InputActions();

        activePlayer = this;

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.canceled += ctx => isRunning = false;
        inputActions.Player.Jump.performed += ctx => HandleJump();
        inputActions.Player.Interact.performed += ctx => HandleInteract();
        inputActions.Player.CameraSwitch.performed += ctx => HandleSwitch();
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.TEMP_Die.performed += ctx => tmp_die = true;
        inputActions.Player.TEMP_Die.canceled += ctx => tmp_die = false;

        //Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable() => inputActions.Player.Enable();
    void OnDisable() => inputActions.Player.Disable();

    void FixedUpdate()
    {
        if (this == activePlayer)
        {
            Move();
            UpdateAnimation();
        }
    }

    private void LateUpdate()
    {
        if (this == activePlayer)
        {
            Look();
            UpdateAnimation();
            UpdateTargetDot(); // 하얀 점 업데이트
        }
    }

    void OnDestroy()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        inputActions.Player.Run.performed -= ctx => isRunning = true;
        inputActions.Player.Run.canceled -= ctx => isRunning = false;
        inputActions.Player.Jump.performed -= ctx => HandleJump();
        inputActions.Player.Interact.performed -= ctx => HandleInteract();
        inputActions.Player.CameraSwitch.performed -= ctx => HandleSwitch();
        inputActions.Player.Look.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled -= ctx => lookInput = Vector2.zero;
        inputActions.Player.TEMP_Die.performed -= ctx => tmp_die = true;
        inputActions.Player.TEMP_Die.canceled -= ctx => tmp_die = false;
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

        bodyAnimator.SetFloat("Speed", moveVelocity.magnitude);
        chestAnimator.SetFloat("Speed", moveVelocity.magnitude);

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

        if (tmp_die)
        {
            HandleDie();
        }
    }

    public void HandleAlive()
    {
        if (this == activePlayer)
        {
            bodyAnimator.SetTrigger("Alive");
            chestAnimator.SetTrigger("Alive");
            chestRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    private void HandleDie()
    {
        if (this == activePlayer)
        {
            bodyAnimator.SetTrigger("Die");
            chestAnimator.SetTrigger("Die");
            chestRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
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
        if (this != activePlayer) return;

        if (heldItem != null)
        {
            DropItem();
        }
        else
        {
            Ray ray = firstPersonCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactRange))
            {
                if (hit.collider.CompareTag("Pickup"))
                {

                    PickUpItem(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("HorizontalButton"))
                {
                    // 손가락으로 누르기
                    bodyAnimator.SetTrigger("Press");
                    chestAnimator.SetTrigger("Press");
                    Debug.Log("Pressed HorizontalButton!");
                }
                else if (hit.collider.CompareTag("VerticalButton"))
                {
                    // 주먹으로 치기
                    bodyAnimator.SetTrigger("Punch");
                    chestAnimator.SetTrigger("Punch");
                    Debug.Log("Punched VerticalButton!");
                }
            }
        }

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

        bodyAnimator.SetTrigger("PickUp"); // 새로운 PickUp 트리거
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

            bodyAnimator.SetTrigger("Drop"); // 새로운 Drop 트리거
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

    private void HandleSwitch()
    {
        Debug.Log("Switch pressed! (Tab 키)");
    }
}