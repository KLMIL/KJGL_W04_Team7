using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class TutorialStageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons; // 총 6개 버튼 (0, 1: 벽 이동, 2~5: 퍼즐)
    [SerializeField] private PuzzleLight[] puzzleLights; // 퍼즐용 라이트 4개 (인덱스 2~5에 대응)
    [SerializeField] private GameObject[] doors; // 퍼즐 완료 시 열릴 문 (크기 2)
    [SerializeField] private GameObject[] walls; // 버튼 0, 1이 제어할 벽 (크기 2)

    private List<int> activationOrder;
    private int[] correctOrder = { 2, 1, 0, 3 }; // 퍼즐 순서
    private bool isPuzzleComplete = false;

    void Start()
    {
        activationOrder = new List<int>();
        if (buttons.Length != 6 || puzzleLights.Length != 4 || walls.Length != 2 || doors.Length != 2)
        {
            Debug.LogError("buttons는 6개, puzzleLights는 4개, walls는 2개, doors는 2개여야 합니다!");
        }
        // puzzleLights 초기화 확인
        for (int i = 0; i < puzzleLights.Length; i++)
        {
            if (puzzleLights[i] == null)
            {
                Debug.LogError($"puzzleLights[{i}]가 null입니다!");
            }
        }
        // doors 초기화 확인
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i] == null)
            {
                Debug.LogError($"doors[{i}]가 null입니다!");
            }
        }
    }

    public void ButtonListen(GameObject button)
    {
        if (isPuzzleComplete) return;

        int index = System.Array.IndexOf(buttons, button);
        Debug.Log($"Button Index: {index}");

        if (index == -1) return;

        // 인덱스 0, 1: 벽 제어
        if (index == 0)
        {
            if (walls[0] != null)
            {
                walls[0].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Wall 1 이동 시도");
            }
            else
            {
                Debug.LogError("walls[0]이 null입니다!");
            }
        }
        else if (index == 1)
        {
            if (walls[1] != null)
            {
                walls[1].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Wall 2 이동 시도");
            }
            else
            {
                Debug.LogError("walls[1]이 null입니다!");
            }
        }
        // 인덱스 2~5: 퍼즐 순서 체크
        else if (index >= 2 && index < buttons.Length && index - 2 < puzzleLights.Length)
        {
            int lightIndex = index - 2;
            if (puzzleLights[lightIndex] != null)
            {
                if (!activationOrder.Contains(lightIndex))
                {
                    puzzleLights[lightIndex].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    Debug.Log($"puzzleLights[{lightIndex}] 활성화 시도");
                    activationOrder.Add(lightIndex);
                    CheckOrder();
                }
                else
                {
                    Debug.Log($"puzzleLights[{lightIndex}]는 이미 활성화됨");
                }
            }
            else
            {
                Debug.LogError($"puzzleLights[{lightIndex}]가 null입니다!");
            }
        }
    }

    private void CheckOrder()
    {
        Debug.Log($"현재 순서: {string.Join(", ", activationOrder)}");

        if (activationOrder.Count == correctOrder.Length)
        {
            bool isCorrect = activationOrder.SequenceEqual(correctOrder);
            if (isCorrect)
            {
                Debug.Log("순서가 맞습니다! 퍼즐 완료!");
                isPuzzleComplete = true;
                // 모든 문 열기
                foreach (var door in doors)
                {
                    if (door != null)
                    {
                        door.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                        Debug.Log($"{door.name} 열림 시도");
                    }
                    else
                    {
                        Debug.LogError("doors 배열에 null 요소가 있습니다!");
                    }
                }
            }
            else
            {
                Debug.Log("순서가 틀렸습니다. 초기화합니다.");
                ResetPuzzle();
            }
        }
    }

    private void ResetPuzzle()
    {
        activationOrder.Clear();
        foreach (var light in puzzleLights)
        {
            if (light != null)
            {
                light.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver);
                Debug.Log($"{light.name} 비활성화 시도");
            }
        }
        isPuzzleComplete = false;
        Debug.Log("퍼즐이 초기화되었습니다.");
    }

    private System.Collections.IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetPuzzle();
    }
}