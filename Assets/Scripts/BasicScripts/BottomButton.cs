using System;
using UnityEngine;

public class BottomButton : MonoBehaviour
{
    // Action에 GameObject 타입으로 버튼 자신을 전달
    public Action<bool, GameObject> onPressedStateChanged; // 눌림 상태와 버튼 오브젝트가 변경될 때 호출되는 액션

    private Vector3 originalPosition; // 버튼의 원래 위치
    private Vector3 pressedPosition; // 눌린 위치 (Y축으로 -0.1)
    private bool isPressed = false; // 버튼이 눌렸는지 상태
    public float moveSpeed = 5f; // 버튼이 내려가는 속도

    void Start()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, -0.1f, 0); // Y축으로 -0.1 이동
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
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isPressed) // 상태가 변경될 때만 호출
            {
                isPressed = true;
                // 버튼 자신(this.gameObject)을 전달
                onPressedStateChanged?.Invoke(true, this.gameObject);
                Debug.Log("BottomButton이 눌렸습니다. Button: " + gameObject.name);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isPressed) // 상태가 변경될 때만 호출
            {
                isPressed = false;
                // 버튼 자신(this.gameObject)을 전달
                onPressedStateChanged?.Invoke(false, this.gameObject);
                Debug.Log("BottomButton이 해제되었습니다. Button: " + gameObject.name);
            }
        }
    }

    public bool IsPressed()
    {
        return isPressed;
    }
}