using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WallButtonOnceList : MonoBehaviour
{
    public List<GameObject> targetObjects; // 호출할 대상 오브젝트
    public string methodName = "Activate"; // 호출할 메소드 이름
    public float activationRange = 2f; // 버튼 활성화 범위 (거리)
    


    void Update()
    {
        // 가장 가까운 플레이어 찾기
        GameObject closestPlayer = FindClosestPlayer();

        // 가장 가까운 플레이어와의 거리 체크 및 'E' 키 입력 감지
        if (closestPlayer != null && Vector3.Distance(transform.position, closestPlayer.transform.position) <= activationRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                InvokeMethod();
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


    private void InvokeMethod()
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
            
        }
        else
        {
            Debug.LogWarning("타겟 오브젝트가 설정되지 않았습니다.");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }

}
