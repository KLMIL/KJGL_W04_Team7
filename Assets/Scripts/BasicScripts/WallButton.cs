using System.Collections;
using UnityEngine;

public class WallButton : MonoBehaviour
{
    public GameObject targetObject; // 호출할 대상 오브젝트
    public string methodName = "Activate"; // 호출할 메소드 이름

    public bool IsButtonPressed { get; private set; } = false;

    public void PressButton()
    {
        if (targetObject != null)
        {
            targetObject.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
            Debug.Log($"{methodName} 메소드가 호출되었습니다!");
        }
        else
        {
            Debug.LogWarning("타겟 오브젝트가 설정되지 않았습니다.");
        }
        IsButtonPressed = true;
        StartCoroutine(ResetButtonAfterDelay(1f)); // 코루틴 시작
    }


    private IEnumerator ResetButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간(1초) 대기
        IsButtonPressed = false; // 버튼 상태 리셋
        Debug.Log("버튼 상태가 리셋되었습니다.");
    }
}
