using UnityEngine;

public class TutorialStageManager : MonoBehaviour
{

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] lights;
    [SerializeField] private GameObject[] doors;

    public void ButtonListen(GameObject button)
    {
        // buttons 배열에서 button의 인덱스 찾기
        int index = System.Array.IndexOf(buttons, button);

        // 인덱스가 유효하고 lights 배열에 해당 인덱스가 존재하면 Activate 호출
        if (index != -1 && index < lights.Length && lights[index] != null)
        {
            // Light 객체에서 MovingWall 스크립트의 Activate 호출
            lights[index].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
        }
    }
}
