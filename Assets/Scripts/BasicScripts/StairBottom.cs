using System.Collections;
using UnityEngine;

public class StairBottom : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    [SerializeField] protected BottomButton bottomButton; // 단일 BottomButton
    protected Coroutine currentCoroutine;
    private Rigidbody rb;

    [SerializeField] protected float moveSpeed = 30f; // 이동 속도

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0);

        // Rigidbody 설정
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; // Kinematic으로 설정
            rb.useGravity = false; // 중력 비활성화
            Debug.Log($"{gameObject.name}: Kinematic Rigidbody 추가됨");
        }

        // 버튼 상태 구독
        if (bottomButton != null)
        {
            bottomButton.onPressedStateChanged += HandleButtonState;
        }
        else
        {
            Debug.LogError($"{gameObject.name}: BottomButton이 설정되지 않았습니다.");
        }
    }

    public virtual void HandleButtonState(bool pressed, GameObject buttonObject)
    {
        if (buttonObject != bottomButton.gameObject)
        {
            return; // 다른 버튼의 입력은 무시
        }

        // 버튼이 눌렸을 때 길을 올림
        if (pressed)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveUp());
            Debug.Log($"{gameObject.name}: 버튼 눌림 - 길 올라감 시작");
        }
        // 버튼이 해제되었을 때 길을 내림
        else
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveDown());
            Debug.Log($"{gameObject.name}: 버튼 해제 - 길 내려감 시작");
        }
    }

    private IEnumerator MoveUp()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            yield return new WaitForFixedUpdate(); // FixedUpdate와 동기화
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
        }
        rb.MovePosition(targetPosition);
        Debug.Log($"{gameObject.name}: 길이 목표 위치에 도달");

        yield return null; // 필요 시 추가 대기 로직 삽입 가능
    }

    private IEnumerator MoveDown()
    {
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            yield return new WaitForFixedUpdate(); // FixedUpdate와 동기화
            rb.MovePosition(Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.fixedDeltaTime));
        }
        rb.MovePosition(originalPosition);
        Debug.Log($"{gameObject.name}: 길이 원래 위치로 복귀");
        currentCoroutine = null;
    }
}