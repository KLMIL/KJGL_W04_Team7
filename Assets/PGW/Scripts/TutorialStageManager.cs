using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TutorialStageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] lights;
    [SerializeField] private GameObject door;

    private List<int> activationOrder; // 실제 활성화 순서를 기록
    private int[] correctOrder = { 2, 1, 0, 3 }; // 올바른 순서 (예시, 필요에 따라 수정)
    private bool isPuzzleComplete = false; // 퍼즐 완료 여부

    void Start()
    {
        activationOrder = new List<int>(); // 순서 리스트 초기화
    }

    public void ButtonListen(GameObject button)
    {
        if (isPuzzleComplete) return; // 퍼즐 완료 시 더 이상 작동하지 않음

        // buttons 배열에서 button의 인덱스 찾기
        int index = System.Array.IndexOf(buttons, button);

        // 인덱스가 유효하고 lights 배열에 해당 인덱스가 존재하면 처리
        if (index != -1 && index < lights.Length && lights[index] != null)
        {
            // 이미 활성화된 경우 중복 체크 방지
            if (!activationOrder.Contains(index))
            {
                lights[index].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                activationOrder.Add(index); // 순서에 추가
                CheckOrder(); // 순서 확인
            }
        }
    }

    // 순서가 맞는지 확인
    private void CheckOrder()
    {
        Debug.Log($"현재 순서: {string.Join(", ", activationOrder)}");

        // 모든 버튼이 눌렸는지 확인
        if (activationOrder.Count == correctOrder.Length)
        {
            bool isCorrect = activationOrder.SequenceEqual(correctOrder);
            if (isCorrect)
            {
                Debug.Log("순서가 맞습니다! 퍼즐 완료!");
                isPuzzleComplete = true;

                // 모든 문 열기 (선택적)
                if (door != null)
                {
                    door.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                }

                // 3초 후 리셋 (선택적)
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
        activationOrder.Clear();
        foreach (var light in lights)
        {
            if (light != null)
            {
                light.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver); // 비활성화 메소드 필요
            }
        }
        isPuzzleComplete = false;
        Debug.Log("퍼즐이 초기화되었습니다.");
    }

    // 지정된 시간 후 리셋하는 코루틴
    private System.Collections.IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetPuzzle();
    }
}
