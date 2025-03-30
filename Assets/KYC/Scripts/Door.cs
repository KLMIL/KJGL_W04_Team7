using UnityEngine;

public class Door : MonoBehaviour
{
    public bool forPlayer1; // Player1용 문인지 여부
    private Vector3 spawnPointExtra;
    public int index;

    void Start()
    {
        spawnPointExtra = new Vector3(0f, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (forPlayer1 && index >= GameManager.Instance.StageData)
            {
                // Player1이 통과하면 임시 스폰 포인트 설정
                GameManager.Instance.SetTempPlayer1SpawnPoint(transform.position + spawnPointExtra);
                GameManager.Instance.SetPlayer1Passed(true);
                Debug.Log("player1 passed and set temp spawn point: " + (transform.position + spawnPointExtra));
            }
            else if (index >= GameManager.Instance.StageData)
            {
                // Player2가 통과하면 임시 스폰 포인트 설정
                GameManager.Instance.SetTempPlayer2SpawnPoint(transform.position + spawnPointExtra);
                GameManager.Instance.SetPlayer2Passed(true);
                Debug.Log("player2 passed and set temp spawn point: " + (transform.position + spawnPointExtra));
            }

            // 두 플레이어가 모두 통과했는지 확인
            if (GameManager.Instance.AreBothPlayersPassed())
            {
                Debug.Log("Both players passed");
                GameManager.Instance.CommitSpawnPoints(); // 임시 스폰 포인트를 영구 적용
                GameManager.Instance.PlusStageData();
                GameManager.Instance.ShowStageData();
                GameManager.Instance.ResetPassedFlags(); // 플래그 초기화

                if (index == 6)
                {
                    UIManager.Instance.escapeSuccess.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
        }
    }

    void Update()
    {
        // Update에서 추가 처리 불필요
    }
}