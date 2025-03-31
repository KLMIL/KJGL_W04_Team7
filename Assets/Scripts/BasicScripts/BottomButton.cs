using System;
using UnityEngine;

public class BottomButton : MonoBehaviour
{
    public Action<bool, GameObject> onPressedStateChanged;

    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    public bool isPressed = false;
    public float moveSpeed = 5f;
    private float exitDelay = 0.3f; // 해제 지연 시간
    private float lastExitTime;
    private bool isWaitingForDebounce = false;

    [SerializeField] private GameObject triggerObject; // 자식 트리거 오브젝트 (인스펙터에서 설정)
    private Rigidbody rb;

    void Awake()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, -0.1f, 0);
        rb = GetComponent<Rigidbody>();

        // 자식 트리거 오브젝트 확인
        if (triggerObject == null)
        {
            Debug.LogError($"{gameObject.name}: 자식 트리거 오브젝트가 설정되지 않았습니다. 인스펙터에서 연결하세요.");
        }
    }

    void FixedUpdate()
    {
        if (rb == null)
        {
            Debug.LogError($"{gameObject.name}: Rigidbody가 null입니다!");
            return; // rb가 null이면 여기서 멈춤
        }

        if (isPressed)
        {
            rb.MovePosition(Vector3.Lerp(transform.position, pressedPosition, moveSpeed * Time.fixedDeltaTime));
        }
        else
        {
            rb.MovePosition(Vector3.Lerp(transform.position, originalPosition, moveSpeed * Time.fixedDeltaTime));
        }

        // 디바운스 처리
        if (isWaitingForDebounce && Time.time - lastExitTime > exitDelay)
        {
            if (!isPressed)
            {
                Debug.Log($"{gameObject.name}: 해제 상태 확정");
                onPressedStateChanged?.Invoke(false, this.gameObject);
            }
            isWaitingForDebounce = false;
        }
    }

    // 자식 트리거에서 호출할 메서드 (이름 명확화)
    public void HandleTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            isPressed = true;
            isWaitingForDebounce = false;
            onPressedStateChanged?.Invoke(true, this.gameObject);
            Debug.Log("BottomButton이 눌렸습니다. Button: " + gameObject.name);
        }
    }

    public void HandleTriggerExit(Collider other)
    {
        if (isPressed && !isWaitingForDebounce)
        {
            isPressed = false;
            lastExitTime = Time.time;
            isWaitingForDebounce = true;
            Debug.Log("BottomButton이 잠시 해제됨. 디바운스 대기 시작: " + gameObject.name);
        }
    }

}