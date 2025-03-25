using UnityEngine;

public class BottomButton : MonoBehaviour
{
    public GameObject targetObject; // ȣ���� ��� ������Ʈ
    public string methodName = "Activate"; // ȣ���� �޼ҵ� �̸�
    public float invokeInterval = 0.5f; // �޼ҵ� ȣ�� ���� (�� ����)

    private Vector3 originalPosition; // ��ư�� ���� ��ġ
    private Vector3 pressedPosition; // ���� ��ġ (Y������ -0.1)
    private bool isPressed = false; // ��ư�� ���ȴ��� ����
    private float lastInvokeTime; // ������ ȣ�� �ð�
    public float moveSpeed = 5f; // ��ư�� �������� �ӵ�

    void Start()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, -0.1f, 0); // Y������ -0.1 �̵�
        lastInvokeTime = -invokeInterval; // �ʱ� ȣ�� ���� ���·� ����
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

        // ��ư�� ���� ���¿��� �ֱ������� �޼ҵ� ȣ��
        if (isPressed && Time.time - lastInvokeTime >= invokeInterval)
        {
            InvokeMethod();
            lastInvokeTime = Time.time; // ������ ȣ�� �ð� ����
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPressed = true; // �÷��̾ ���� �ִ� ���� ���� ���� ����
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPressed = false; // �÷��̾ ������ ���� ����
        }
    }

    private void InvokeMethod()
    {
        if (targetObject != null)
        {
            targetObject.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
            Debug.Log($"{methodName} �޼ҵ尡 ȣ��Ǿ����ϴ�!");
        }
        else
        {
            Debug.LogWarning("Ÿ�� ������Ʈ�� �������� �ʾҽ��ϴ�.");
        }
    }

    public bool IsPressed()
    {
        return isPressed;
    }
}