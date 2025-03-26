using UnityEngine;
using System;

public class WallButtonDuration : MonoBehaviour
{
    public Action<bool, GameObject> onPressedStateChanged; // Ȱ��ȭ ���� ��ȭ �˸� (true: Ȱ��ȭ, false: ��Ȱ��ȭ)
    public Action onIntervalTriggered;        // �ֱ������� ȣ��Ǵ� �̺�Ʈ

    private Vector3 originalPosition; // ��ư�� ���� ��ġ
    private Vector3 pressedPosition; // ���� ��ġ (Z������ �̵�)
    private bool isPressed = false;  // ��ư�� ���ȴ��� ���� (�ð��� ǥ�ÿ�)
    private bool isActive = false;   // ��ư�� Ȱ��ȭ�� ��������
    private float lastInvokeTime;    // ������ ȣ�� �ð�
    private float activeTimer;       // Ȱ��ȭ Ÿ�̸�
    public float moveSpeed = 5f;     // ��ư �̵� �ӵ�
    public float activeDuration = 3f; // ��ư Ȱ��ȭ ���� �ð�
    public float invokeInterval = 0.5f; // �ֱ��� ȣ�� ����
    public float activationRange = 2f;  // ��ư Ȱ��ȭ ����

    void Start()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, 0, -0.1f); // Z������ �̵�
        lastInvokeTime = -invokeInterval; // �ʱ� ȣ�� ���� ���·� ����
    }

    void Update()
    {
        // ���� ����� �÷��̾� ã��
        GameObject closestPlayer = FindClosestPlayer();

        // ���� ����� �÷��̾���� �Ÿ� üũ �� 'E' Ű �Է� ����
        if (!isActive && closestPlayer != null && Vector3.Distance(transform.position, closestPlayer.transform.position) <= activationRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isPressed = true; // ��ư ���� ǥ��
                ActivateButton(); // ��ư Ȱ��ȭ
            }
        }

        // ��ư �̵� ����
        if (isPressed || isActive)
        {
            transform.position = Vector3.Lerp(transform.position, pressedPosition, Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * moveSpeed);
        }

        // Ȱ��ȭ�� ���¿��� �ֱ������� �̺�Ʈ ȣ��
        if (isActive && Time.time - lastInvokeTime >= invokeInterval)
        {
            onIntervalTriggered?.Invoke(); // �ֱ��� �̺�Ʈ �߻�
            lastInvokeTime = Time.time;    // ������ ȣ�� �ð� ����
            Debug.Log("Interval �̺�Ʈ ȣ���");
        }

        // Ȱ��ȭ Ÿ�̸� ����
        if (isActive)
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0)
            {
                DeactivateButton(); // �ð��� �� �Ǹ� ��Ȱ��ȭ
            }
        }
    }

    // ���� ����� �÷��̾� ã��
    private GameObject FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // ��� "Player" �±� ������Ʈ ã��
        if (players.Length == 0)
        {
            Debug.LogWarning("�±װ� 'Player'�� ������Ʈ�� �����ϴ�.");
            return null;
        }

        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = player;
            }
        }

        return closest;
    }

    private void ActivateButton()
    {
        isActive = true;
        activeTimer = activeDuration; // Ÿ�̸� ����
        onPressedStateChanged?.Invoke(true, this.gameObject); // Ȱ��ȭ ���� �˸�
        Debug.Log("WallButton Ȱ��ȭ��");
    }

    private void DeactivateButton()
    {
        isActive = false;
        isPressed = false; // ���� ���� ����
        onPressedStateChanged?.Invoke(false, this.gameObject); // ��Ȱ��ȭ ���� �˸�
        Debug.Log("WallButton ��Ȱ��ȭ��");
    }

    public bool IsPressed()
    {
        return isPressed;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }
}