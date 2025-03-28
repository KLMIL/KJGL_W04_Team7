using UnityEngine;

public class WallMoveDown : MonoBehaviour
{
    // ���� ����
    public float moveDistance = 2f;    // �̵��� �Ÿ�
    public float moveSpeed = 2f;       // �̵� �ӵ�

    // ���� ����
    private Vector3 originalPosition;  // ���� ��ġ
    private Vector3 targetPosition;    // ��ǥ ��ġ
    private bool isMoving = false;     // �̵� ������ üũ

    void Start()
    {
        // ���� �� ���� ��ġ ����
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            // �ε巴�� ��ǥ ��ġ�� �̵�
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            // ��ǥ ��ġ�� ���� �����ߴ��� üũ
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // ��Ȯ�� ��ǥ ��ġ�� ����
                isMoving = false;                    // �̵� �Ϸ�
            }
        }
    }

    public void Activate()
    {
        if (!isMoving)
        {
            isMoving = true;
            // ��ǥ ��ġ�� Y������ moveDistance��ŭ �̵��� ��ġ�� ����
            targetPosition = originalPosition + Vector3.up * moveDistance;
        }
    }
}
