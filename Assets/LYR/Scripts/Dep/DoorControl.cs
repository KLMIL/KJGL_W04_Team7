using UnityEngine;

public class DoorControl : MonoBehaviour
{
    // 공용 변수
    public float targetAngle = 90f;      // 열리는 목표 각도
    public float openSpeed = 2f;         // 열리는 속도
    public float closeSpeed = 1f;        // 닫히는 속도
    public bool autoClose = false;       // 자동 닫힘 여부
    public float openDuration = 0f;      // 열린 상태 유지 시간 (autoClose가 true일 때만 적용)

    // 내부 변수
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private bool isOpening = false;
    private bool isClosing = false;
    private float timer = 0f;

    void Start()
    {
        // 초기 회전값 저장
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isOpening)
        {
            // 부드럽게 열림
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);

            // 목표 각도에 도달했는지 체크
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isOpening = false;

                if (autoClose)
                {
                    timer = 0f;
                    isClosing = true;
                }
            }
        }
        else if (isClosing)
        {
            timer += Time.deltaTime;

            if (timer >= openDuration)
            {
                // 부드럽게 닫힘 (별도의 닫힘 속도 사용)
                transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * closeSpeed);

                // 원래 각도에 도달했는지 체크
                if (Quaternion.Angle(transform.rotation, originalRotation) < 0.1f)
                {
                    transform.rotation = originalRotation;
                    isClosing = false;
                }
            }
        }
        //Activate();
    }

    public void Activate()
    {
        if (!isOpening && !isClosing)
        {
            isOpening = true;
            targetRotation = originalRotation * Quaternion.Euler(0, targetAngle, 0);
        }
    }

    public void CloseDoor()
    {
        isOpening = false;
        isClosing = true;
    }
}
