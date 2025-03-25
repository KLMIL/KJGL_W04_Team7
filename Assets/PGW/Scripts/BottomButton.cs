using UnityEngine;

public class BottomButton : MonoBehaviour
{
    public GameObject targetObject; // 호출할 대상 오브젝트
    public string methodName = "Activate"; // 호출할 메소드 이름
    public float invokeInterval = 0.5f; // 메소드 호출 간격 (초 단위)

    private Vector3 originalPosition; // 버튼의 원래 위치
    private Vector3 pressedPosition; // 눌린 위치 (Y축으로 -0.1)
    private bool isPressed = false; // 버튼이 눌렸는지 상태
    private float lastInvokeTime; // 마지막 호출 시간
    public float moveSpeed = 5f; // 버튼이 내려가는 속도

    void Start()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, -0.1f, 0); // Y축으로 -0.1 이동
        lastInvokeTime = -invokeInterval; // 초기 호출 가능 상태로 설정
    }

    void Update()
    {
        // 버튼 이동 로직
        if (isPressed)
        {
            transform.position = Vector3.Lerp(transform.position, pressedPosition, Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * moveSpeed);
        }

        // 버튼이 눌린 상태에서 주기적으로 메소드 호출
        if (isPressed && Time.time - lastInvokeTime >= invokeInterval)
        {
            InvokeMethod();
            lastInvokeTime = Time.time; // 마지막 호출 시간 갱신
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPressed = true; // 플레이어가 위에 있는 동안 눌림 상태 유지
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPressed = false; // 플레이어가 떠나면 눌림 해제
        }
    }

    private void InvokeMethod()
    {
        if (targetObject != null)
        {
            targetObject.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
            Debug.Log($"{methodName} 메소드가 호출되었습니다!");
        }
        else
        {
            Debug.LogWarning("타겟 오브젝트가 설정되지 않았습니다.");
        }
    }

    public bool IsPressed()
    {
        return isPressed;
    }
}