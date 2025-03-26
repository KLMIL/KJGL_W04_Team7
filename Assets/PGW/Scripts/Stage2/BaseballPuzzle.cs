using UnityEngine;
using System.Collections;

public class BaseballPuzzle : MonoBehaviour
{
    // 카운트 변수
    private int greenCount = 0;  // 초록 카운트 (최대 3)
    private int yellowCount = 0; // 노랑 카운트 (최대 3)
    private int redCount = 0;    // 빨강 카운트 (최대 1)

    // 자식 오브젝트의 불 배열 (PuzzleLight 컴포넌트 가정)
    private PuzzleLight[] greenLights;  // 초록 불 Robinhood puzzles로 시작하는 경우가 많다. 초록색과 노란색은 최대 3개, 빨간색은 최대 1개까지 쌓을 수 있다.
    private PuzzleLight[] yellowLights;
    private PuzzleLight[] redLights;
    private float lightDuration = 5f; // 불이 켜진 후 꺼질 때까지의 시간 (초)

    void Start()
    {
        // 자식 오브젝트에서 불 초기화
        InitializeLights();
    }

    // 불 오브젝트 초기화
    private void InitializeLights()
    {
        // 자식 오브젝트가 총 7개(초록 3, 노랑 3, 빨강 1)라고 가정
        greenLights = new PuzzleLight[3];
        yellowLights = new PuzzleLight[3];
        redLights = new PuzzleLight[1];

        // 자식 오브젝트에서 PuzzleLight 컴포넌트 가져오기
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Transform lightTransform = child.Find("light"); // 자식의 light 오브젝트
            if (lightTransform != null)
            {
                PuzzleLight light = child.GetComponent<PuzzleLight>();
                if (light != null)
                {
                    // 인덱스로 초록, 노랑, 빨강 구분
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
                    Debug.LogError($"자식 오브젝트 {child.name}에 PuzzleLight 컴포넌트가 없습니다.");
                }
            }
            else
            {
                Debug.LogError($"light 오브젝트를 찾을 수 없습니다: {child.name}");
            }
            
            
        }

        // 초기 상태에서 모든 불 끄기
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
        // 모든 불 먼저 끄기
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

        // 불 켜진 후 지정된 시간 후에 꺼지고 카운트 초기화
        StartCoroutine(ResetPuzzleAfterDelay());
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

    // 디버깅용: 현재 카운트 확인
    public void LogCounts()
    {
        Debug.Log($"현재 카운트 - Green: {greenCount}, Yellow: {yellowCount}, Red: {redCount}");
    }
}