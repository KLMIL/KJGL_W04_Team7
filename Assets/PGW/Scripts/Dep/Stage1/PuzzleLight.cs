using System;
using UnityEngine;

public class PuzzleLight : MonoBehaviour
{
    private Material material;           // 오브젝트의 머터리얼
    private Color originalColor;         // 원래 색상 저장
    private Light objectLight;           // 오브젝트의 Light 컴포넌트
    private bool isActive = false;       // 활성화 상태 체크

    public Action<bool, GameObject> onSetActive; // SetActive 호출 시 발생하는 이벤트

    
    void Start()
    {
        // 머터리얼과 라이트 컴포넌트 가져오기
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            originalColor = material.color;  // 원래 색상 저장
            // 생성 시 #11820A 색상으로 변경 (16진수 -> RGB 변환)
            //material.color = new Color32(17, 130, 10, 255); // #11820A
        }

        objectLight = GetComponent<Light>();
        if (objectLight != null)
        {
            objectLight.enabled = false;     // 처음에는 꺼진 상태
        }
    }


    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;

            if (material != null)
            {
                // 원래 색상으로 복원
                material.color = originalColor;
            }

            if (objectLight != null)
            {
                // 라이트 켜기
                SetActive(true);
            }
        }
    }

    public void SetActive(bool active)
    {
        objectLight.enabled = active;
        isActive = false;
        onSetActive?.Invoke(active, gameObject); // 이벤트 발생
    }
}
