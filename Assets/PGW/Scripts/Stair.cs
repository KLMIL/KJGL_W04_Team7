using UnityEngine;
using System.Collections;
using System.Linq; // Any 메서드 사용을 위해 추가

public class Stair : MonoBehaviour
{
    private Vector3 originalPosition; // 시작 위치
    private Vector3 targetPosition;   // 올라간 위치
    public BottomButton[] buttons;   // 퍼블릭 버튼 배열 (인스펙터에서 연결)
    private bool isMoving = false;    // 이동 중인지 확인
    private Coroutine currentCoroutine;

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0); // Y축으로 20만큼 위로
        if (buttons == null || buttons.Length == 0)
        {
            Debug.LogError("ButtonTrigger 배열이 비어 있습니다. 인스펙터에서 버튼을 연결하세요.");
        }
    }

    public void Activate()
    {
        if (!isMoving)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveUpAndStay());
        }
    }

    private IEnumerator MoveUpAndStay()
    {
        isMoving = true;

        // 순식간에 올라감
        transform.position = targetPosition;

        // 모든 버튼 중 하나라도 눌려 있는 동안 대기
        while (buttons.Any(b => b != null && b.IsPressed()))
        {
            yield return null; // 매 프레임 확인
        }

        // 모든 버튼이 해제되면 내려옴
        transform.position = originalPosition;

        isMoving = false;
        currentCoroutine = null;
    }
}