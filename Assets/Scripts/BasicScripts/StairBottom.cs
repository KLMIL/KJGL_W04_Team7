using System.Collections;
using System.Linq;
using UnityEngine;

public class StairBottom : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    public BottomButton[] bottomButtons; // BottomButton �迭
    private Coroutine currentCoroutine;

    [SerializeField] private float moveSpeed = 30f; // �̵� �ӵ�

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0);

        // ��ư ���� ����
        foreach (var button in bottomButtons)
        {
            if (button != null)
            {
                button.onPressedStateChanged += HandleButtonState;
            }
        }

        if (bottomButtons == null || bottomButtons.Length == 0)
        {
            Debug.LogError("BottomButton �迭�� ��� �ֽ��ϴ�.");
        }
    }

    public void HandleButtonState(bool pressed, GameObject buttonObject)
    {
        if (bottomButtons.Any(b => b != null && b.gameObject == buttonObject))
        {
            // ��ư�� ������ �� ���� �ø�
            if (pressed && currentCoroutine == null)
            {
                if (currentCoroutine != null) StopCoroutine(currentCoroutine);
                currentCoroutine = StartCoroutine(MoveUp());
                Debug.Log($"{gameObject.name}: ��ư ���� - �� �ö� ����");
            }
            // ��� ��ư�� �����Ǿ��� �� ���� ����
            else if (!pressed && !bottomButtons.Any(b => b != null && b.IsPressed()))
            {
                if (currentCoroutine != null) StopCoroutine(currentCoroutine);
                currentCoroutine = StartCoroutine(MoveDown());
                Debug.Log($"{gameObject.name}: ��� ��ư ���� - �� ������ ����");
            }
        }
    }

    private IEnumerator MoveUp()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        Debug.Log($"{gameObject.name}: ���� ��ǥ ��ġ�� ����");

        // ��ư�� ���� �ִ� ���� ����
        while (bottomButtons.Any(b => b != null && b.IsPressed()))
        {
            yield return null;
        }

        // ��ư�� ��� �����Ǹ� ������
        currentCoroutine = StartCoroutine(MoveDown());
    }

    private IEnumerator MoveDown()
    {
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = originalPosition;
        Debug.Log($"{gameObject.name}: ���� ���� ��ġ�� ����");
        currentCoroutine = null;
    }
}