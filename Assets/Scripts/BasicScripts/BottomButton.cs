using System;
using UnityEngine;

public class BottomButton : MonoBehaviour
{
    // Action�� GameObject Ÿ������ ��ư �ڽ��� ����
    public Action<bool, GameObject> onPressedStateChanged; // ���� ���¿� ��ư ������Ʈ�� ����� �� ȣ��Ǵ� �׼�

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
                // ��ư �ڽ�(this.gameObject)�� ����
                onPressedStateChanged?.Invoke(true, this.gameObject);
                Debug.Log("BottomButton�� ���Ƚ��ϴ�. Button: " + gameObject.name);
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
                // ��ư �ڽ�(this.gameObject)�� ����
                onPressedStateChanged?.Invoke(false, this.gameObject);
                Debug.Log("BottomButton�� �����Ǿ����ϴ�. Button: " + gameObject.name);
            }
        }
    }

    public bool IsPressed()
    {
        return isPressed;
    }
}