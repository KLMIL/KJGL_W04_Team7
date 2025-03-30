using UnityEngine;
using System.Collections;

public class PuzzleBottom : StairBottom
{
    private bool isPlayerTouching = false;
    [SerializeField] private string playerTag = "Player";
    private Vector3 originalPosition; // Initial position (middle)
    private Vector3 upPosition;       // Highest position (initial + 12)
    private Vector3 downPosition;     // Lowest position (initial - 8)
    private ChildTriggerDetector childTriggerDetector; // �ڽ��� Ʈ���� ������
    private bool isButtonPressed = false; // ��ư�� ���� ���¸� �����ϴ� ����
    [SerializeField] private float returnSpeed = 2f; // ���ڸ��� ���ư��� �ӵ� (�⺻�� 2)

    void Start()
    {
        originalPosition = transform.position;              // Initial position
        upPosition = originalPosition + new Vector3(0, 12f, 0);   // Highest position: initial + 12
        downPosition = originalPosition + new Vector3(0, -8f, 0); // Lowest position: initial - 8

        // ù ��° �ڽĿ��� ChildTriggerDetector ã��
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

            // ���� �̺�Ʈ ����(isButtonPressed)�� ���� HandleButtonState ȣ��
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

            // �÷��̾ ������ �ʱ� ��ġ�� �̵� (returnSpeed ���)
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
        // ��ư ���� ������Ʈ
        isButtonPressed = pressed;

        if (pressed && isPlayerTouching)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveTo(upPosition, moveSpeed)); // moveSpeed ���
            Debug.Log($"{gameObject.name}: Button pressed - moving up to highest position.");
        }
        else if (isPlayerTouching)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveTo(downPosition, moveSpeed)); // moveSpeed ���
            Debug.Log($"{gameObject.name}: Button released - moving down to lowest position.");
        }
    }

    // Movement coroutine (�ӵ��� �Ű������� ����)
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
        // ������ �� ������Ʈ�̹Ƿ� Collider�� ����� ��
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Debug.LogWarning($"{gameObject.name}: This object should be empty and not have a Collider.");
            Destroy(col); // �ʿ��ϸ� Collider ����
        }
    }
}