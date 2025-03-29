using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square_Button : MonoBehaviour
{
    public List<GameObject> targetObjects; // ȣ���� ��� ������Ʈ
    public string methodName = "Activate"; // ȣ���� �޼ҵ� �̸�
    public float width = 2f;  // X�� ���� �ʺ� (ť���� ���� ũ��)
    public float height = 2f; // Y�� ���� ���� (ť���� ���� ũ��)
    public float depth = 2f;  // Z�� ���� ���� (ť���� ����)
    public bool IsButtonPressed { get; private set; } = false;

    void Update()
    {
        // ���� ����� �÷��̾� ã��
        GameObject closestPlayer = FindClosestPlayer();

        // ���� ����� �÷��̾���� �Ÿ� üũ �� 'E' Ű �Է� ����
        if (closestPlayer != null)
        {
            if (IsPlayerInCubeRange(closestPlayer.transform.position))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ButtonListen();
                }
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

    // �÷��̾ ť�� ���� �ȿ� �ִ��� Ȯ��
    private bool IsPlayerInCubeRange(Vector3 playerPosition)
    {
        Vector3 buttonPosition = transform.position;

        // �� �࿡���� �Ÿ� ���� ���
        float xDiff = Mathf.Abs(playerPosition.x - buttonPosition.x);
        float yDiff = Mathf.Abs(playerPosition.y - buttonPosition.y);
        float zDiff = Mathf.Abs(playerPosition.z - buttonPosition.z);

        // ť�� ���� ���� �ִ��� Ȯ�� (���� ũ��� ��)
        return xDiff <= width / 2f && yDiff <= height / 2f && zDiff <= depth / 2f;
    }

    private void ButtonListen()
    {
        if (targetObjects != null)
        {
            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                {
                    obj.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
                    Debug.Log($"{methodName} �޼ҵ尡 ȣ��Ǿ����ϴ�!");
                }
            }
            IsButtonPressed = true;

            StartCoroutine(ResetButtonAfterDelay(1f)); // �ڷ�ƾ ����
        }
        else
        {
            Debug.LogWarning("Ÿ�� ������Ʈ�� �������� �ʾҽ��ϴ�.");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        // ť�� ���·� ����� �׸���
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, depth));
    }

    private IEnumerator ResetButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð�(1��) ���
        IsButtonPressed = false; // ��ư ���� ����
        Debug.Log("��ư ���°� ���µǾ����ϴ�.");
    }
}
