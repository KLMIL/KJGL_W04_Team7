using UnityEngine;

public class ClosingWall_1 : MonoBehaviour
{
    public float moveDistance = 7f;      // ���� ������ �Ÿ�
    public float moveSpeed = 1f;         // ���ƿ��� �ӵ�

    private GameObject leftWall;
    private GameObject rightWall;
    public bool isPlayerPassed = false;
    public PlayerController playerController;
    private Vector3 leftOriginalPos;
    private Vector3 rightOriginalPos;
    private bool isOpening = false;
    private bool isClosing = false;
    private float closeTimer = 0f;

    void Awake()
    {
        leftWall = transform.GetChild(0).gameObject;
        rightWall = transform.GetChild(1).gameObject;
        leftOriginalPos = leftWall.transform.position;  // LeftWall�� �ʱ� ��ġ
        rightOriginalPos = rightWall.transform.position; // RightWall�� �ʱ� ��ġ

        // �ڽ� ������Ʈ Ȯ��
        Debug.Log($"LeftWall �ڽ� ��: {leftWall.transform.childCount}");
        Debug.Log($"RightWall �ڽ� ��: {rightWall.transform.childCount}");
    }

    void Update()
    {
        if (isClosing)
        {
            closeTimer += Time.deltaTime;

            if (closeTimer >= 1f)
            {
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

                // �̵� �� �ڽ� ��ġ �α�
                if (leftWall.transform.childCount > 0)
                {
                    Debug.Log($"LeftWall ù ��° �ڽ� ��ġ: {leftWall.transform.GetChild(0).position}");
                }
                if (rightWall.transform.childCount > 0)
                {
                    Debug.Log($"RightWall ù ��° �ڽ� ��ġ: {rightWall.transform.GetChild(0).position}");
                }

                if (t >= 1f)
                {
                    isClosing = false;
                    if (!isPlayerPassed)
                    {
                        playerController.HandleDie();
                    }
                    closeTimer = 0f;
                    
                }
            }
        }
    }

    public void Activate()
    {
        if (!isOpening && !isClosing)
        {
            isOpening = true;

            leftWall.transform.position = leftOriginalPos - Vector3.right * moveDistance;
            rightWall.transform.position = rightOriginalPos + Vector3.right * moveDistance;

            // Ȱ��ȭ �� ��ġ �α�
            Debug.Log($"LeftWall ��ġ: {leftWall.transform.position}");
            Debug.Log($"RightWall ��ġ: {rightWall.transform.position}");
            if (leftWall.transform.childCount > 0)
            {
                Debug.Log($"LeftWall ù ��° �ڽ� ��ġ: {leftWall.transform.GetChild(0).position}");
            }
            if (rightWall.transform.childCount > 0)
            {
                Debug.Log($"RightWall ù ��° �ڽ� ��ġ: {rightWall.transform.GetChild(0).position}");
            }

            isOpening = false;
            isClosing = true;
            closeTimer = 0f;
        }
    }
}