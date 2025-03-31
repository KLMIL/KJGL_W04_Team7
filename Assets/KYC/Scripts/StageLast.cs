using UnityEngine;

public class StageLast : MonoBehaviour
{
    public CubeArrayController cubeController;
    [SerializeField] private GameObject endBlock;
    [SerializeField] private GameObject endDoor;
    [SerializeField] private GameObject endBlock2;
    public bool hasPlayer1Reached = false; // player1이 도달했는지
    public bool hasPlayer2Reached = false; // player2가 도달했는지
    [SerializeField] private GameObject endDoor2;
     public GameObject player1; // 첫 번째 플레이어 (Inspector에서 설정)
     public GameObject player2;
    public bool gameComplete = false;
    [SerializeField] private GameObject[] buttons;
    [SerializeField]
    private int[][] cubeIndicesToTurnOn = new int[][]
    {
        new int[] { },                      // 버튼 0: 리셋
        new int[] { 3, 7, 11, 15, 19, 23 }, // 버튼 1
        new int[] { 0, 4, 8, 12, 16, 20 },  // 버튼 2
        new int[] { 2, 5, 6, 9, 8, 11, 12, 15, 14, 17, 18, 21 }, // 버튼 3
        new int[] { 7, 6, 3, 15, 17, 21 },  // 버튼 4
        new int[] { 1, 2, 3, 9, 16, 23 },   // 버튼 5
        new int[] { 24 },                   // 버튼 6
        new int[] { 25 },                   // 버튼 7
        new int[] { 26 },                    // 버튼 8
        new int[] { 44,48,49,50,47 },                    // 버튼 9
        new int[] { 48,49,45,38,28 },                    // 버튼 10
        new int[] { 29,33,32,36,34,35,37,41,42,39,45,44,47 },                    // 버튼 11
        new int[] { 34,35,39,37,44,48,46,49 },                    // 버튼 12
        new int[] { 34,35,37,39,41,42,45 },                    // 버튼 13
        new int[] { 37,38,42,43,39,36,33,29 },                      // 버튼 14
        new int[] {}
    };
    private bool hasPlayedEndScene = false; // 엔딩 씬 재생 여부
    void Start()
    {
        if (cubeController == null)
        {
            cubeController = FindFirstObjectByType<CubeArrayController>();
            if (cubeController == null)
                Debug.LogError("CubeArrayController가 설정되지 않았습니다.");
        }
        player1 = GameObject.Find("Player1"); // "Player1"이라는 이름의 오브젝트
        player2 = GameObject.Find("Player2"); // "Player2"라는 이름의 오브젝트
        if (buttons == null || buttons.Length != 16)
        {
            Debug.LogError("buttons는 9개여야 합니다!");
        }
        else if (buttons.Length != cubeIndicesToTurnOn.Length)
        {
            Debug.LogError("buttons와 cubeIndicesToTurnOn의 길이가 일치하지 않습니다!");
        }
    }

