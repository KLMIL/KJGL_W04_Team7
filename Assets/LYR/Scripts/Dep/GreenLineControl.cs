using System.Collections.Generic;
using UnityEngine;

public class GreenLineControl : MonoBehaviour
{
    private Material material;
    private Color originalEmissionColor;
    private float activeTimer = 0f;
    private bool isCorrect = false;

    public float activeDuration = 2f;
    public List<GameObject> lightsToCheck;
    public GreenLineControl dependentLine;

    // 상태를 외부에서 읽을 수 있도록 public getter 추가
    public bool IsCorrect => isCorrect;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            originalEmissionColor = material.GetColor("_EmissionColor");
            material.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        // 라이트 상태 체크 및 색상 업데이트
        if (isCorrect)
        {
            activeTimer += Time.deltaTime;

            // 두 라이트가 모두 켜져있지 않으면 즉시 원래 상태로 복구
            if (!AreAssignedLightsActive())
            {
                ResetColor();

            }
            // 지정된 시간이 지나면 원래 상태로 복구
            else if (activeTimer >= activeDuration)
            {
                ResetColor();

            }
        }
        else
        {
            Activate();

        }
    }

    public void Activate()
    {
        if (!isCorrect && AreAssignedLightsActive())
        {
            isCorrect = true;
            activeTimer = 0f;
            Color newEmissionColor = new Color(7f / 255f, 120f / 255f, 0f / 255f);

            if (material != null)
            {
                material.SetColor("_EmissionColor", newEmissionColor);
            }
        }
    }

    private void ResetColor()
    {
        if (material != null)
        {
            material.SetColor("_EmissionColor", originalEmissionColor);
        }
        isCorrect = false;
        activeTimer = 0f;
    }

    private bool AreAssignedLightsActive()
    {
        if (lightsToCheck == null || lightsToCheck.Count != 2) return false;

        // 각 오브젝트에서 Light 컴포넌트 가져오기
        Light light1 = lightsToCheck[0].GetComponent<Light>();
        Light light2 = lightsToCheck[1].GetComponent<Light>();

        // Light 컴포넌트가 없거나 비활성화 상태면 false 반환
        if (light1 == null || light2 == null) return false;

        return light1.enabled && light2.enabled;
    }

    private bool IsDependentLineActive()
    {
        // dependentLine이 null이면 조건을 무시 (true 반환)
        if (dependentLine == null) return true;

        // dependentLine의 IsCorrect가 false면 false 반환
        return dependentLine.IsCorrect;
    }

    private void TurnOffSecondLight()
    {
        if (lightsToCheck == null || lightsToCheck.Count != 2) return;

        Light light2 = lightsToCheck[1].GetComponent<Light>();
        if (light2 != null)
        {
            light2.enabled = false;
            Debug.Log($"{gameObject.name}의 두 번째 라이트가 꺼졌습니다.");
        }
    }



}
