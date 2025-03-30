using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class LineSManager : MonoBehaviour
{
    public GameObject[] buttons;    //버튼 1~6
    public List<GreenLineControl> lines;    //이어지는 선
    public GameObject[] lights;     //들어오는 불
    public GameObject[] checkLights; //정답
    private GreenLightControl[] lightConditions;
    public GameObject[] doors;

    private int[] correctSequence = { 5, 3, 1, 4 }; // 4개의 올바른 순서
    [SerializeField]
    private List<int> playerSequence = new List<int>(); // 플레이어 입력 순서
    private int currentStep = 0;
    private bool[] previousLightStates; // 이전 프레임의 라이트 상태 저장
    private bool isResetting = false;



    void Start()
    {

        lightConditions = new GreenLightControl[checkLights.Length];

        // 각 GameObject에서 LightCondition 스크립트 가져오기
        for (int i = 0; i < checkLights.Length; i++)
        {
            if (checkLights[i] != null)
            {
                lightConditions[i] = checkLights[i].GetComponent<GreenLightControl>();

            }
        }

        previousLightStates = new bool[lightConditions.Length];
        for (int i = 0; i < lightConditions.Length; i++)
        {
            previousLightStates[i] = false;
        }
    }

    void Update()
    {
        CheckLineConditions();
        CheckState();
    } 




    public void ButtonListen(GameObject button) 
    {
        int buttonIndex = Array.IndexOf(buttons, button);

        if (buttonIndex < 0) return;

        foreach (GameObject light in lights)
        {
            light.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver);
        }

        // 선택된 버튼에 해당하는 불빛만 켬 (Activate 호출)
        if (buttonIndex < lights.Length) // lights 배열 범위 체크
        {
            lights[buttonIndex].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
            checkLights[buttonIndex].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
        }
    }

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

    private void CheckLineConditions()
    {
        if (AreAllLinesActive())
        {
            DoorOpen();
        }
    }

    void DoorOpen()
    {
        if (doors != null)
        {
            foreach (GameObject obj in doors)
            {
                if (obj != null)
                {
                    obj.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    Debug.Log("문 열림!");
                }
            }

        }
        else
        {
            Debug.LogWarning("doors가 설정되지 않았습니다.");
        }
    }

    private void HandleLightActivation(int lightIndex)
    {
        playerSequence.Add(lightIndex);
        Debug.Log($"Light {lightIndex} activated");

        // 순서에 없는 라이트인지 확인
        bool isInvalidLight = true;
        foreach (int correctIndex in correctSequence)
        {
            if (lightIndex == correctIndex)
            {
                isInvalidLight = false;
                break;
            }
        }

        if (isInvalidLight)
        {
            Debug.Log("순서에 없는 라이트가 켜졌습니다. 실패!");
            Invoke("ResetLights", 0.1f);
            return;
        }

        // 순서가 맞는지 확인
        if (playerSequence[currentStep] == correctSequence[currentStep])
        {
            currentStep++;
            if (currentStep >= correctSequence.Length)
            {
                Debug.Log("성공! 4개의 라이트를 올바른 순서로 켰습니다.");
            }
        }
        else
        {
            Debug.Log("순서가 틀렸습니다. 실패!");
            Invoke("ResetLights", 0.1f);
        }
    }



    private void ResetLights()
    {
        isResetting = true;
        foreach (GameObject cLight in checkLights)
        {
            cLight.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver);

        }
        playerSequence.Clear();
        currentStep = 0;
        Invoke("FinishReset", 0.1f);
    }

    private void FinishReset()
    {
        for (int i = 0; i < previousLightStates.Length; i++)
        {
            previousLightStates[i] = false; // 이전 상태 초기화
        }
        isResetting = false;
        Debug.Log("리셋 완료");
    }


    void CheckState()
    {
        if (isResetting) return;
        for (int i = 0; i < lightConditions.Length; i++)
        {
            bool currentState = lightConditions[i].transform.GetChild(0).GetComponent<Light>().enabled;
            if (currentState && !previousLightStates[i]) // 꺼져있다가 켜진 경우
            {
                HandleLightActivation(i);
            }
            previousLightStates[i] = currentState;
        }
    }


}


