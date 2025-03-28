using System.Collections;
using UnityEngine;

public class TrapBottom : MonoBehaviour
{
    private bool isPlayerOn = false; // 플레이어가 바닥을 밟고 있는지 여부
    public float moveSpeed = 5f; // 올라가고 내려가는 속도
    private float targetY = 67f; // 목표 Y 값
    private bool isRising = false; // 상승 중인지 여부
    private Coroutine riseCoroutine; // 상승 코루틴 참조
    private Vector3 originalPosition; // 원래 위치 저장
    private bool hasReturned = false; // 복귀 완료 여부

    void Start()
    {
        // 원래 위치 저장
        originalPosition = transform.position;
    }

    void FixedUpdate()
    {
        // 상승 중이고 아직 복귀하지 않았다면 Y 값을 증가
        if (isRising && !hasReturned && transform.position.y < targetY)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, targetY, transform.position.z),
                moveSpeed * Time.deltaTime
            );
        }
    }

    // 플레이어가 바닥에 닿았을 때
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlayerOn && !hasReturned)
        {
            isPlayerOn = true;
            Debug.Log("플레이어가 바닥을 밟았습니다. 1초 후 상승 시작.");
            if (riseCoroutine != null) StopCoroutine(riseCoroutine); // 기존 코루틴 중지
            riseCoroutine = StartCoroutine(StartRisingAfterDelay());
        }
    }

    // 플레이어가 바닥에서 떠났을 때
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOn = false;
            isRising = false;
            if (riseCoroutine != null) StopCoroutine(riseCoroutine); // 코루틴 중지
            Debug.Log("플레이어가 바닥에서 떠났습니다. 상승 중지.");
        }
    }

    // 1초 대기 후 상승 시작
    private IEnumerator StartRisingAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // 0.5초 대기
        if (isPlayerOn && !hasReturned) // 플레이어가 여전히 밟고 있고 복귀하지 않았다면
        {
            isRising = true;
            Debug.Log("0.5초 후 상승 시작!");
        }
    }

    // 원래 위치로 돌아가는 메서드
    public void Activate()
    {
        if (riseCoroutine != null) StopCoroutine(riseCoroutine); // 상승 코루틴 중지
        isRising = false; // 상승 중지
        StartCoroutine(ReturnToOriginalPosition());
        Debug.Log("Activate 호출: 원래 위치로 복귀 시작.");
    }

    // 부드럽게 원래 위치로 복귀
    private IEnumerator ReturnToOriginalPosition()
    {
        while (transform.position.y > originalPosition.y)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                originalPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        transform.position = originalPosition; // 정확히 원래 위치로 설정
        hasReturned = true; // 복귀 완료 플래그 설정
        Debug.Log("원래 위치로 복귀 완료. 더 이상 올라가지 않습니다.");
    }
}