using System.Collections;
using UnityEngine;

public class PlayerCameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.1f;
    [SerializeField] private float dampingSpeed = 1.0f;
    [SerializeField] private Transform playerController; // PlayerController�� Transform ����

    private Vector3 originalOffset; // PlayerController�� ���� ����� ������
    private bool isShaking = false;

    private void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController�� �������� �ʾҽ��ϴ�!");
            return;
        }
        // �ʱ� ����� ������ ���
        originalOffset = transform.position - playerController.position;
    }

    private void Update()
    {
        if (playerController == null) return;

        if (isShaking)
        {
            // ��鸮�� ���� �÷��̾� ��ġ + ���������� �ε巴�� ����
            Vector3 targetPos = playerController.position + originalOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * dampingSpeed);
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;
                isShaking = false;
            }
        }
        else
        {
            // ��鸮�� ���� ���� �÷��̾� ��ġ�� ������ ����
            transform.position = playerController.position + originalOffset;
        }
    }

    /// <summary>
    /// ī�޶� ���� �ð�, ī�޶� ���� �ӵ�, ī�޶� ��ġ ���� �ӵ�
    /// </summary>
    public void ShakeCamera(float shakeDuration = 0.5f, float shakeMagnitude = 0.1f, float dampingSpeed = 1.0f)
    {
        this.shakeDuration = shakeDuration;
        this.shakeMagnitude = shakeMagnitude;
        this.dampingSpeed = dampingSpeed;

        if (!isShaking)
        {
            StartCoroutine(DoShake());
        }
    }

    private IEnumerator DoShake()
    {
        if (playerController == null) yield break;

        isShaking = true;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // �÷��̾� ��ġ�� �������� ��鸲 ����
            Vector3 basePos = playerController.position + originalOffset;
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            transform.position = basePos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}