using UnityEngine;
using System.Collections;
using System.Linq;

public class StairWallDuration : MonoBehaviour
{
    private Vector3 originalPosition; // 시작 위치
    private Vector3 targetPosition;   // 올라간 위치
    public WallButtonDuration[] wallButtons; // WallButtonDuration 배열
    private bool isMoving = false;           // 이동 중인지 확인
    private Coroutine currentCoroutine;
    private int activeButtonCount = 0;       // 활성화된 버튼 수 추적

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0); // Y축으로 20만큼 위로

        // WallButtonDuration 이벤트 구독
        foreach (var button in wallButtons)
        {
            if (button != null)
            {
                button.onPressedStateChanged += HandleButtonState;
                button.onIntervalTriggered += HandleIntervalTrigger;
            }
        }

        if (wallButtons == null || wallButtons.Length == 0)
        {
            Debug.LogError("WallButtonDuration 배열이 비어 있습니다. 인스펙터에서 버튼을 연결하세요.");
        }
    }

    void OnDestroy()
    {
        // WallButtonDuration 이벤트 구독 해제
        foreach (var button in wallButtons)
        {
            if (button != null)
            {
                button.onPressedStateChanged -= HandleButtonState;
                button.onIntervalTriggered -= HandleIntervalTrigger;
            }
        }
    }

    private void HandleButtonState(bool active, GameObject gameObject)
    {
        if (active && wallButtons.Any(b => b != null && b.gameObject == gameObject))
        {
            activeButtonCount++; // 활성화된 버튼 수 증가
            if (!isMoving)
            {
                if (currentCoroutine != null) StopCoroutine(currentCoroutine);
                currentCoroutine = StartCoroutine(MoveUpAndStay());
            }
        }
        else
        {
            activeButtonCount--; // 활성화된 버튼 수 감소
            if (activeButtonCount < 0) activeButtonCount = 0; // 음수 방지
        }
    }

    private void HandleIntervalTrigger()
    {
        // 주기적 이벤트 처리 (필요 시 추가 로직 구현)
        Debug.Log("WallButtonDuration 간격 트리거 발생 - 계단 유지 중");
    }

    private IEnumerator MoveUpAndStay()
    {
        isMoving = true;
        transform.position = targetPosition;
        Debug.Log("Stair가 올라갔습니다.");

        // 하나라도 활성화된 버튼이 있는 동안 유지
        while (activeButtonCount > 0)
        {
            yield return null; // 매 프레임 확인
        }

        // 모든 버튼이 비활성화되면 내려옴
        transform.position = originalPosition;
        Debug.Log("Stair가 내려갔습니다.");
        isMoving = false;
        currentCoroutine = null;
    }
}
