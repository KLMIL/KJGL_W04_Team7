using System;
using UnityEngine;

public class BottomButton : MonoBehaviour
{
    public Action<bool, GameObject> onPressedStateChanged;

    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    public bool isPressed = false;
    public float moveSpeed = 5f;
    private float exitDelay = 0.3f; // ���� ���� �ð�
    private float lastExitTime;
    private bool isWaitingForDebounce = false;

    [SerializeField] private GameObject triggerObject; // �ڽ� Ʈ���� ������Ʈ (�ν����Ϳ��� ����)
    private Rigidbody rb;

    void Awake()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, -0.1f, 0);
        rb = GetComponent<Rigidbody>();

        // �ڽ� Ʈ���� ������Ʈ Ȯ��
        if (triggerObject == null)
        {
            Debug.LogError($"{gameObject.name}: �ڽ� Ʈ���� ������Ʈ�� �������� �ʾҽ��ϴ�. �ν����Ϳ��� �����ϼ���.");
        }
    }

    void FixedUpdate()
    {
        if (rb == null)
        {
            Debug.LogError($"{gameObject.name}: Rigidbody�� null�Դϴ�!");
            return; // rb�� null�̸� ���⼭ ����
        }

        if (isPressed)
        {
            rb.MovePosition(Vector3.Lerp(transform.position, pressedPosition, moveSpeed * Time.fixedDeltaTime));
        }
        else
        {
            rb.MovePosition(Vector3.Lerp(transform.position, originalPosition, moveSpeed * Time.fixedDeltaTime));
        }

        // ��ٿ ó��
        if (isWaitingForDebounce && Time.time - lastExitTime > exitDelay)
        {
            if (!isPressed)
            {
                Debug.Log($"{gameObject.name}: ���� ���� Ȯ��");
                onPressedStateChanged?.Invoke(false, this.gameObject);
            }
            isWaitingForDebounce = false;
        }
    }

    // �ڽ� Ʈ���ſ��� ȣ���� �޼��� (�̸� ��Ȯȭ)
    public void HandleTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            isPressed = true;
            isWaitingForDebounce = false;
            onPressedStateChanged?.Invoke(true, this.gameObject);
            Debug.Log("BottomButton�� ���Ƚ��ϴ�. Button: " + gameObject.name);
        }
    }

    public void HandleTriggerExit(Collider other)
    {
        if (isPressed && !isWaitingForDebounce)
        {
            isPressed = false;
            lastExitTime = Time.time;
            isWaitingForDebounce = true;
            Debug.Log("BottomButton�� ��� ������. ��ٿ ��� ����: " + gameObject.name);
        }
    }

}