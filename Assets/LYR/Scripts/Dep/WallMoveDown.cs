using UnityEngine;

public class WallMoveDown : MonoBehaviour
{
    // 공용 변수
    public float moveDistance = 2f;    // 이동할 거리
    public float moveSpeed = 2f;       // 이동 속도

    // 내부 변수
    private Vector3 originalPosition;  // 원래 위치
    private Vector3 targetPosition;    // 목표 위치
    private bool isMoving = false;     // 이동 중인지 체크

    void Start()
    {
        // 시작 시 원래 위치 저장
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            // 부드럽게 목표 위치로 이동
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            // 목표 위치에 거의 도달했는지 체크
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // 정확히 목표 위치로 설정
                isMoving = false;                    // 이동 완료
            }
        }
    }

    public void Activate()
    {
        if (!isMoving)
        {
            isMoving = true;
            // 목표 위치를 Y축으로 moveDistance만큼 이동한 위치로 설정
            targetPosition = originalPosition + Vector3.up * moveDistance;
        }
    }
}
