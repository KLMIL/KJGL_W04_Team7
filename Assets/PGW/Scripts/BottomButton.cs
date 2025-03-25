using UnityEngine;
using System;

public class BottomButton : MonoBehaviour
{
    public Action<bool> onPressedStateChanged; // ���� ���°� ����� �� ȣ��Ǵ� �׼�

    private Vector3 originalPosition; // ��ư�� ���� ��ġ
    private Vector3 pressedPosition; // ���� ��ġ (Y������ -0.1)
    private bool isPressed = false; // ��ư�� ���ȴ��� ����
    public float moveSpeed = 5f; // ��ư�� �������� �ӵ�

    void Start()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, -0.1f, 0); // Y������ -0.1 �̵�
    }

    void Update()
    {
        // ��ư �̵� ����
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
            if (!isPressed) // ���°� ����� ���� ȣ��
            {
                isPressed = true;
                onPressedStateChanged?.Invoke(true); // ���� ���¸� true�� ����
                Debug.Log("BottomButton�� ���Ƚ��ϴ�.");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isPressed) // ���°� ����� ���� ȣ��
            {
                isPressed = false;
                onPressedStateChanged?.Invoke(false); // ���� ���¸� false�� ����
                Debug.Log("BottomButton�� �����Ǿ����ϴ�.");
            }
        }
    }

    public bool IsPressed()
    {
        return isPressed;
    }
}