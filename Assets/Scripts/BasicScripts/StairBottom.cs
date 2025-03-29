using System.Collections;
using System.Linq;
using UnityEngine;

public class StairBottom : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    public BottomButton[] bottomButtons; // BottomButton �迭
    private bool isMoving = false;
    private Coroutine currentCoroutine;

    [SerializeField] private float moveSpeed = 30f; // �ö���� �ӵ� (���� ����)
    [SerializeField] private float stayDuration = 0.1f; // �ö� �� ���� �ð� (���� ����)

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0);

        // ��ư ���� ����
        foreach (var button in bottomButtons)
        {
            if (button != null)
            {
                button.onPressedStateChanged += HandleButtonState; // ���� ���� �� ȣ��
            }
        }

        if (bottomButtons == null || bottomButtons.Length == 0)
        {
            Debug.LogError("BottomButton �迭�� ��� �ֽ��ϴ�.");
        }
    }

    private void HandleButtonState(bool pressed, GameObject buttonObject)
    {
        // buttonObject�� bottomButtons�� ���ԵǾ� �ִ��� Ȯ��
        if (pressed && !isMoving && bottomButtons.Any(b => b != null && b.gameObject == buttonObject))
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(MoveUpAndStay());
        }
    }

    private IEnumerator MoveUpAndStay()
    {
        isMoving = true;

        // �ε巴�� �ö�
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition; // ��Ȯ�� ��ǥ ��ġ�� ����

        // �ϳ��� ���� �ִ� ���� ����
        while (bottomButtons.Any(b => b != null && b.IsPressed()))
        {
            yield return null;
        }

        // �ε巴�� ������
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = originalPosition; // ��Ȯ�� ���� ��ġ�� ����

        isMoving = false;
        currentCoroutine = null;
    }
}