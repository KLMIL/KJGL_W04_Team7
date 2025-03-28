using UnityEngine;

public class GreenLightControl : MonoBehaviour
{
    //private Material material;           // 오브젝트의 머터리얼
    //private Color originalColor;         // 원래 색상 저장
    private Light objectLight;           // 오브젝트의 Light 컴포넌트
    private bool isActive = false;       // 활성화 상태 체크
    private float activeTimer = 0f;      // 타이머

    // 공용 변수
    public float activeDuration = 2f;    // 활성화 상태 유지 시간 (초)

    void Start()
    {
        // 머터리얼과 라이트 컴포넌트 가져오기
        /*        Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    material = renderer.material;
                    originalColor = material.color;  // 원래 색상 저장
                    // 생성 시 #11820A 색상으로 변경 (16진수 -> RGB 변환)
                    material.color = new Color32(17, 130, 10, 255); // #11820A
                }*/

        objectLight = transform.GetChild(0).GetComponent<Light>();
        if (objectLight != null)
        {
            objectLight.enabled = false;     // 처음에는 꺼진 상태
        }
    }

    void Update()
    {
        if (isActive)
        {
            activeTimer += Time.deltaTime;

            // 지정된 시간이 지나면 원래 상태로 복구
            if (activeTimer >= activeDuration)
            {
                /*                if (material != null)
                                {
                                    material.color = new Color32(17, 130, 10, 255); // #11820A로 복구
                                }*/
                if (objectLight != null)
                {
                    objectLight.enabled = false;    // 라이트 끄기
                }
                isActive = false;
                activeTimer = 0f;
            }
        }
        //Activate();
    }

    public void ButtonListen()
    {
        if (!isActive)
        {
            isActive = true;
            activeTimer = 0f;

            /*            if (material != null)
                        {
                            // 원래 색상으로 복원
                            material.color = originalColor;
                        }*/

            if (objectLight != null)
            {
                // 라이트 켜기
                objectLight.enabled = true;
            }
        }
    }

    public void Deactivate()
    {
        if (objectLight != null)
        {
            objectLight.enabled = false; // 라이트 끄기
        }
        isActive = false;
        activeTimer = 0f;
    }

}
