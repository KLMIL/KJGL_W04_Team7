using UnityEngine;

public class WordSManager : MonoBehaviour
{
    public GameObject button;
    public GameObject[] doors;
    public GameObject wall;


    // �̵��� �Ϸ�Ǿ����� üũ�ϴ� ����
    [SerializeField]
    private bool isClosing = false;
    private float speed = 1f;


    private GameObject player;

    void Update()
    {
        if (isClosing)
        {
            // ���� ���ʰ� ������ �κ� ��������
            Transform wallLeft = wall.transform.GetChild(0);
            Transform wallRight = wall.transform.GetChild(1);

            // �߾� ��ġ
            Vector3 wallCenter = wall.transform.position;

            // ��ǥ ��ġ ���� (���Ϳ��� ������ ����)
            Vector3 targetLeft = wallCenter + new Vector3(4f, 0f, 0f);  // left�� +4
            Vector3 targetRight = wallCenter + new Vector3(-4f, 0f, 0f); // right�� -4

            // ���� ��ġ���� ��ǥ ��ġ������ �Ÿ� ���
            float distanceLeft = Vector3.Distance(wallLeft.position, targetLeft);
            float distanceRight = Vector3.Distance(wallRight.position, targetRight);

            // �̵� ���� ���
            Vector3 directionLeft = (targetLeft - wallLeft.position).normalized;
            Vector3 directionRight = (targetRight - wallRight.position).normalized;

            // 1�ʿ� 1���� �̵��ϵ��� �ӵ� ����
            Vector3 newLeftPos = wallLeft.position + directionLeft * speed * Time.deltaTime;
            Vector3 newRightPos = wallRight.position + directionRight * speed * Time.deltaTime;

            // ��ǥ�� ����ġ�� �ʵ��� ����
            if (Vector3.Distance(wallLeft.position, targetLeft) > Vector3.Distance(newLeftPos, targetLeft))
            {
                wallLeft.position = newLeftPos;
            }
            else
            {
                wallLeft.position = targetLeft;
            }

            if (Vector3.Distance(wallRight.position, targetRight) > Vector3.Distance(newRightPos, targetRight))
            {
                wallRight.position = newRightPos;
            }
            else
            {
                wallRight.position = targetRight;
            }

            // ��ǥ ��ġ�� �����ߴ��� üũ
            if (Vector3.Distance(wallLeft.position, targetLeft) < 0.01f &&
                Vector3.Distance(wallRight.position, targetRight) < 0.01f)
            {
                isClosing = false;
                wallLeft.position = targetLeft;   // ��Ȯ�� ��ǥ ��ġ�� ����
                wallRight.position = targetRight;
            }
        }
    }


    public void ButtonListen(GameObject button)
    {
        foreach (GameObject door in doors)
        {
            door.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void wallClosing()
    {
        Vector3 wallLeft = wall.transform.GetChild(0).position;
        Vector3 wallRight = wall.transform.GetChild(1).position;

        Vector3 wallCenter = wall.transform.position;

        isClosing = true;  // �̵� ���� �÷���



    }


    void KillPlayer()
    {
        if (player != null)
        {
            GameManager.Instance.SetPlayerDead(true);
        }
    }

    

}
