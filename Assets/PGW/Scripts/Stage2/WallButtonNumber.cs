using UnityEngine;

public class WallButtonNumber : MonoBehaviour
{
    public GameObject targetObject; // ȣ���� ��� ������Ʈ
    public string methodName = "Activate"; // ȣ���� �޼ҵ� �̸�
    public float activationRange = 2f; // ��ư Ȱ��ȭ ���� (�Ÿ�)
    public int buttonNumber = 0; // �̸� ���� ���� (0~9)

    void Update()
    {
        // ���� ����� �÷��̾� ã��
        GameObject closestPlayer = FindClosestPlayer();

        // ���� ����� �÷��̾���� �Ÿ� üũ �� 'E' Ű �Է� ����
        if (closestPlayer != null)
        {
            if (Vector3.Distance(transform.position, closestPlayer.transform.position) <= activationRange)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PressButton();
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

    public void PressButton()
    {
        if (targetObject != null)
        {
            // buttonNumber�� �μ��� �����Ͽ� SendMessage ȣ��
            targetObject.SendMessage(methodName, buttonNumber, SendMessageOptions.DontRequireReceiver);
            Debug.Log($"{methodName} �޼ҵ尡 {buttonNumber}�� �Բ� ȣ��Ǿ����ϴ�!");
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