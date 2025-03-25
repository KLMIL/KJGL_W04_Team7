using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public float speed = 2f; // 기본 오른쪽 이동 속도
    public float pushBackDistance = 0.5f; // Spacebar 누를 때마다 왼쪽으로 밀리는 거리
    private float lastInteractTime; // 마지막 상호작용 시간
    private const float interactCooldown = 0.1f; // 상호작용 간격 (너무 빠르게 안 되게)

    void Update()
    {
        // 기본적으로 오른쪽으로 이동
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);

        // Player 1의 상호작용 확인
        if (Input.GetButtonDown("Interact1") && Time.time - lastInteractTime > interactCooldown)
        {
            PushBack(); // 왼쪽으로 밀기
            lastInteractTime = Time.time; // 시간 기록
        }
        if (Input.GetButtonDown("Interact2") && Time.time - lastInteractTime > interactCooldown)
        {
            PushBack(); // 왼쪽으로 밀기
            lastInteractTime = Time.time; // 시간 기록
        }
    }

    void PushBack()
    {
        // 벽을 왼쪽으로 밀어냄
        transform.Translate(Vector3.left * pushBackDistance, Space.World);
        Debug.Log("Player 1이 벽을 왼쪽으로 밀었습니다!");
    }
}