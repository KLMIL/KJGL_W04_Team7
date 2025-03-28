using System.Collections;
using UnityEngine;

public class TrapBottom : MonoBehaviour
{
    private bool isPlayerOn = false; // �÷��̾ �ٴ��� ��� �ִ��� ����
    public float moveSpeed = 5f; // �ö󰡰� �������� �ӵ�
    private float targetY = 67f; // ��ǥ Y ��
    private bool isRising = false; // ��� ������ ����
    private Coroutine riseCoroutine; // ��� �ڷ�ƾ ����
    private Vector3 originalPosition; // ���� ��ġ ����
    private bool hasReturned = false; // ���� �Ϸ� ����

    void Start()
    {
        // ���� ��ġ ����
        originalPosition = transform.position;
    }

    void FixedUpdate()
    {
        // ��� ���̰� ���� �������� �ʾҴٸ� Y ���� ����
        if (isRising && !hasReturned && transform.position.y < targetY)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, targetY, transform.position.z),
                moveSpeed * Time.deltaTime
            );
        }
    }

    // �÷��̾ �ٴڿ� ����� ��
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlayerOn && !hasReturned)
        {
            isPlayerOn = true;
            Debug.Log("�÷��̾ �ٴ��� ��ҽ��ϴ�. 1�� �� ��� ����.");
            if (riseCoroutine != null) StopCoroutine(riseCoroutine); // ���� �ڷ�ƾ ����
            riseCoroutine = StartCoroutine(StartRisingAfterDelay());
        }
    }

    // �÷��̾ �ٴڿ��� ������ ��
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOn = false;
            isRising = false;
            if (riseCoroutine != null) StopCoroutine(riseCoroutine); // �ڷ�ƾ ����
            Debug.Log("�÷��̾ �ٴڿ��� �������ϴ�. ��� ����.");
        }
    }

    // 1�� ��� �� ��� ����
    private IEnumerator StartRisingAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // 0.5�� ���
        if (isPlayerOn && !hasReturned) // �÷��̾ ������ ��� �ְ� �������� �ʾҴٸ�
        {
            isRising = true;
            Debug.Log("0.5�� �� ��� ����!");
        }
    }

    // ���� ��ġ�� ���ư��� �޼���
    public void Activate()
    {
        if (riseCoroutine != null) StopCoroutine(riseCoroutine); // ��� �ڷ�ƾ ����
        isRising = false; // ��� ����
        StartCoroutine(ReturnToOriginalPosition());
        Debug.Log("Activate ȣ��: ���� ��ġ�� ���� ����.");
    }

    // �ε巴�� ���� ��ġ�� ����
    private IEnumerator ReturnToOriginalPosition()
    {
        while (transform.position.y > originalPosition.y)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                originalPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        transform.position = originalPosition; // ��Ȯ�� ���� ��ġ�� ����
        hasReturned = true; // ���� �Ϸ� �÷��� ����
        Debug.Log("���� ��ġ�� ���� �Ϸ�. �� �̻� �ö��� �ʽ��ϴ�.");
    }
}