using UnityEngine;
using System;

public class WallButtonDuration : MonoBehaviour
{
    public Action<bool, GameObject> onPressedStateChanged; // 활성화 상태 변화 알림 (true: 활성화, false: 비활성화)
    public Action onIntervalTriggered;        // 주기적으로 호출되는 이벤트

    private Vector3 originalPosition; // 버튼의 원래 위치
    private Vector3 pressedPosition; // 눌린 위치 (Z축으로 이동)
    private bool isPressed = false;  // 버튼이 눌렸는지 상태 (시각적 표시용)
    private bool isActive = false;   // 버튼이 활성화된 상태인지
    private float lastInvokeTime;    // 마지막 호출 시간
    private float activeTimer;       // 활성화 타이머
    public float moveSpeed = 5f;     // 버튼 이동 속도
    public float activeDuration = 3f; // 버튼 활성화 유지 시간
    public float invokeInterval = 0.5f; // 주기적 호출 간격
    public float activationRange = 2f;  // 버튼 활성화 범위

    void Start()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, 0, -0.1f); // Z축으로 이동
        lastInvokeTime = -invokeInterval; // 초기 호출 가능 상태로 설정
    }

    void Update()
    {
        // 가장 가까운 플레이어 찾기
        GameObject closestPlayer = FindClosestPlayer();

        // 가장 가까운 플레이어와의 거리 체크 및 'E' 키 입력 감지
        if (!isActive && closestPlayer != null && Vector3.Distance(transform.position, closestPlayer.transform.position) <= activationRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isPressed = true; // 버튼 눌림 표시
                ActivateButton(); // 버튼 활성화
            }
        }

        // 버튼 이동 로직
        if (isPressed || isActive)
        {
            transform.position = Vector3.Lerp(transform.position, pressedPosition, Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * moveSpeed);
        }

        // 활성화된 상태에서 주기적으로 이벤트 호출
        if (isActive && Time.time - lastInvokeTime >= invokeInterval)
        {
            onIntervalTriggered?.Invoke(); // 주기적 이벤트 발생
            lastInvokeTime = Time.time;    // 마지막 호출 시간 갱신
            Debug.Log("Interval 이벤트 호출됨");
        }

        // 활성화 타이머 관리
        if (isActive)
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0)
            {
                DeactivateButton(); // 시간이 다 되면 비활성화
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

    private void ActivateButton()
    {
        isActive = true;
        activeTimer = activeDuration; // 타이머 설정
        onPressedStateChanged?.Invoke(true, this.gameObject); // 활성화 상태 알림
        Debug.Log("WallButton 활성화됨");
    }

    private void DeactivateButton()
    {
        isActive = false;
        isPressed = false; // 눌림 상태 해제
        onPressedStateChanged?.Invoke(false, this.gameObject); // 비활성화 상태 알림
        Debug.Log("WallButton 비활성화됨");
    }

    public bool IsPressed()
    {
        return isPressed;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }
}