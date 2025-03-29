using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public float moveDistance = 7f;      // ���� ������ �Ÿ�
    public float moveSpeed = 1f;         // ������ �ӵ�

    private GameObject leftWall;
    private GameObject rightWall;
    private Vector3 leftOriginalPos;
    private Vector3 rightOriginalPos;
    private bool isOpening = false;
    private bool isOpened = false;       // ���� �����ִ��� ���¸� ����
    private float openProgress = 0f;

    void Awake()
    {
        leftWall = transform.GetChild(0).gameObject;
        rightWall = transform.GetChild(1).gameObject;
        leftOriginalPos = leftWall.transform.position;  // LeftWall�� �ʱ� ��ġ (���� ����)
        rightOriginalPos = rightWall.transform.position; // RightWall�� �ʱ� ��ġ (���� ����)
    }

    void Update()
    {
        if (isOpening)
        {
            openProgress += Time.deltaTime * moveSpeed;

            // ���� ������ �������� �̵�
            leftWall.transform.position = Vector3.Lerp(
                leftOriginalPos,
                leftOriginalPos - Vector3.right * moveDistance,
                openProgress
            );
            rightWall.transform.position = Vector3.Lerp(
                rightOriginalPos,
                rightOriginalPos + Vector3.right * moveDistance,
                openProgress
            );

            // ������ �Ϸ�Ǹ� ����
            if (openProgress >= 1f)
            {
                isOpening = false;
                isOpened = true;    // ���� ������ �������� ǥ��
                openProgress = 1f;
            }
        }
    }

    public void Activate()
    {
        // ���� �������� �ʰ�, ������ ���� �ƴ� ���� �۵�
        if (!isOpened && !isOpening)
        {
            isOpening = true;
            openProgress = 0f;
        }
    }


    public void DeActivate()
    {
        if (isOpened)
        {
            leftWall.transform.position = leftOriginalPos;
            rightWall.transform.position = rightOriginalPos;
            isOpened = false;
        }
    }
}