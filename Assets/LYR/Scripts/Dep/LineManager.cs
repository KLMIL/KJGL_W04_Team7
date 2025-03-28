using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public List<GreenLineControl> lines; // 관리할 모든 GreenLineControl 오브젝트

    private bool isButtonPressed = false; // 버튼이 눌렸는지 여부

    public string methodName = "Activate"; // 호출할 메소드 이름
    public List<GameObject> targetObjects;

    void Start()
    {

    }

    void Update()
    {
        CheckConditions();
    }

    // 모든 라인이 활성화되고 버튼이 눌렸는지 확인
    private void CheckConditions()
    {
        if (AreAllLinesActive())
        {
            InvokeMethod();
            enabled = false; // 더 이상 체크하지 않도록 스크립트 비활성화 (선택사항)
        }
    }

    // 모든 라인이 활성화되었는지 확인
    private bool AreAllLinesActive()
    {
        if (lines == null || lines.Count == 0) return false;

        foreach (GreenLineControl line in lines)
        {
            if (!line.IsCorrect) // public getter를 사용해 상태 확인
            {
                return false;
            }
        }
        return true;
    }


    private void InvokeMethod()
    {
        if (targetObjects != null)
        {
            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                {
                    obj.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
                    Debug.Log($"{methodName} 메소드가 호출되었습니다!");
                }
            }

        }
        else
        {
            Debug.LogWarning("타겟 오브젝트가 설정되지 않았습니다.");
        }
    }




}