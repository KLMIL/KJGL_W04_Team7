using UnityEngine;

public class CheckLight : MonoBehaviour
{

    private Light objectLight;           // 오브젝트의 Light 컴포넌트
    private bool isActive = false;       // 활성화 상태 체크
    private float activeTimer = 0f;      // 타이머

    void Start()
    {


        objectLight = GetComponent<Light>();
        if (objectLight != null)
        {
            objectLight.enabled = false;     // 처음에는 꺼진 상태
        }
    }

    void Update()
    {
        //if (isActive)
        //{
        //    activeTimer += Time.deltaTime;

            
        //    if (activeTimer >= activeDuration)
        //    {

        //        if (objectLight != null)
        //        {
        //            objectLight.enabled = false;    // 라이트 끄기
        //        }
        //        isActive = false;
        //        activeTimer = 0f;
        //    }
        //}

    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            activeTimer = 0f;


            if (objectLight != null)
            {
                objectLight.enabled = true;
            }
        }
    }
}
