using UnityEngine;

public class StageLast : MonoBehaviour
{
    public CubeArrayController cubeController;
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
        new int[] { 26 }                    // 버튼 8
    };

    void Start()
    {
        if (cubeController == null)
        {
            cubeController = FindFirstObjectByType<CubeArrayController>();
            if (cubeController == null)
                Debug.LogError("CubeArrayController가 설정되지 않았습니다.");
        }
        if (buttons == null || buttons.Length != 9)
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
        if (buttonIndex != -1 && buttonIndex < cubeIndicesToTurnOn.Length) // 0~8까지 처리
        {
            if (buttonIndex == 0)
            {
                ResetAllCubes();
                Debug.Log("첫 번째 버튼이 눌려 모든 큐브가 꺼졌습니다.");
            }
            else
            {
                int[] cubesToTurnOn = cubeIndicesToTurnOn[buttonIndex];
                if (cubesToTurnOn.Length == 1 && cubesToTurnOn[0] >= 24 && cubesToTurnOn[0] <= 26)
                {
                    // 특수 큐브(24~26) 처리
                    cubeController.ToggleSpecialCube(cubesToTurnOn[0]);
                    Debug.Log($"버튼 {buttonIndex} ({buttons[buttonIndex].name})이 눌려 특수 큐브 {cubesToTurnOn[0]} 토글");
                }
                else
                {
                    // 일반 큐브(0~23) 처리
                    cubeController.SetCubeState(cubesToTurnOn);
                    Debug.Log($"버튼 {buttonIndex} ({buttons[buttonIndex].name})이 눌려 {cubesToTurnOn.Length}개의 큐브 상태가 토글되었습니다.");
                }
            }
        }
        else
        {
            Debug.LogWarning($"알 수 없는 버튼: {button.name}");
        }
    }

    private void ResetAllCubes()
    {
        cubeController.ResetAllToFalse();
    }
}