using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WallButtonOnceList : MonoBehaviour
{
    public List<GameObject> targetObjects; // ȣ���� ��� ������Ʈ
    public string methodName = "Activate"; // ȣ���� �޼ҵ� �̸�
    public float activationRange = 2f; // ��ư Ȱ��ȭ ���� (�Ÿ�)
    


    void Update()
    {
        // ���� ����� �÷��̾� ã��
        GameObject closestPlayer = FindClosestPlayer();

        // ���� ����� �÷��̾���� �Ÿ� üũ �� 'E' Ű �Է� ����
        if (closestPlayer != null && Vector3.Distance(transform.position, closestPlayer.transform.position) <= activationRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                InvokeMethod();
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


    private void InvokeMethod()
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
            
        }
        else
        {
            Debug.LogWarning("Ÿ�� ������Ʈ�� �������� �ʾҽ��ϴ�.");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }

}
