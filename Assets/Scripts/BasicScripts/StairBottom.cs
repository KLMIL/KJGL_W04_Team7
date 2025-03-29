using System.Collections;
using System.Linq;
using UnityEngine;

public class StairBottom : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    public BottomButton[] bottomButtons; // BottomButton 배열
    private bool isMoving = false;
    private Coroutine currentCoroutine;

    [SerializeField] private float moveSpeed = 30f; // 올라오는 속도 (조정 가능)
    [SerializeField] private float stayDuration = 0.1f; // 올라간 후 유지 시간 (조정 가능)

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0);

        // 버튼 상태 구독
        foreach (var button in bottomButtons)
        {
            if (button != null)
            {
                button.onPressedStateChanged += HandleButtonState; // 상태 변경 시 호출
            }
        }

        if (bottomButtons == null || bottomButtons.Length == 0)
        {
            Debug.LogError("BottomButton 배열이 비어 있습니다.");
        }
    }

    private void HandleButtonState(bool pressed, GameObject buttonObject)
    {
        // buttonObject가 bottomButtons에 포함되어 있는지 확인
        if (pressed && !isMoving && bottomButtons.Any(b => b != null && b.gameObject == buttonObject))
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(MoveUpAndStay());
        }
    }

    private IEnumerator MoveUpAndStay()
    {
        isMoving = true;

        // 부드럽게 올라감
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition; // 정확히 목표 위치에 도달

        // 하나라도 눌려 있는 동안 유지
        while (bottomButtons.Any(b => b != null && b.IsPressed()))
        {
            yield return null;
        }

        // 부드럽게 내려감
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = originalPosition; // 정확히 원래 위치에 도달

        isMoving = false;
        currentCoroutine = null;
    }
}