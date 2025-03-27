using UnityEngine;
using System;
using System.Collections;

public class WallButtonOnce : MonoBehaviour
{
    public GameObject targetObject; // ȣ���� ��� ������Ʈ
    public string methodName = "Activate"; // ȣ���� �޼ҵ� �̸�
    public float activationRange = 2f; // ��ư Ȱ��ȭ ���� (�Ÿ�)
    public bool IsPlayerInRange { get; private set; } = false;
    public bool IsButtonPressed { get; private set; } = false;


    void Update()
    {
        // ���� ����� �÷��̾� ã��
        GameObject closestPlayer = FindClosestPlayer();

        // ���� ����� �÷��̾���� �Ÿ� üũ �� 'E' Ű �Է� ����
        if (closestPlayer != null)
        {
            if (Vector3.Distance(transform.position, closestPlayer.transform.position) <= activationRange)
            {
                IsPlayerInRange = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PressButton();
                    
                }
                
            }
            else IsPlayerInRange = false; 
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


    public void PressButton()
    {
        if (targetObject != null)
        {
            //IsButtonPressed = true;
            targetObject.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
            Debug.Log($"{methodName} �޼ҵ尡 ȣ��Ǿ����ϴ�!");
        }
        else
        {
            Debug.LogWarning("Ÿ�� ������Ʈ�� �������� �ʾҽ��ϴ�.");
        }
        IsButtonPressed = true;
        StartCoroutine(ResetButtonAfterDelay(1f));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }


    private IEnumerator ResetButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð�(1��) ���
        IsButtonPressed = false; // ��ư ���� ����
        Debug.Log("��ư ���°� ���µǾ����ϴ�.");
    }
}