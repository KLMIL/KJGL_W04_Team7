using UnityEngine;

public class WordSManager : MonoBehaviour
{
    public GameObject button;
    public GameObject[] doors;
    public GameObject wall;

    private bool _isClosing = false; // private으로 유지

    // 프로퍼티로 외부 접근 허용
    public bool isClosing
    {
        get { return _isClosing; }
        set { _isClosing = value; }
    }


    private float speed = 1f;


    private GameObject player;
    [SerializeField] private float damageDelay = 0.5f;

    [SerializeField]
    private bool isRightWallTriggered = false; // movingWallRight의 트리거 상태
    [SerializeField]
    private bool isLeftWallTriggered = false;  // movingWallLeft의 트리거 상태


    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject;
        isClosing = false;
    }

    void Update()
    {
        if (isClosing)
        {
            // 벽의 왼쪽과 오른쪽 부분 가져오기
            Transform wallLeft = wall.transform.GetChild(0);
            Transform wallRight = wall.transform.GetChild(1);

            // 중앙 위치
            Vector3 wallCenter = wall.transform.position;

            // 목표 위치 설정 (센터에서 오프셋 적용)
            Vector3 targetLeft = wallCenter + new Vector3(4.4f, 0f, 0f);  // left는 +4
            Vector3 targetRight = wallCenter + new Vector3(-4.4f, 0f, 0f); // right는 -4

            // 현재 위치에서 목표 위치까지의 거리 계산
            float distanceLeft = Vector3.Distance(wallLeft.position, targetLeft);
            float distanceRight = Vector3.Distance(wallRight.position, targetRight);

            // 이동 방향 계산
            Vector3 directionLeft = (targetLeft - wallLeft.position).normalized;
            Vector3 directionRight = (targetRight - wallRight.position).normalized;

            // 1초에 1단위 이동하도록 속도 적용
            Vector3 newLeftPos = wallLeft.position + directionLeft * speed * Time.deltaTime;
            Vector3 newRightPos = wallRight.position + directionRight * speed * Time.deltaTime;

            // 목표를 지나치지 않도록 제한
            if (Vector3.Distance(wallLeft.position, targetLeft) > Vector3.Distance(newLeftPos, targetLeft))
            {
                wallLeft.position = newLeftPos;
            }
            else
            {
                wallLeft.position = targetLeft;
            }

            if (Vector3.Distance(wallRight.position, targetRight) > Vector3.Distance(newRightPos, targetRight))
            {
                wallRight.position = newRightPos;
            }
            else
            {
                wallRight.position = targetRight;
            }

            // 목표 위치에 도달했는지 체크
            if (Vector3.Distance(wallLeft.position, targetLeft) < 0.01f &&
                Vector3.Distance(wallRight.position, targetRight) < 0.01f)
            {
                isClosing = false;
                wallLeft.position = targetLeft;   // 정확히 목표 위치로 맞춤
                wallRight.position = targetRight;
            }
        }

        if (isRightWallTriggered && isLeftWallTriggered)
        {
            KillPlayer();
        }

    }


    public void ButtonListen(GameObject button)
    {
        foreach (GameObject door in doors)
        {
            door.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void wallClosing()
    {
        Vector3 wallLeft = wall.transform.GetChild(0).position;
        Vector3 wallRight = wall.transform.GetChild(1).position;

        Vector3 wallCenter = wall.transform.position;

        isClosing = true;  // 이동 시작 플래그



    }



    // WallTrigger에서 호출되는 메서드
    public void OnWallTriggerEnter(string wallName)
    {
        if (wallName == "movingWallRight")
        {
            isRightWallTriggered = true;
        }
        else if (wallName == "movingWallLeft")
        {
            isLeftWallTriggered = true;
        }
    }

    public void OnWallTriggerExit(string wallName)
    {
        if (wallName == "movingWallRight")
        {
            isRightWallTriggered = false;
        }
        else if (wallName == "movingWallLeft")
        {
            isLeftWallTriggered = false;
        }
    }




    void KillPlayer()
    {
        if (player != null)
        {
            GameManager.Instance.SetPlayerDead(true);
        }
    }

    

}
