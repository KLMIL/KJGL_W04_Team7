using UnityEngine;

public class ButtonLight : MonoBehaviour
{
    public WallButtonOnce button; // 참조할 WallButtonOnce 스크립트
    public Light targetLight; // 제어할 Light 컴포넌트

    void Start()
    {
        // 시작 시 조명 상태 초기화 (선택적)
        if (targetLight != null)
        {
            targetLight.enabled = false; // 기본적으로 꺼짐
        }
    }

    void Update()
    {
        if (button == null)
        {
            Debug.LogWarning("WallButtonOnce 스크립트가 연결되지 않았습니다.");
            return;
        }

        if (targetLight == null)
        {
            Debug.LogWarning("Light 컴포넌트가 연결되지 않았습니다.");
            return;
        }

        // IsButtonPressed가 true면 조명을 켜고, false면 끔
        targetLight.enabled = button.IsButtonPressed;
    }
}
