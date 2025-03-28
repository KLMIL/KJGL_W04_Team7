using System.Collections;
using UnityEngine;

public class StairWallOnce : MonoBehaviour
{
    private Vector3 originalPosition; // ���� ��ġ
    private Vector3 targetPosition;   // �ö� ��ġ
    private bool isMoving = false;    // �̵� ������ Ȯ��
    private Coroutine currentCoroutine;
    public float stayDuration = 3f;   // ����� �ö� ���·� �����Ǵ� �ð� (��)

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0); // Y������ 20��ŭ ����
    }

    // WallButtonOnce���� ȣ��� �޼���
    public void Activate()
    {
        if (!isMoving)
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(MoveUpAndStay());
        }
    }

    private IEnumerator MoveUpAndStay()
    {
        isMoving = true;
        transform.position = targetPosition;
        Debug.Log("Stair�� �ö󰬽��ϴ�.");

        // ������ �ð� ���� ����
        yield return new WaitForSeconds(stayDuration);

        // �ð� �� ������
        transform.position = originalPosition;
        Debug.Log("Stair�� ���������ϴ�.");
        isMoving = false;
        currentCoroutine = null;
    }
}