    public void ButtonListen(GameObject button)
    {
        if (cubeController == null || buttons == null) return;

        int buttonIndex = System.Array.IndexOf(buttons, button);
        if (buttonIndex != -1 && buttonIndex < cubeIndicesToTurnOn.Length)
        {
            if (buttonIndex == 0)
            {
                ResetFirstPuzzleCubes();
                Debug.Log("첫 번째 버튼이 눌려 모든 큐브가 꺼졌습니다.");
            }
            else if (buttonIndex == 15)
            {
                ResetLastPuzzleCubes();
                Debug.Log("첫 번째 버튼이 눌려 모든 큐브가 꺼졌습니다.");
            }
            else
            {
                int[] cubesToTurnOn = cubeIndicesToTurnOn[buttonIndex];
                cubeController.ToggleCubes(cubesToTurnOn);
                Debug.Log($"버튼 {buttonIndex} ({buttons[buttonIndex].name})이 눌려 {cubesToTurnOn.Length}개의 큐브 상태가 토글되었습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"알 수 없는 버튼: {button.name}");
        }
    }

    private bool CheckGameEndCondition()
    {
        // 각 퍼즐의 클리어 여부를 저장하는 변수
        bool firstPuzzleCleared = true;
        bool middlePuzzleCleared = true;
        bool lastPuzzleCleared = true;

        // 첫 번째 퍼즐: 0~23번 큐브 체크
        int[] requiredTrueCubes = new int[] { 0, 2, 3, 4, 5, 6, 7, 9, 14, 16, 17, 18, 19, 20, 21, 23 };
        for (int i = 0; i < 24; i++)
        {
            bool shouldBeTrue = System.Array.IndexOf(requiredTrueCubes, i) != -1;
            if (cubeController.cubes[i].isMaterial1 != shouldBeTrue)
            {
                firstPuzzleCleared = false;
                break;
            }
        }

        // 중간 퍼즐: 24~26번 큐브 체크
        if (cubeController.cubes[24].specialMaterialState != 1 || // material3
            cubeController.cubes[25].specialMaterialState != 2 || // material4
            cubeController.cubes[26].specialMaterialState != 3)   // material5
        {
            middlePuzzleCleared = false;
        }

        // 마지막 퍼즐: 27~50번 큐브 체크
        int[] state1Cubes = new int[] { 29, 32, 33, 36 };
        int[] state3Cubes = new int[] { 34, 35, 37, 39, 41, 42, 45 };
        int[] state4Cubes = new int[] { 48, 49, 50 };
        for (int i = 27; i <= 50; i++)
        {
            int expectedState = 0; // 기본은 0
            if (System.Array.IndexOf(state1Cubes, i) != -1) expectedState = 1;
            else if (System.Array.IndexOf(state3Cubes, i) != -1) expectedState = 3;
            else if (System.Array.IndexOf(state4Cubes, i) != -1) expectedState = 4;

            if (cubeController.cubes[i].specialMaterialState != expectedState)
            {
                lastPuzzleCleared = false;
                break;
            }
        }

        // 첫 번째 퍼즐 클리어 시 버튼 0~5 비활성화
        if (firstPuzzleCleared)
        {
            for (int i = 0; i <= 5; i++)
            {
                if (buttons[i] != null)
                {
                    buttons[i].SetActive(false); // 버튼 오브젝트 비활성화
                }
            }
            Debug.Log("첫 번째 퍼즐 클리어! 버튼 0~5 비활성화");
        }

        // 중간 퍼즐 클리어 시 버튼 6~8 비활성화
        if (middlePuzzleCleared)
        {
            for (int i = 6; i <= 8; i++)
            {
                if (buttons[i] != null)
                {
                    buttons[i].SetActive(false); // 버튼 오브젝트 비활성화
                }
            }
            Debug.Log("중간 퍼즐 클리어! 버튼 6~8 비활성화");
        }

        // 마지막 퍼즐 클리어 시 버튼 9~15 비활성화
        if (lastPuzzleCleared)
        {
            for (int i = 9; i <= 15; i++)
            {
                if (buttons[i] != null)
                {
                    buttons[i].SetActive(false); // 버튼 오브젝트 비활성화
                }
            }
            Debug.Log("마지막 퍼즐 클리어! 버튼 9~15 비활성화");
        }

        // 모든 퍼즐이 클리어된 경우
        if (firstPuzzleCleared && middlePuzzleCleared && lastPuzzleCleared)
        {
            // 전체 게임이 완료되었을 때 추가적으로 버튼 비활성화
            // 예: 모든 버튼(0~15)을 비활성화
            for (int i = 0; i <= 15; i++)
            {
                if (buttons[i] != null)
                {
                    buttons[i].SetActive(false); // 버튼 오브젝트 비활성화
                }
            }
            Debug.Log("모든 퍼즐 클리어! 모든 버튼(0~15) 비활성화");

            Debug.Log("모든 조건 만족했어! 게임 끝날 준비 됐어~");
            return true;
        }

        return false;
    }
    private void ResetFirstPuzzleCubes()
    {
        cubeController.ResetFirstPuzzleToFalse();
    }
    private void ResetLastPuzzleCubes()
    {
        cubeController.ResetLastPuzzleToFalse();
    }


    public void Update()
    {
        if (CheckGameEndCondition())
        {
            gameComplete = true;
            PlayEndScene();
        }
        if (hasPlayer1Reached && hasPlayer2Reached)
        {
            UIManager.Instance.ShowSuccessScreen();
            Time.timeScale = 0;
        }
    }
    private void PlayEndScene()
    {
        if (gameComplete && !hasPlayedEndScene)
        {
            hasPlayedEndScene = true;
            Debug.Log("게임 끝! 엔딩 씬 재생");

            // endBlock과 endDoor의 머터리얼을 material5로 변경
            if (endBlock != null)
            {
                Renderer blockRenderer = endBlock.GetComponent<Renderer>();
                if (blockRenderer != null && cubeController.material7 != null)
                {
                    blockRenderer.material = cubeController.material7;
                    Debug.Log("endBlock 머터리얼을 material5로 바꿨어!");
                }
                else
                {
                    Debug.LogError("endBlock에 Renderer가 없거나 material5가 설정되지 않았어!");
                }
            }
            else
            {
                Debug.LogError("endBlock이 설정되지 않았어!");
            }

            if (endDoor != null)
            {
                Renderer doorRenderer = endDoor.GetComponent<Renderer>();
                if (doorRenderer != null && cubeController.material7 != null)
                {
                    doorRenderer.material = cubeController.material7;
                    Debug.Log("endDoor 머터리얼을 material5로 바꿨어!");
                }
                else
                {
                    Debug.LogError("endDoor에 Renderer가 없거나 material5가 설정되지 않았어!");
                }
            }
            if(endDoor2 != null)
            {
                Renderer doorRenderer = endDoor2.GetComponent<Renderer>();
                if (doorRenderer != null && cubeController.material7 != null)
                {
                    doorRenderer.material = cubeController.material7;
                    Debug.Log("endDoor2 머터리얼을 material5로 바꿨어!");
                }
                else
                {
                    Debug.LogError("endDoor2에 Renderer가 없거나 material5가 설정되지 않았어!");
                }
            }
            if (endBlock2 != null)
            {
                Renderer doorRenderer = endBlock2.GetComponent<Renderer>();
                if (doorRenderer != null && cubeController.material7 != null)
                {
                    doorRenderer.material = cubeController.material7;
                    Debug.Log("endDoor2 머터리얼을 material5로 바꿨어!");
                }
                else
                {
                    Debug.LogError("endDoor2에 Renderer가 없거나 material5가 설정되지 않았어!");
                }
            }
            else
            {
                Debug.LogError("endDoor가 설정되지 않았어!");
            }
        }
    }
}