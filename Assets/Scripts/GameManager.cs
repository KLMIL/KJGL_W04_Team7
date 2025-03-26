using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 플레이어와 카메라를 Inspector에서 연결하기 위한 변수
    public GameObject player1;
    public GameObject player2;
    public Camera camera1;
    public Camera camera2;

    // 현재 활성화된 플레이어 상태를 추적하기 위한 변수
    private bool isPlayer1Active = true;

    void Start()
    {
        // 게임 시작 시 초기 설정: Player1과 Camera1 활성화
        ActivatePlayer1();
    }

    void Update()
    {
        // Tab 키를 눌렀을 때 카메라와 플레이어 전환
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchPlayer();
        }
    }

    // 플레이어와 카메라를 전환하는 메서드
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

    // Player1과 Camera1을 활성화하는 메서드
    void ActivatePlayer1()
    {
        // PlayerController의 activePlayer를 Player1으로 설정
        PlayerController.activePlayer = player1.GetComponent<PlayerController>();

        // Camera1 활성화
        camera1.enabled = true;
        camera2.enabled = false;

        // 플레이어 오브젝트 활성화/비활성화 (선택 사항)
        player1.SetActive(true);
        player2.SetActive(false);

        isPlayer1Active = true;
        Debug.Log("Player1 활성화됨");
    }

    // Player2와 Camera2를 활성화하는 메서드
    void ActivatePlayer2()
    {
        // PlayerController의 activePlayer를 Player2로 설정
        PlayerController.activePlayer = player2.GetComponent<PlayerController>();

        // Camera2 활성화
        camera1.enabled = false;
        camera2.enabled = true;

        // 플레이어 오브젝트 활성화/비활성화 (선택 사항)
        player1.SetActive(false);
        player2.SetActive(true);

        isPlayer1Active = false;
        Debug.Log("Player2 활성화됨");
    }
}