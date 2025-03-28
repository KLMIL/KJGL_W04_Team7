using UnityEngine;

public class ClosingWall : MonoBehaviour
{
    // ���� ����
    public GameObject wallPrefab;        // ������ ����� ť�� ������
    public float initialDistance = 1f;   // �ʱ� ���� �Ÿ� (�߽����κ����� �Ÿ�)
    public float moveDistance = 2f;      // ���� ������ �Ÿ�
    public float moveSpeed = 1f;         // ���ƿ��� �ӵ�

    // ���� ����
    private GameObject leftWall;
    private GameObject rightWall;
    private Vector3 leftOriginalPos;
    private Vector3 rightOriginalPos;
    private bool isOpening = false;
    private bool isClosing = false;
    private float closeTimer = 0f;

    void Start()
    {
        // �ʱ� �� ����
        CreateWalls();
    }

    void Update()
    {
        if (isClosing)
        {
            closeTimer += Time.deltaTime;

            // 1�� ��� �� ������ ����
            if (closeTimer >= 1f)
            {
                // Lerp�� ����Ͽ� �ε巴�� ���� ��ġ�� �̵�
                float t = (closeTimer - 1f) * moveSpeed;
                leftWall.transform.position = Vector3.Lerp(
                    leftOriginalPos - Vector3.right * moveDistance,
                    leftOriginalPos,
                    t
                );
                rightWall.transform.position = Vector3.Lerp(
                    rightOriginalPos + Vector3.right * moveDistance,
                    rightOriginalPos,
                    t
                );

                // �̵� �Ϸ� üũ
                if (t >= 1f)
                {
                    isClosing = false;
                    closeTimer = 0f;
                }
            }
        }
        //Activate();
    }

    void CreateWalls()
    {
        // ���� �� ����
        leftWall = Instantiate(wallPrefab, transform);
        leftWall.transform.localPosition = Vector3.left * initialDistance;
        leftOriginalPos = leftWall.transform.position;

        // ������ �� ����
        rightWall = Instantiate(wallPrefab, transform);
        rightWall.transform.localPosition = Vector3.right * initialDistance;
        rightOriginalPos = rightWall.transform.position;
    }

    public void Activate()
    {
        if (!isOpening && !isClosing)
        {
            isOpening = true;

            // ��� ������ ����
            leftWall.transform.position = leftOriginalPos - Vector3.right * moveDistance;
            rightWall.transform.position = rightOriginalPos + Vector3.right * moveDistance;

            isOpening = false;
            isClosing = true;
            closeTimer = 0f;
        }
    }
}
