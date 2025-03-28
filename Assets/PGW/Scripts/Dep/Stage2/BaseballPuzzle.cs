using System.Collections;
using UnityEngine;

public class BaseballPuzzle : MonoBehaviour
{
    // 카운트 변수
    private int greenCount = 0;  // 초록 카운트 (최대 3)
    private int yellowCount = 0; // 노랑 카운트 (최대 3)
    private int redCount = 0;    // 빨강 카운트 (최대 1)

    // 자식 오브젝트의 불 배열
    private PuzzleLight[] greenLights;  // 초록 불 3개
    private PuzzleLight[] yellowLights; // 노랑 불 3개
    private PuzzleLight[] redLights;    // 빨강 불 1개
    private float lightDuration = 5f;   // 불이 켜진 후 꺼질 때까지의 시간 (초)
    public GameObject targetObject1; // 호출할 대상 오브젝트
    public GameObject targetObject2; // 호출할 대상 오브젝트
    private string methodName = "Activate"; // 호출할 메소드 이름

    void Start()
    {
        // 자식 오브젝트에서 불 초기화
        InitializeLights();
    }

    // 불 오브젝트 초기화
    private void InitializeLights()
    {
        greenLights = new PuzzleLight[3];
        yellowLights = new PuzzleLight[3];
        redLights = new PuzzleLight[1];

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Transform lightTransform = child.Find("light");
            if (lightTransform != null)
            {
                PuzzleLight light = lightTransform.GetComponent<PuzzleLight>(); // light 오브젝트에서 가져오기
                if (light != null)
                {
                    if (i < 3) // 0~2: 초록
                    {
                        greenLights[i] = light;
                    }
                    else if (i < 6) // 3~5: 노랑
                    {
                        yellowLights[i - 3] = light;
                    }
                    else if (i == 6) // 6: 빨강
                    {
                        redLights[0] = light;
                    }
                }
                else
                {
                    Debug.LogError($"light 오브젝트 {lightTransform.name}에 PuzzleLight 컴포넌트가 없습니다.");
                }
            }
            else
            {
                Debug.LogError($"light 오브젝트를 찾을 수 없습니다: {child.name}");
            }
        }

        TurnOffAllLights();
    }

    // 모든 불 끄기
    private void TurnOffAllLights()
    {
        foreach (var light in greenLights)
        {
            if (light != null) light.SetActive(false);
        }
        foreach (var light in yellowLights)
        {
            if (light != null) light.SetActive(false);
        }
        foreach (var light in redLights)
        {
            if (light != null) light.SetActive(false);
        }
    }
    public int GetGreenCount()
    {
        return greenCount;
    }

    public int GetYellowCount()
    {
        return yellowCount;
    }

    // 카운트 쌓는 메서드
    public void AddGreenCount()
    {
        if (greenCount < 3)
        {
            greenCount++;
            Debug.Log($"Green Count: {greenCount}");
        }
        else
        {
            Debug.Log("Green Count가 이미 최대치(3)입니다.");
        }
    }

    public void AddYellowCount()
    {
        if (yellowCount < 3)
        {
            yellowCount++;
            Debug.Log($"Yellow Count: {yellowCount}");
        }
        else
        {
            Debug.Log("Yellow Count가 이미 최대치(3)입니다.");
        }
    }

    public void AddRedCount()
    {
        if (redCount < 1)
        {
            redCount++;
            Debug.Log($"Red Count: {redCount}");
        }
        else
        {
            Debug.Log("Red Count가 이미 최대치(1)입니다.");
        }
    }


    // 불을 켜는 메서드
    public void TurnOnLights()
    {
        TurnOffAllLights();

        // 초록 불 켜기
        for (int i = 0; i < greenCount && i < greenLights.Length; i++)
        {
            if (greenLights[i] != null)
            {
                greenLights[i].SetActive(true);
            }
        }

        // 노랑 불 켜기
        for (int i = 0; i < yellowCount && i < yellowLights.Length; i++)
        {
            if (yellowLights[i] != null)
            {
                yellowLights[i].SetActive(true);
            }
        }

        // 빨강 불 켜기
        for (int i = 0; i < redCount && i < redLights.Length; i++)
        {
            if (redLights[i] != null)
            {
                redLights[i].SetActive(true);
            }
        }

        Debug.Log($"불이 켜졌습니다 - Green: {greenCount}, Yellow: {yellowCount}, Red: {redCount}");

        // 그린 카운트가 3인지 확인
        if (greenCount == 3)
        {
            OnGreenCountThree(); // 별도 행동 호출
        }
        else
        {
            StartCoroutine(ResetPuzzleAfterDelay());
        }
    }

    // 지정된 시간 후 불 끄고 카운트 초기화
    private IEnumerator ResetPuzzleAfterDelay()
    {
        yield return new WaitForSeconds(lightDuration);
        TurnOffAllLights();
        greenCount = 0;
        yellowCount = 0;
        redCount = 0;
        Debug.Log("불이 꺼지고 카운트가 초기화되었습니다.");
    }

    // 그린 카운트가 3일 때 실행할 행동
    private void OnGreenCountThree()
    {
        Debug.Log("그린 카운트가 3입니다! 불이 꺼지지 않고 추가 행동을 실행할 수 있습니다.");
        // 여기에 원하는 추가 행동을 구현하세요
        // 타겟 오브젝트의 메소드 호출
        if (targetObject1 != null)
        {
            targetObject1.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
            Debug.Log($"{methodName} 메소드가 호출되었습니다!");
        }
        else
        {
            Debug.LogWarning("타겟 오브젝트가 설정되지 않았습니다.");
        }
        if (targetObject2 != null)
        {
            targetObject2.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
            Debug.Log($"{methodName} 메소드가 호출되었습니다!");
        }
        else
        {
            Debug.LogWarning("타겟 오브젝트가 설정되지 않았습니다.");
        }
    }

    // 디버깅용: 현재 카운트 확인
    public void LogCounts()
    {
        Debug.Log($"현재 카운트 - Green: {greenCount}, Yellow: {yellowCount}, Red: {redCount}");
    }
}