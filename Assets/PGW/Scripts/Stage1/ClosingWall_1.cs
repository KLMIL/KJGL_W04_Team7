using UnityEngine;

public class ClosingWall_1 : MonoBehaviour
{
    public float moveDistance = 7f;      // 벽이 열리는 거리
    public float moveSpeed = 1f;         // 돌아오는 속도

    private GameObject leftWall;
    private GameObject rightWall;
    public bool isPlayerPassed = false;
    public PlayerController playerController;
    private Vector3 leftOriginalPos;
    private Vector3 rightOriginalPos;
    private bool isOpening = false;
    private bool isClosing = false;
    private float closeTimer = 0f;

    void Awake()
    {
        leftWall = transform.GetChild(0).gameObject;
        rightWall = transform.GetChild(1).gameObject;
        leftOriginalPos = leftWall.transform.position;  // LeftWall의 초기 위치
        rightOriginalPos = rightWall.transform.position; // RightWall의 초기 위치

        // 자식 오브젝트 확인
        Debug.Log($"LeftWall 자식 수: {leftWall.transform.childCount}");
        Debug.Log($"RightWall 자식 수: {rightWall.transform.childCount}");
    }

    void Update()
    {
        if (isClosing)
        {
            closeTimer += Time.deltaTime;

            if (closeTimer >= 1f)
            {
                float t = (closeTimer - 1f) * moveSpeed;
                leftWall.transform.position = Vector3.Lerp(
                    leftOriginalPos - Vector3.right * moveDistance,
                    leftOriginalPos,
                    t
                );
                rightWall.transform.position = Vector3.Lerp(
                    rightOriginalPos + Vector3.right * moveDistance,
                    rightOriginalPos,
                    t
                );

                // 이동 중 자식 위치 로그
                if (leftWall.transform.childCount > 0)
                {
                    Debug.Log($"LeftWall 첫 번째 자식 위치: {leftWall.transform.GetChild(0).position}");
                }
                if (rightWall.transform.childCount > 0)
                {
                    Debug.Log($"RightWall 첫 번째 자식 위치: {rightWall.transform.GetChild(0).position}");
                }

                if (t >= 1f)
                {
                    isClosing = false;
                    if (!isPlayerPassed)
                    {
                        playerController.HandleDie();
                    }
                    closeTimer = 0f;
                    
                }
            }
        }
    }

    public void Activate()
    {
        if (!isOpening && !isClosing)
        {
            isOpening = true;

            leftWall.transform.position = leftOriginalPos - Vector3.right * moveDistance;
            rightWall.transform.position = rightOriginalPos + Vector3.right * moveDistance;

            // 활성화 후 위치 로그
            Debug.Log($"LeftWall 위치: {leftWall.transform.position}");
            Debug.Log($"RightWall 위치: {rightWall.transform.position}");
            if (leftWall.transform.childCount > 0)
            {
                Debug.Log($"LeftWall 첫 번째 자식 위치: {leftWall.transform.GetChild(0).position}");
            }
            if (rightWall.transform.childCount > 0)
            {
                Debug.Log($"RightWall 첫 번째 자식 위치: {rightWall.transform.GetChild(0).position}");
            }

            isOpening = false;
            isClosing = true;
            closeTimer = 0f;
        }
    }
}