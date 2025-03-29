using System;
using UnityEngine;

public class BottomButton : MonoBehaviour
{
    public Action<bool, GameObject> onPressedStateChanged;

    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    private bool isPressed = false;
    public float moveSpeed = 5f;
    private float exitDelay = 0.1f; // 해제 지연 시간 (조정 가능)
    private float lastExitTime; // 마지막으로 Exit가 호출된 시간

    void Start()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, -0.1f, 0);

        // 버튼에 Kinematic Rigidbody 추가
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            Debug.Log($"{gameObject.name}: Kinematic Rigidbody 추가됨");
        }

    }

    void Update()
    {
        if (isPressed)
        {
            transform.position = Vector3.Lerp(transform.position, pressedPosition, Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * moveSpeed);
        }

        // Exit 후 일정 시간이 지나면 해제 상태 확정
        if (!isPressed && Time.time - lastExitTime > exitDelay)
        {
            Debug.Log($"{gameObject.name}: 해제 상태 확정");
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (!isPressed) // 상태가 변경될 때만 호출
        {
            isPressed = true;
            // 버튼 자신(this.gameObject)을 전달
            onPressedStateChanged?.Invoke(true, this.gameObject);
            Debug.Log("BottomButton이 눌렸습니다. Button: " + gameObject.name);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isPressed) // 상태가 변경될 때만 호출
        {
            isPressed = false;
            // 버튼 자신(this.gameObject)을 전달
            onPressedStateChanged?.Invoke(false, this.gameObject);
            Debug.Log("BottomButton이 해제되었습니다. Button: " + gameObject.name);
        }
    }

    public bool IsPressed()
    {
        return isPressed;
    }
}