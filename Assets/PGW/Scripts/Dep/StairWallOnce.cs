using System.Collections;
using UnityEngine;

public class StairWallOnce : MonoBehaviour
{
    private Vector3 originalPosition; // 시작 위치
    private Vector3 targetPosition;   // 올라간 위치
    private bool isMoving = false;    // 이동 중인지 확인
    private Coroutine currentCoroutine;
    public float stayDuration = 3f;   // 계단이 올라간 상태로 유지되는 시간 (초)

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0); // Y축으로 20만큼 위로
    }

    // WallButtonOnce에서 호출될 메서드
    public void Activate()
    {
        if (!isMoving)
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(MoveUpAndStay());
        }
    }

    private IEnumerator MoveUpAndStay()
    {
        isMoving = true;
        transform.position = targetPosition;
        Debug.Log("Stair가 올라갔습니다.");

        // 지정된 시간 동안 유지
        yield return new WaitForSeconds(stayDuration);

        // 시간 후 내려옴
        transform.position = originalPosition;
        Debug.Log("Stair가 내려갔습니다.");
        isMoving = false;
        currentCoroutine = null;
    }
}