using System.Collections;
using UnityEngine;

public class PlayerCameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.1f;
    [SerializeField] private float dampingSpeed = 1.0f;
    [SerializeField] private Transform playerController; // PlayerController의 Transform 참조

    private Vector3 originalOffset; // PlayerController에 대한 상대적 오프셋
    private bool isShaking = false;

    private void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController가 설정되지 않았습니다!");
            return;
        }
        // 초기 상대적 오프셋 계산
        originalOffset = transform.position - playerController.position;
    }

    private void Update()
    {
        if (playerController == null) return;

        if (isShaking)
        {
            // 흔들리는 동안 플레이어 위치 + 오프셋으로 부드럽게 복귀
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
            // 흔들리지 않을 때는 플레이어 위치에 오프셋 적용
            transform.position = playerController.position + originalOffset;
        }
    }

    /// <summary>
    /// 카메라 진동 시간, 카메라 진동 속도, 카메라 위치 복귀 속도
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
            // 플레이어 위치를 기준으로 흔들림 적용
            Vector3 basePos = playerController.position + originalOffset;
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            transform.position = basePos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}