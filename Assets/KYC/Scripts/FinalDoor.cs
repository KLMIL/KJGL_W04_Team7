using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    private StageLast stageLast; // StageLast 참조
    private bool hasPlayer1Reached = false; // player1이 도달했는지
    private bool hasPlayer2Reached = false; // player2가 도달했는지

    void Start()
    {
        // StageLast 컴포넌트 가져오기
        stageLast = FindFirstObjectByType<StageLast>();
        if (stageLast == null)
        {
            Debug.LogError("StageLast를 찾을 수 없어!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (stageLast == null || stageLast.gameComplete == false) return; // 게임 완료 전에는 동작 안 함

        // 플레이어 태그로 충돌 감지
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError("충돌한 오브젝트에 PlayerController가 없어!");
                return;
            }

            // 현재 플레이어 비활성화
            playerController.enabled = false;
            Debug.Log($"{other.gameObject.name}의 조종이 비활성화됐어!");

            // 플레이어 확인 및 상태 업데이트
            if (other.gameObject == stageLast.player1)
            {
                hasPlayer1Reached = true;
                Debug.Log("player1이 EndDoor에 도달했어!");
            }
            else if (other.gameObject == stageLast.player2)
            {
                hasPlayer2Reached = true;
                Debug.Log("player2가 EndDoor에 도달했어!");
            }

            // 다른 플레이어로 전환
            SwitchPlayer();

            // 두 플레이어가 모두 도달했는지 확인
            if (hasPlayer1Reached && hasPlayer2Reached)
            {
                ShowGameOverScreen();
            }
        }
    }

    private void SwitchPlayer()
    {
        if (stageLast.player1 == null || stageLast.player2 == null) return;

        // 현재 비활성화된 플레이어 확인 후 다른 플레이어 활성화
        if (!stageLast.player1.GetComponent<PlayerController>().enabled)
        {
            stageLast.player2.GetComponent<PlayerController>().enabled = true;
            Debug.Log("player2로 전환했어!");
        }
        else if (!stageLast.player2.GetComponent<PlayerController>().enabled)
        {
            stageLast.player1.GetComponent<PlayerController>().enabled = true;
            Debug.Log("player1로 전환했어!");
        }
    }

    private void ShowGameOverScreen()
    {
        UIManager uiManager = UIManager.Instance;
        if (uiManager != null)
        {
            UIManager.Instance.ShowSuccessScreen();
            Debug.Log("게임 종료 화면 띄웠어!");
        }
        else
        {
            Debug.LogError("UIManager를 찾을 수 없어!");
        }
    }
}