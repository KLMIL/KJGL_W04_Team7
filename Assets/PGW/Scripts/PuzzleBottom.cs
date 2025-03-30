using UnityEngine;
using System.Collections;

public class PuzzleBottom : StairBottom
{
    private bool isPlayerTouching = false;
    [SerializeField] private string playerTag = "Player";
    private Vector3 originalPosition; // Initial position (middle)
    private Vector3 upPosition;       // Highest position (initial + 12)
    private Vector3 downPosition;     // Lowest position (initial - 8)
    private ChildTriggerDetector childTriggerDetector; // 자식의 트리거 감지기
    private bool isButtonPressed = false; // 버튼의 현재 상태를 추적하는 변수
    [SerializeField] private float returnSpeed = 2f; // 제자리로 돌아가는 속도 (기본값 2)

    void Start()
    {
        originalPosition = transform.position;              // Initial position
        upPosition = originalPosition + new Vector3(0, 12f, 0);   // Highest position: initial + 12
        downPosition = originalPosition + new Vector3(0, -8f, 0); // Lowest position: initial - 8

        // 첫 번째 자식에서 ChildTriggerDetector 찾기
        childTriggerDetector = GetComponentInChildren<ChildTriggerDetector>();
        if (childTriggerDetector != null)
        {
            childTriggerDetector.onTriggerEnter += OnChildTriggerEnter;
            childTriggerDetector.onTriggerExit += OnChildTriggerExit;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: ChildTriggerDetector not found in children.");
        }

        // Rigidbody setup handled by parent class
        if (bottomButton != null)
        {
            bottomButton.onPressedStateChanged += HandleButtonState;
        }
        else
        {
            Debug.LogError($"{gameObject.name}: BottomButton is not set.");
        }
    }

    private void OnChildTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) || other.CompareTag("DisablePlayer"))
        {
            isPlayerTouching = true;
            Debug.Log($"{gameObject.name}: Player has made contact with child.");

            // 현재 이벤트 상태(isButtonPressed)에 따라 HandleButtonState 호출
            if (bottomButton != null)
            {
                HandleButtonState(isButtonPressed, bottomButton.gameObject);
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: Cannot handle button state - bottomButton is null.");
            }
        }
    }

    private void OnChildTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) || other.CompareTag("DisablePlayer"))
        {
            isPlayerTouching = false;
            Debug.Log($"{gameObject.name}: Player has lost contact with child.");

            // 플레이어가 떠나면 초기 위치로 이동 (returnSpeed 사용)
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveTo(originalPosition, returnSpeed));
            Debug.Log($"{gameObject.name}: Player left - returning to original position with speed {returnSpeed}.");
        }
    }

    // HandleButtonState implemented directly
    public override void HandleButtonState(bool pressed, GameObject buttonObject)
    {
        // 버튼 상태 업데이트
        isButtonPressed = pressed;

        if (pressed && isPlayerTouching)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveTo(upPosition, moveSpeed)); // moveSpeed 사용
            Debug.Log($"{gameObject.name}: Button pressed - moving up to highest position.");
        }
        else if (isPlayerTouching)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveTo(downPosition, moveSpeed)); // moveSpeed 사용
            Debug.Log($"{gameObject.name}: Button released - moving down to lowest position.");
        }
    }

    // Movement coroutine (속도를 매개변수로 받음)
    private IEnumerator MoveTo(Vector3 target, float speed)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            yield return new WaitForFixedUpdate(); // Sync with FixedUpdate
            rb.MovePosition(Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime));
        }
        rb.MovePosition(target);
        Debug.Log($"{gameObject.name}: Reached target position ({target.y}) with speed {speed}.");
        currentCoroutine = null;
    }

    protected void Awake()
    {
        // 본인은 빈 오브젝트이므로 Collider가 없어야 함
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Debug.LogWarning($"{gameObject.name}: This object should be empty and not have a Collider.");
            Destroy(col); // 필요하면 Collider 제거
        }
    }
}