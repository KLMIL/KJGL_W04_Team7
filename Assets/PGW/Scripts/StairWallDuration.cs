using UnityEngine;
using System.Collections;
using System.Linq;

public class StairWallDuration : MonoBehaviour
{
    private Vector3 originalPosition; // ���� ��ġ
    private Vector3 targetPosition;   // �ö� ��ġ
    public WallButtonDuration[] wallButtons; // WallButtonDuration �迭
    private bool isMoving = false;           // �̵� ������ Ȯ��
    private Coroutine currentCoroutine;
    private int activeButtonCount = 0;       // Ȱ��ȭ�� ��ư �� ����

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0); // Y������ 20��ŭ ����

        // WallButtonDuration �̺�Ʈ ����
        foreach (var button in wallButtons)
        {
            if (button != null)
            {
                button.onPressedStateChanged += HandleButtonState;
                button.onIntervalTriggered += HandleIntervalTrigger;
            }
        }

        if (wallButtons == null || wallButtons.Length == 0)
        {
            Debug.LogError("WallButtonDuration �迭�� ��� �ֽ��ϴ�. �ν����Ϳ��� ��ư�� �����ϼ���.");
        }
    }

    void OnDestroy()
    {
        // WallButtonDuration �̺�Ʈ ���� ����
        foreach (var button in wallButtons)
        {
            if (button != null)
            {
                button.onPressedStateChanged -= HandleButtonState;
                button.onIntervalTriggered -= HandleIntervalTrigger;
            }
        }
    }

    private void HandleButtonState(bool active, GameObject gameObject)
    {
        if (active && wallButtons.Any(b => b != null && b.gameObject == gameObject))
        {
            activeButtonCount++; // Ȱ��ȭ�� ��ư �� ����
            if (!isMoving)
            {
                if (currentCoroutine != null) StopCoroutine(currentCoroutine);
                currentCoroutine = StartCoroutine(MoveUpAndStay());
            }
        }
        else
        {
            activeButtonCount--; // Ȱ��ȭ�� ��ư �� ����
            if (activeButtonCount < 0) activeButtonCount = 0; // ���� ����
        }
    }

    private void HandleIntervalTrigger()
    {
        // �ֱ��� �̺�Ʈ ó�� (�ʿ� �� �߰� ���� ����)
        Debug.Log("WallButtonDuration ���� Ʈ���� �߻� - ��� ���� ��");
    }

    private IEnumerator MoveUpAndStay()
    {
        isMoving = true;
        transform.position = targetPosition;
        Debug.Log("Stair�� �ö󰬽��ϴ�.");

        // �ϳ��� Ȱ��ȭ�� ��ư�� �ִ� ���� ����
        while (activeButtonCount > 0)
        {
            yield return null; // �� ������ Ȯ��
        }

        // ��� ��ư�� ��Ȱ��ȭ�Ǹ� ������
        transform.position = originalPosition;
        Debug.Log("Stair�� ���������ϴ�.");
        isMoving = false;
        currentCoroutine = null;
    }
}
