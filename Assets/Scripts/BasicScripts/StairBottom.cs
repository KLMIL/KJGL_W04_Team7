using System.Collections;
using System.Linq;
using UnityEngine;

public class StairBottom : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    public BottomButton[] bottomButtons; // BottomButton 배열
    private Coroutine currentCoroutine;

    [SerializeField] private float moveSpeed = 30f; // 이동 속도

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0);

        // 버튼 상태 구독
        foreach (var button in bottomButtons)
        {
            if (button != null)
            {
                button.onPressedStateChanged += HandleButtonState;
            }
        }

        if (bottomButtons == null || bottomButtons.Length == 0)
        {
            Debug.LogError("BottomButton 배열이 비어 있습니다.");
        }
    }

    public void HandleButtonState(bool pressed, GameObject buttonObject)
    {
        if (bottomButtons.Any(b => b != null && b.gameObject == buttonObject))
        {
            // 버튼이 눌렸을 때 길을 올림
            if (pressed && currentCoroutine == null)
            {
                if (currentCoroutine != null) StopCoroutine(currentCoroutine);
                currentCoroutine = StartCoroutine(MoveUp());
                Debug.Log($"{gameObject.name}: 버튼 눌림 - 길 올라감 시작");
            }
            // 모든 버튼이 해제되었을 때 길을 내림
            else if (!pressed && !bottomButtons.Any(b => b != null && b.IsPressed()))
            {
                if (currentCoroutine != null) StopCoroutine(currentCoroutine);
                currentCoroutine = StartCoroutine(MoveDown());
                Debug.Log($"{gameObject.name}: 모든 버튼 해제 - 길 내려감 시작");
            }
        }
    }

    private IEnumerator MoveUp()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        Debug.Log($"{gameObject.name}: 길이 목표 위치에 도달");

        // 버튼이 눌려 있는 동안 유지
        while (bottomButtons.Any(b => b != null && b.IsPressed()))
        {
            yield return null;
        }

        // 버튼이 모두 해제되면 내려감
        currentCoroutine = StartCoroutine(MoveDown());
    }

    private IEnumerator MoveDown()
    {
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = originalPosition;
        Debug.Log($"{gameObject.name}: 길이 원래 위치로 복귀");
        currentCoroutine = null;
    }
}