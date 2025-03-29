using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square_Button : MonoBehaviour
{
    public List<GameObject> targetObjects; // 호출할 대상 오브젝트
    public string methodName = "Activate"; // 호출할 메소드 이름
    public float width = 2f;  // X축 방향 너비 (큐브의 가로 크기)
    public float height = 2f; // Y축 방향 높이 (큐브의 세로 크기)
    public float depth = 2f;  // Z축 방향 깊이 (큐브의 깊이)
    public bool IsButtonPressed { get; private set; } = false;

    void Update()
    {
        // 가장 가까운 플레이어 찾기
        GameObject closestPlayer = FindClosestPlayer();

        // 가장 가까운 플레이어와의 거리 체크 및 'E' 키 입력 감지
        if (closestPlayer != null)
        {
            if (IsPlayerInCubeRange(closestPlayer.transform.position))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ButtonListen();
                }
            }
        }
    }

    // 가장 가까운 플레이어 찾기
    private GameObject FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // 모든 "Player" 태그 오브젝트 찾기 

        if (players.Length == 0)
        {
            Debug.LogWarning("태그가 'Player'인 오브젝트가 없습니다.");
            return null;
        }

        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = player;
            }
        }

        return closest;
    }

    // 플레이어가 큐브 범위 안에 있는지 확인
    private bool IsPlayerInCubeRange(Vector3 playerPosition)
    {
        Vector3 buttonPosition = transform.position;

        // 각 축에서의 거리 차이 계산
        float xDiff = Mathf.Abs(playerPosition.x - buttonPosition.x);
        float yDiff = Mathf.Abs(playerPosition.y - buttonPosition.y);
        float zDiff = Mathf.Abs(playerPosition.z - buttonPosition.z);

        // 큐브 범위 내에 있는지 확인 (절반 크기와 비교)
        return xDiff <= width / 2f && yDiff <= height / 2f && zDiff <= depth / 2f;
    }

    private void ButtonListen()
    {
        if (targetObjects != null)
        {
            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                {
                    obj.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
                    Debug.Log($"{methodName} 메소드가 호출되었습니다!");
                }
            }
            IsButtonPressed = true;

            StartCoroutine(ResetButtonAfterDelay(1f)); // 코루틴 시작
        }
        else
        {
            Debug.LogWarning("타겟 오브젝트가 설정되지 않았습니다.");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        // 큐브 형태로 기즈모 그리기
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, depth));
    }

    private IEnumerator ResetButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간(1초) 대기
        IsButtonPressed = false; // 버튼 상태 리셋
        Debug.Log("버튼 상태가 리셋되었습니다.");
    }
}
