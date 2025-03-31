using UnityEngine;

public class Stage1_TranBridge : MonoBehaviour
{
    public float moveDistance = 7f;      // 문이 열리는 거리
    public float moveSpeed = 1f;         // 열리는 속도

    private GameObject leftWall;
    private GameObject rightWall;
    private Vector3 leftOriginalPos;
    private Vector3 rightOriginalPos;
    private bool isOpening = false;
    private bool isOpened = false;       // 문이 열려있는지 상태를 추적
    private float openProgress = 0f;

    void Start()
    {
        leftWall = transform.GetChild(0).gameObject;
        rightWall = transform.GetChild(1).gameObject;
        leftOriginalPos = leftWall.transform.position;  // LeftWall의 초기 위치 (닫힌 상태)
        rightOriginalPos = rightWall.transform.position; // RightWall의 초기 위치 (닫힌 상태)


        Debug.Log($"leftwall: {leftWall.name}");
    }

    void Update()
    {
        if (isOpening)
        {
            openProgress += Time.deltaTime * moveSpeed;

            // 문을 열리는 방향으로 이동
            leftWall.transform.position = Vector3.Lerp(
                leftOriginalPos,
                leftOriginalPos - Vector3.forward * moveDistance,
                openProgress
            );
            rightWall.transform.position = Vector3.Lerp(
                rightOriginalPos,
                rightOriginalPos + Vector3.forward * moveDistance,
                openProgress
            );

            // 열림이 완료되면 정지
            if (openProgress >= 1f)
            {
                isOpening = false;
                isOpened = true;    // 문이 완전히 열렸음을 표시
                openProgress = 1f;
            }
        }
    }

    public void Activate()
    {
        // 문이 열려있지 않고, 열리는 중이 아닐 때만 작동
        if (!isOpened && !isOpening)
        {
            isOpening = true;
            openProgress = 0f;
        }
    }


    public void DeActivate()
    {
        if (isOpened)
        {
            leftWall.transform.position = leftOriginalPos;
            rightWall.transform.position = rightOriginalPos;
            isOpened = false;
        }
    }
}