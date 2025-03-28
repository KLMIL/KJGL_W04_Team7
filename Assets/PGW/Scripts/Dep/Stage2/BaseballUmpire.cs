using UnityEngine;

public class BaseballUmpire : MonoBehaviour
{
    private BaseballPuzzle puzzle; // BaseballPuzzle 스크립트 참조
    private int[] targetNumbers = { 6, 3, 7 }; // 비교할 숫자 배열
    private int currentIndex = 0; // 현재 비교할 숫자의 인덱스
    private int activateCallCount = 0; // Activate 호출 횟수

    void Start()
    {
        // BaseballPuzzle 컴포넌트를 가져옴
        puzzle = GetComponent<BaseballPuzzle>();
        if (puzzle == null)
        {
            Debug.LogError("BaseballPuzzle 스크립트를 찾을 수 없습니다.");
        }
    }

    // Activate 메서드: 버튼 입력(0~9)을 받아 카운트를 업데이트
    public void Activate(int inputNumber)
    {
        // 입력 유효성 검사
        if (inputNumber < 0 || inputNumber > 9)
        {
            Debug.LogError("입력 숫자는 0~9 사이여야 합니다.");
            return;
        }

        // 현재 비교 대상 숫자
        int currentTarget = targetNumbers[currentIndex];

        if (inputNumber == currentTarget)
        {
            // 입력이 현재 대상과 같으면 그린 카운트 증가
            puzzle.AddGreenCount();
            Debug.Log($"입력: {inputNumber}, 대상: {currentTarget} → Green Count 증가");
        }
        else if (inputNumber == targetNumbers[0] || inputNumber == targetNumbers[1] || inputNumber == targetNumbers[2])
        {
            // 입력이 현재 대상과 다르고 다른 두 숫자 중 하나와 같으면 옐로우 카운트 증가
            puzzle.AddYellowCount();
            Debug.Log($"입력: {inputNumber}, 대상: {currentTarget} → Yellow Count 증가");
        }
        else if (currentIndex == 2 && puzzle.GetGreenCount() == 0 && puzzle.GetYellowCount() == 0)
        {
            // 인덱스가 2이고 그린/옐로우 카운트가 0일 때만 레드 카운트 증가
            puzzle.AddRedCount();
            Debug.Log($"입력: {inputNumber}, 대상: {currentTarget} → Red Count 증가 (인덱스 2, 그린/옐로우 0)");
        }
        else
        {
            Debug.Log($"입력: {inputNumber}, 대상: {currentTarget} → 조건에 맞지 않음, 카운트 변화 없음");
        }

        // 다음 비교 대상으로 이동 (0, 1, 2 순환)
        currentIndex = (currentIndex + 1) % 3;

        // 호출 횟수 증가 및 3번마다 TurnOnLights 호출
        activateCallCount++;
        if (activateCallCount >= 3)
        {
            puzzle.TurnOnLights();
            activateCallCount = 0; // 호출 횟수 초기화
            Debug.Log("Activate가 3번 호출되어 TurnOnLights를 실행했습니다.");
        }
    }
}