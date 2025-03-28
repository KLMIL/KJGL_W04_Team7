using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class NumberPuzzle : MonoBehaviour
{
    private PuzzleLight[] lights; // 4개의 light 스크립트 배열
    private List<int> activationOrder; // 실제 활성화 순서를 기록
    private int[] correctOrder = { 2, 1, 0, 3 }; // 올바른 순서
    private bool isPuzzleComplete = false; // 퍼즐 완료 여부
    public GameObject targetObject; // 호출할 대상 오브젝트
    private string methodName = "Activate"; // 호출할 메소드 이름

    void Start()
    {
        // 초기화
        activationOrder = new List<int>();

        // 자식 오브젝트에서 light 컴포넌트 가져오기
        lights = new PuzzleLight[4];
        for (int i = 0; i < 4; i++)
        {
            Transform child = transform.GetChild(i); // Buttonboard의 i번째 자식
            Transform lightTransform = child.Find("light"); // 자식의 light 오브젝트
            if (lightTransform != null)
            {
                lights[i] = lightTransform.GetComponent<PuzzleLight>();
                if (lights[i] != null)
                {
                    // Light 스크립트에 이벤트 핸들러 추가 (SetActive 호출 감지)
                    lights[i].onSetActive += HandleLightActivation;
                }
                else
                {
                    Debug.LogError($"light 오브젝트에 PuzzleLight 스크립트가 없습니다: {lightTransform.name}");
                }
            }
            else
            {
                Debug.LogError($"light 오브젝트를 찾을 수 없습니다: {child.name}");
            }
        }
    }

    // Light가 활성화될 때 호출되는 핸들러
    private void HandleLightActivation(bool isActive, GameObject lightObject)
    {
        if (isPuzzleComplete) return; // 퍼즐이 완료되면 더 이상 체크하지 않음

        if (isActive) // 활성화될 때만 순서 체크
        {
            // 활성화된 light의 인덱스 찾기
            int index = Array.FindIndex(lights, l => l.gameObject == lightObject);
            if (index != -1 && !activationOrder.Contains(index)) // 중복 방지
            {
                activationOrder.Add(index);
                CheckOrder();
            }
        }
    }

    // 순서가 맞는지 확인
    private void CheckOrder()
    {
        Debug.Log($"현재 순서: {string.Join(", ", activationOrder)}");

        if (activationOrder.Count == correctOrder.Length)
        {
            bool isCorrect = activationOrder.SequenceEqual(correctOrder);
            if (isCorrect)
            {
                Debug.Log("순서가 맞습니다! 퍼즐 완료!");
                isPuzzleComplete = true; // 퍼즐 완료 상태로 설정

                // 타겟 오브젝트의 메소드 호출
                if (targetObject != null)
                {
                    targetObject.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
                    Debug.Log($"{methodName} 메소드가 호출되었습니다!");
                }
                else
                {
                    Debug.LogWarning("타겟 오브젝트가 설정되지 않았습니다.");
                }

                // 3초 후 리셋을 위해 코루틴 시작
                StartCoroutine(ResetAfterDelay(3f));
            }
            else
            {
                Debug.Log("순서가 틀렸습니다. 초기화합니다.");
                ResetPuzzle();
            }
        }
    }

    // 퍼즐 초기화
    private void ResetPuzzle()
    {
        activationOrder.Clear(); // 한 번만 호출
        foreach (var light in lights)
        {
            if (light != null)
            {
                light.SetActive(false); // 모든 라이트 비활성화
            }
        }
        isPuzzleComplete = false; // 퍼즐 완료 상태 해제
        Debug.Log("퍼즐이 초기화되었습니다. 다시 시도하세요.");
    }

    // 지정된 시간 후 퍼즐을 리셋하는 코루틴
    private System.Collections.IEnumerator ResetAfterDelay(float delay)
    {
        Debug.Log($"퍼즐 완료 후 {delay}초 대기 후 리셋됩니다.");
        yield return new WaitForSeconds(delay);
        ResetPuzzle();
    }
}