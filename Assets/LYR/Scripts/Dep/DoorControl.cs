using UnityEngine;

public class DoorControl : MonoBehaviour
{
    // ���� ����
    public float targetAngle = 90f;      // ������ ��ǥ ����
    public float openSpeed = 2f;         // ������ �ӵ�
    public float closeSpeed = 1f;        // ������ �ӵ�
    public bool autoClose = false;       // �ڵ� ���� ����
    public float openDuration = 0f;      // ���� ���� ���� �ð� (autoClose�� true�� ���� ����)

    // ���� ����
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private bool isOpening = false;
    private bool isClosing = false;
    private float timer = 0f;

    void Start()
    {
        // �ʱ� ȸ���� ����
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isOpening)
        {
            // �ε巴�� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);

            // ��ǥ ������ �����ߴ��� üũ
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isOpening = false;

                if (autoClose)
                {
                    timer = 0f;
                    isClosing = true;
                }
            }
        }
        else if (isClosing)
        {
            timer += Time.deltaTime;

            if (timer >= openDuration)
            {
                // �ε巴�� ���� (������ ���� �ӵ� ���)
                transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * closeSpeed);

                // ���� ������ �����ߴ��� üũ
                if (Quaternion.Angle(transform.rotation, originalRotation) < 0.1f)
                {
                    transform.rotation = originalRotation;
                    isClosing = false;
                }
            }
        }
        //Activate();
    }

    public void Activate()
    {
        if (!isOpening && !isClosing)
        {
            isOpening = true;
            targetRotation = originalRotation * Quaternion.Euler(0, targetAngle, 0);
        }
    }

    public void CloseDoor()
    {
        isOpening = false;
        isClosing = true;
    }
}
