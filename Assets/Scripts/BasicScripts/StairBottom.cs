using System.Collections;
using UnityEngine;

public class StairBottom : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    [SerializeField] protected BottomButton bottomButton; // ���� BottomButton
    protected Coroutine currentCoroutine;
    private Rigidbody rb;

    [SerializeField] protected float moveSpeed = 30f; // �̵� �ӵ�

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 20f, 0);

        // Rigidbody ����
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; // Kinematic���� ����
            rb.useGravity = false; // �߷� ��Ȱ��ȭ
            Debug.Log($"{gameObject.name}: Kinematic Rigidbody �߰���");
        }

        // ��ư ���� ����
        if (bottomButton != null)
        {
            bottomButton.onPressedStateChanged += HandleButtonState;
        }
        else
        {
            Debug.LogError($"{gameObject.name}: BottomButton�� �������� �ʾҽ��ϴ�.");
        }
    }

    public virtual void HandleButtonState(bool pressed, GameObject buttonObject)
    {
        if (buttonObject != bottomButton.gameObject)
        {
            return; // �ٸ� ��ư�� �Է��� ����
        }

        // ��ư�� ������ �� ���� �ø�
        if (pressed)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveUp());
            Debug.Log($"{gameObject.name}: ��ư ���� - �� �ö� ����");
        }
        // ��ư�� �����Ǿ��� �� ���� ����
        else
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveDown());
            Debug.Log($"{gameObject.name}: ��ư ���� - �� ������ ����");
        }
    }

    private IEnumerator MoveUp()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            yield return new WaitForFixedUpdate(); // FixedUpdate�� ����ȭ
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
        }
        rb.MovePosition(targetPosition);
        Debug.Log($"{gameObject.name}: ���� ��ǥ ��ġ�� ����");

        yield return null; // �ʿ� �� �߰� ��� ���� ���� ����
    }

    private IEnumerator MoveDown()
    {
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            yield return new WaitForFixedUpdate(); // FixedUpdate�� ����ȭ
            rb.MovePosition(Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.fixedDeltaTime));
        }
        rb.MovePosition(originalPosition);
        Debug.Log($"{gameObject.name}: ���� ���� ��ġ�� ����");
        currentCoroutine = null;
    }
}