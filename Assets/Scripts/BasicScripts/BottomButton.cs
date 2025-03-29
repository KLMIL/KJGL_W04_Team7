using System;
using UnityEngine;

public class BottomButton : MonoBehaviour
{
    public Action<bool, GameObject> onPressedStateChanged;

    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    private bool isPressed = false;
    public float moveSpeed = 5f;
    private float exitDelay = 0.1f; // ���� ���� �ð� (���� ����)
    private float lastExitTime; // ���������� Exit�� ȣ��� �ð�

    void Start()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, -0.1f, 0);

        // ��ư�� Kinematic Rigidbody �߰�
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            Debug.Log($"{gameObject.name}: Kinematic Rigidbody �߰���");
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

        // Exit �� ���� �ð��� ������ ���� ���� Ȯ��
        if (!isPressed && Time.time - lastExitTime > exitDelay)
        {
            Debug.Log($"{gameObject.name}: ���� ���� Ȯ��");
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (!isPressed) // ���°� ����� ���� ȣ��
        {
            isPressed = true;
            // ��ư �ڽ�(this.gameObject)�� ����
            onPressedStateChanged?.Invoke(true, this.gameObject);
            Debug.Log("BottomButton�� ���Ƚ��ϴ�. Button: " + gameObject.name);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isPressed) // ���°� ����� ���� ȣ��
        {
            isPressed = false;
            // ��ư �ڽ�(this.gameObject)�� ����
            onPressedStateChanged?.Invoke(false, this.gameObject);
            Debug.Log("BottomButton�� �����Ǿ����ϴ�. Button: " + gameObject.name);
        }
    }

    public bool IsPressed()
    {
        return isPressed;
    }
}