using UnityEngine;

public class ClosingWall : MonoBehaviour
{
    // 공용 변수
    public GameObject wallPrefab;        // 벽으로 사용할 큐브 프리팹
    public float initialDistance = 1f;   // 초기 생성 거리 (중심으로부터의 거리)
    public float moveDistance = 2f;      // 벽이 열리는 거리
    public float moveSpeed = 1f;         // 돌아오는 속도

    // 내부 변수
    private GameObject leftWall;
    private GameObject rightWall;
    private Vector3 leftOriginalPos;
    private Vector3 rightOriginalPos;
    private bool isOpening = false;
    private bool isClosing = false;
    private float closeTimer = 0f;

    void Start()
    {
        // 초기 벽 생성
        CreateWalls();
    }

    void Update()
    {
        if (isClosing)
        {
            closeTimer += Time.deltaTime;

            // 1초 대기 후 닫히기 시작
            if (closeTimer >= 1f)
            {
                // Lerp를 사용하여 부드럽게 원래 위치로 이동
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

                // 이동 완료 체크
                if (t >= 1f)
                {
                    isClosing = false;
                    closeTimer = 0f;
                }
            }
        }
        //Activate();
    }

    void CreateWalls()
    {
        // 왼쪽 벽 생성
        leftWall = Instantiate(wallPrefab, transform);
        leftWall.transform.localPosition = Vector3.left * initialDistance;
        leftOriginalPos = leftWall.transform.position;

        // 오른쪽 벽 생성
        rightWall = Instantiate(wallPrefab, transform);
        rightWall.transform.localPosition = Vector3.right * initialDistance;
        rightOriginalPos = rightWall.transform.position;
    }

    public void Activate()
    {
        if (!isOpening && !isClosing)
        {
            isOpening = true;

            // 즉시 열리는 동작
            leftWall.transform.position = leftOriginalPos - Vector3.right * moveDistance;
            rightWall.transform.position = rightOriginalPos + Vector3.right * moveDistance;

            isOpening = false;
            isClosing = true;
            closeTimer = 0f;
        }
    }
}
