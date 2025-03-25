using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    public float speed = 5f; // 이동 속도

    void Update()
    {
        // 이동 입력
        float moveX = Input.GetAxis("Horizontal1"); // A/D 키
        float moveZ = Input.GetAxis("Vertical1");   // W/S 키
        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // 상호작용 키 (Spacebar)
        if (Input.GetButtonDown("Interact1")) // Spacebar가 눌렸을 때
        {
            Interact(); // 상호작용 함수 호출
        }
    }

    void Interact()
    {
        // 상호작용 로직 예시
        Debug.Log("플레이어 1이 Spacebar로 상호작용했습니다!");
        // 여기에 상호작용 로직 추가 (예: 문 열기)
    }
}