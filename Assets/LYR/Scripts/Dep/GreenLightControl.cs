using UnityEngine;

public class GreenLightControl : MonoBehaviour
{

    private Light objectLight;           // 오브젝트의 Light 컴포넌트
    private bool isActive = false;       // 활성화 상태 체크
    private float activeTimer = 0f;      // 타이머


    public bool autoOff = false;         // 시간 지나면 자동 Off
    public float activeDuration = 2f;    // 활성화 상태 유지 시간 (초)

    void Start()
    {
        objectLight = transform.GetChild(0).GetComponent<Light>();
        if (objectLight != null)
        {
            objectLight.enabled = false;     // 처음에는 꺼진 상태
        }
    }

    void Update()
    {
        if (isActive && autoOff)
        {
            activeTimer += Time.deltaTime;
            // 지정된 시간이 지나면 원래 상태로 복구
            if (activeTimer >= activeDuration)
            {
                if (objectLight != null)
                {
                    objectLight.enabled = false;    // 라이트 끄기
                }
                isActive = false;
                activeTimer = 0f;
            }
        }
    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            activeTimer = 0f;


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
