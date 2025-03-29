using System.Collections.Generic;
using UnityEngine;

public class CheckLight : MonoBehaviour
{
    public List<GreenLightControl> lightControls = new List<GreenLightControl>(); // GreenLightControl 오브젝트 리스트
    private int[] correctSequence = { 0,4,1,3 }; // 4개의 올바른 순서
    [SerializeField]
    private List<int> playerSequence = new List<int>(); // 플레이어 입력 순서
    private int currentStep = 0;
    private bool[] previousLightStates; // 이전 프레임의 라이트 상태 저장
    [SerializeField]
    private bool isResetting = false;

    void Start()
    {
        previousLightStates = new bool[lightControls.Count];
        for (int i = 0; i < lightControls.Count; i++)
        {
            previousLightStates[i] = false;
        }
    }

    void Update()
    {
        if (isResetting) return;

        for (int i = 0; i < lightControls.Count; i++)
        {
            bool currentState = lightControls[i].transform.GetChild(0).GetComponent<Light>().enabled;
            if (currentState && !previousLightStates[i]) // 꺼져있다가 켜진 경우
            {
                HandleLightActivation(i);
            }
            previousLightStates[i] = currentState;
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
        foreach (GreenLightControl lightControl in lightControls)
        {
            lightControl.Deactivate(); // GreenLightControl의 Deactivate 호출
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
}