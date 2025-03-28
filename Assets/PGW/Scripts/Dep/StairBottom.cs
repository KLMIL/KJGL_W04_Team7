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
        transform.position = targetPosition;

        // 하나라도 눌려 있는 동안 유지
        while (bottomButtons.Any(b => b != null && b.IsPressed()))
        {
            yield return null;
        }

        transform.position = originalPosition;
        isMoving = false;
        currentCoroutine = null;
    }
}