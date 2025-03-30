using UnityEngine;
using System.Collections.Generic;

public class LavaStageManager : MonoBehaviour
{
    [SerializeField] private BottomButton[] buttons; // 바닥 버튼 4개
    [SerializeField] private StairBottom[] paths;    // 길 4개
    [SerializeField] private WallButton wallButton;  // 추가: 벽 버튼
    [SerializeField] private GameObject[] doors;     // 추가: 문 2개

    private Queue<(bool pressed, GameObject buttonObject, int index)> inputQueue = new Queue<(bool pressed, GameObject, int)>();

    void Start()
    {
        // 기존 배열 크기 및 null 체크
        if (buttons.Length != 4 || paths.Length != 4)
        {
            Debug.LogError("buttons와 paths는 각각 4개여야 합니다!");
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null)
            {
                Debug.LogError($"buttons[{i}]가 null입니다!");
            }
            if (paths[i] == null)
            {
                Debug.LogError($"paths[{i}]가 null입니다!");
            }
        }

        // 추가: WallButton과 doors 체크
        if (wallButton == null)
        {
            Debug.LogError("wallButton이 null입니다!");
            return;
        }
        if (doors.Length != 2 || doors[0] == null || doors[1] == null)
        {
            Debug.LogError("doors는 2개여야 하며, null이 아니어야 합니다!");
            return;
        }

        // 버튼과 길 연결
        ConnectButtonsToPaths();
    }

    private void ConnectButtonsToPaths()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null && paths[i] != null)
            {
                int index = i; // 클로저 문제 방지
                buttons[i].onPressedStateChanged += (pressed, buttonObject) =>
                {
                    // 입력을 큐에 추가
                    inputQueue.Enqueue((pressed, buttonObject, index));
                    Debug.Log($"Button[{index}] 입력 감지: {pressed}, 큐에 추가됨");
                };
            }
        }
    }

    void FixedUpdate()
    {
        // 큐에 쌓인 입력을 순차적으로 처리
        while (inputQueue.Count > 0)
        {
            var (pressed, buttonObject, index) = inputQueue.Dequeue();
            if (paths[index] != null)
            {
                paths[index].HandleButtonState(pressed, buttonObject);
                Debug.Log($"Button[{index}] 상태 변경: {pressed}, 연결된 Path[{index}] 동작");
            }
        }
    }

    public void ButtonListen(GameObject button)
    {
        // WallButton 처리
        if (button == wallButton.gameObject)
        {
            foreach (var door in doors)
            {
                if (door != null)
                {
                    door.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    Debug.Log($"{door.name} 열림");
                }
            }
        }
    }
}