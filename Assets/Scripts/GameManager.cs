using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player1;
    public GameObject player2;
    public Camera camera1;
    public Camera camera2;
    [SerializeField] private int StageData;
    [SerializeField] private Vector3 player1SpawnPoint; // 영구 스폰 포인트
    [SerializeField] private Vector3 player2SpawnPoint; // 영구 스폰 포인트
    [SerializeField] private Vector3 tempPlayer1SpawnPoint; // 임시 스폰 포인트
    [SerializeField] private Vector3 tempPlayer2SpawnPoint; // 임시 스폰 포인트
    [SerializeField] private bool player1Passed;
    [SerializeField] private bool player2Passed;

    private bool isPlayer1Active = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        ActivatePlayer1();
        StageData = 1;
        player1SpawnPoint = player1.transform.position;
        player2SpawnPoint = player2.transform.position;
        tempPlayer1SpawnPoint = player1SpawnPoint; // 초기값 설정
        tempPlayer2SpawnPoint = player2SpawnPoint; // 초기값 설정
        player1Passed = false;
        player2Passed = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchPlayer();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayers(); // R 키로 테스트용 리스폰
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
        PlayerController.activePlayer = player1.GetComponent<PlayerController>();
        camera1.enabled = true;
        camera2.enabled = false;
        isPlayer1Active = true;
        Debug.Log("Player1 활성화됨");
    }

    void ActivatePlayer2()
    {
        PlayerController.activePlayer = player2.GetComponent<PlayerController>();
        camera1.enabled = false;
        camera2.enabled = true;
        isPlayer1Active = false;
        Debug.Log("Player2 활성화됨");
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
        player1.transform.position = player1SpawnPoint; // 영구 스폰 포인트로 이동
        player2.transform.position = player2SpawnPoint; // 영구 스폰 포인트로 이동
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
}