using UnityEngine;
using System.Collections;
using System.Linq; // Any �޼��� ����� ���� �߰�

public class Stair : MonoBehaviour
{
    private Vector3 originalPosition; // ���� ��ġ
    private Vector3 targetPosition;   // �ö� ��ġ
    public BottomButton[] buttons;   // �ۺ� ��ư �迭 (�ν����Ϳ��� ����)
    private bool isMoving = false;    // �̵� ������ Ȯ��
    private Coroutine currentCoroutine;

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0); // Y������ 20��ŭ ����
        if (buttons == null || buttons.Length == 0)
        {
            Debug.LogError("ButtonTrigger �迭�� ��� �ֽ��ϴ�. �ν����Ϳ��� ��ư�� �����ϼ���.");
        }
    }

    public void Activate()
    {
        if (!isMoving)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveUpAndStay());
        }
    }

    private IEnumerator MoveUpAndStay()
    {
        isMoving = true;

        // ���İ��� �ö�
        transform.position = targetPosition;

        // ��� ��ư �� �ϳ��� ���� �ִ� ���� ���
        while (buttons.Any(b => b != null && b.IsPressed()))
        {
            yield return null; // �� ������ Ȯ��
        }

        // ��� ��ư�� �����Ǹ� ������
        transform.position = originalPosition;

        isMoving = false;
        currentCoroutine = null;
    }
}