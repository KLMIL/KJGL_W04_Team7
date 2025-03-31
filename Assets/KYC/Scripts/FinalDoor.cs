using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    private StageLast stageLast; // StageLast 참조

    void Start()
    {
        // StageLast 컴포넌트 가져오기
        stageLast = FindFirstObjectByType<StageLast>();
        if (stageLast == null)
        {
            Debug.LogError("StageLast를 찾을 수 없어!");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (stageLast == null || stageLast.gameComplete == false) return; // 게임 완료 전에는 동작 안 함

        // 플레이어 태그로 충돌 감지
        if (other.CompareTag("Player"))
        {
            // 플레이어 확인 및 상태 업데이트
            if (other.gameObject == stageLast.player1)
            {
                stageLast.GetComponent<StageLast>().hasPlayer1Reached = true;
                Debug.Log("player1이 EndDoor에 머물고 있어!");
            }
            else if (other.gameObject == stageLast.player2)
            {
                stageLast.GetComponent<StageLast>().hasPlayer2Reached = true;
                Debug.Log("player2가 EndDoor에 머물고 있어!");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (stageLast == null || stageLast.gameComplete == false) return;

        if (other.CompareTag("Player"))
        {
            if (other.gameObject == stageLast.player1)
            {
                stageLast.hasPlayer1Reached = false;
                Debug.Log("player1이 EndDoor에서 벗어났어!");
            }
            else if (other.gameObject == stageLast.player2)
            {
                stageLast.hasPlayer2Reached = false;
                Debug.Log("player2가 EndDoor에서 벗어났어!");
            }
        }
    }
}