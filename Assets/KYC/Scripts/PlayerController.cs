using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public static PlayerController activePlayer;
    public bool isPlayer1;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerController otherPlayer;

    private bool isInitialized = false; // 초기화 여부 플래그

    void Awake()
    {
        // Awake에서 초기화
        if (isPlayer1 && activePlayer == null)
        {
            activePlayer = this;
            Debug.Log("Player 1이 activePlayer로 설정됨 (Awake)");
        }
    }

    void Start()
    {
        if (!isPlayer1 && activePlayer == null)
        {
            activePlayer = this; // Player 1이 없으면 Player 2가 기본
            Debug.Log("Player 2가 activePlayer로 설정됨 (Start)");
        }
        isInitialized = true;
        // UpdateCameraTarget()은 Start에서 바로 호출하지 않음
    }

    void Update()
    {
        // 초기화가 완료된 후 첫 Update에서 카메라 설정
        if (!isInitialized) return;
        if (this == activePlayer)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;
            transform.Translate(movement * speed * Time.deltaTime, Space.World);

            if (Input.GetButtonDown("Interact"))
            {
                Interact();
            }

            // 카메라 위치를 매 프레임 업데이트 (부드러운 이동)
            if (playerCamera != null)
            {
                Vector3 targetPos = new Vector3(
                    transform.position.x,
                    playerCamera.transform.position.y,
                    transform.position.z - 10f
                );
                playerCamera.transform.position = Vector3.Lerp(
                    playerCamera.transform.position,
                    targetPos,
                    Time.deltaTime * 5f
                );
            }
        }
    }

    void Interact()
    {
        Debug.Log((isPlayer1 ? "Player 1" : "Player 2") + "이 E로 상호작용했습니다!");
    }


}