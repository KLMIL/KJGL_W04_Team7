using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player1;
    public GameObject player2;
    public Camera camera1;
    public Camera camera2;
    [SerializeField] public int StageData;
    [SerializeField] private Vector3 player1SpawnPoint; // 영구 스폰 포인트
    [SerializeField] private Vector3 player2SpawnPoint; // 영구 스폰 포인트
    [SerializeField] private Vector3 tempPlayer1SpawnPoint; // 임시 스폰 포인트
    [SerializeField] private Vector3 tempPlayer2SpawnPoint; // 임시 스폰 포인트
    [SerializeField] private bool player1Passed;
    [SerializeField] private bool player2Passed;
    [SerializeField] public bool isPlayer1Dead;
    [SerializeField] public bool isPlayer2Dead;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject mapPrefab;

    private bool isPlayer1Active = true;
    private Camera mainCamera; // 주 화면 카메라
    private Camera subCamera;  // 보조 화면 카메라
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
    public void GameStart()
    {
        UIManager.Instance.howtoplayScreen.SetActive(false);
        ActivatePlayer1();
        Time.timeScale = 1f;
        UIManager.Instance.gameStartScreen.SetActive(false);
        StageData = 1;
        player1SpawnPoint = player1.transform.position;
        player2SpawnPoint = player2.transform.position;
        tempPlayer1SpawnPoint = player1SpawnPoint; // 초기값 설정
        tempPlayer2SpawnPoint = player2SpawnPoint; // 초기값 설정
        Cursor.lockState = CursorLockMode.Locked;
        player1Passed = false;
        player2Passed = false;
        SetCameras(); // 게임 시작 시 카메라 설정
    }
    void Start()
    {

        Time.timeScale = 0f;
        UIManager.Instance.gameOverScreen.SetActive(false);
        UIManager.Instance.gameStartScreen.SetActive(true);
        UIManager.Instance.escapeSuccess.SetActive(false);
        UIManager.Instance.stageText.text = "";




    }

    void Update()
    {
        UIManager.Instance.stageText.text = "Stage " + StageData;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchPlayer();
            SetCameras(); // Tab 키를 누를 때마다 카메라 재설정
        }
        if ((isPlayer1Dead || isPlayer2Dead) && Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayers(); // R 키로 테스트용 리스폰
        }

        if (player1.GetComponent<Transform>().position.y < -10f)
        {
            isPlayer1Dead = true;
        }
        if (player2.GetComponent<Transform>().position.y < -10f)
        {
            isPlayer2Dead = true;
        }
        if (isPlayer1Dead || isPlayer2Dead)
        {
            EndGame();
        }
    }

    void SwitchPlayer()
    {
        if (isPlayer1Active)
        {
            ActivatePlayer2();
        }
        else
        {
            ActivatePlayer1();
        }
    }

    void ActivatePlayer1()
    {
        player2.GetComponent<PlayerController>().DisablePlayer();
        PlayerController.activePlayer = player1.GetComponent<PlayerController>();
        isPlayer1Active = true;
        // Player1에 "Player" 태그 설정, Player2에서 태그 제거
        player1.tag = "Player";
        player2.tag = "Untagged"; // 또는 다른 태그로 변경
    }

    void ActivatePlayer2()
    {
        player1.GetComponent<PlayerController>().DisablePlayer();
        PlayerController.activePlayer = player2.GetComponent<PlayerController>();
        isPlayer1Active = false;
        player2.tag = "Player";
        player1.tag = "Untagged"; // 또는 다른 태그로 변경
    }

    public void PlusStageData()
    {
        StageData++;
    }

    public void ShowStageData()
    {
        Debug.Log("StageData: " + StageData);
    }

    // 임시 스폰 포인트 설정
    public void SetTempPlayer1SpawnPoint(Vector3 position)
    {
        tempPlayer1SpawnPoint = position;
        Debug.Log("Player1 temp spawn point set to: " + tempPlayer1SpawnPoint);
    }

    public void SetTempPlayer2SpawnPoint(Vector3 position)
    {
        tempPlayer2SpawnPoint = position;
        Debug.Log("Player2 temp spawn point set to: " + tempPlayer2SpawnPoint);
    }

    // 영구 스폰 포인트로 커밋
    public void CommitSpawnPoints()
    {
        player1SpawnPoint = tempPlayer1SpawnPoint;
        player2SpawnPoint = tempPlayer2SpawnPoint;
        Debug.Log("Spawn points committed - Player1: " + player1SpawnPoint + ", Player2: " + player2SpawnPoint);
    }

    public void RespawnPlayers()
    {
        // 기존 맵 파괴
        if (map != null)
        {
            Destroy(map);
            Debug.Log("기존 맵이 파괴되었습니다.");
        }

        // 새 맵 생성
        if (mapPrefab != null)
        {
            map = Instantiate(mapPrefab, new Vector3(94.362f, 7.14f, 162.228f), Quaternion.identity);
            Debug.Log("새 맵이 생성되었습니다.");
        }
        else
        {
            Debug.LogError("Map Prefab이 설정되지 않았습니다!");
        }

        // UI 및 플레이어 리스폰
        UIManager.Instance.gameOverScreen.SetActive(false);
        player1.transform.position = player1SpawnPoint; // 영구 스폰 포인트로 이동
        player2.transform.position = player2SpawnPoint; // 영구 스폰 포인트로 이동
        isPlayer1Dead = false;
        isPlayer2Dead = false;
        player1.GetComponent<PlayerController>().HandleAlive();
        player2.GetComponent<PlayerController>().HandleAlive();

        Debug.Log("Players respawned - Player1 at: " + player1SpawnPoint + ", Player2 at: " + player2SpawnPoint);
        ResetPassedFlags(); // 리스폰 시 플래그 초기화
    }



    public void SetPlayer1Passed(bool value)
    {
        player1Passed = value;
    }

    public void SetPlayer2Passed(bool value)
    {
        player2Passed = value;
    }

    public bool AreBothPlayersPassed()
    {
        return player1Passed && player2Passed;
    }

    public void ResetPassedFlags()
    {
        player1Passed = false;
        player2Passed = false;
    }
    public void SetPlayer1Dead(bool value)
    {
        isPlayer1Dead = value;
    }
    public void SetPlayer2Dead(bool value)
    {
        isPlayer2Dead = value;
    }
    private void EndGame()
    {
        UIManager.Instance.gameOverScreen.SetActive(true);
        Debug.Log("Game Over");
    }
    // 카메라를 주 화면과 보조 화면으로 설정하는 함수
    void SetCameras()
    {
        // 활성화된 플레이어에 따라 주 화면과 보조 화면 결정
        if (isPlayer1Active)
        {
            mainCamera = camera1;
            subCamera = camera2;
        }
        else
        {
            mainCamera = camera2;
            subCamera = camera1;
        }

        // 두 카메라 모두 활성화
        mainCamera.enabled = true;
        subCamera.enabled = true;

        // 주 화면 카메라: 화면 전체
        mainCamera.rect = new Rect(0, 0, 1, 1);
        mainCamera.depth = 0; // 주 화면이 위에 렌더링되도록

        // 보조 화면 카메라: 오른쪽 아래 작은 창
        subCamera.rect = new Rect(0.68f, 0.1f, 0.30f, 0.30f);
        subCamera.depth = 1; // 보조 화면이 아래에 렌더링되도록
    }
}