using UnityEngine;

public class TutorialStageManager : MonoBehaviour
{

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] lights;
    [SerializeField] private GameObject[] doors;

    public void ButtonListen(GameObject button)
    {
        // buttons �迭���� button�� �ε��� ã��
        int index = System.Array.IndexOf(buttons, button);

        // �ε����� ��ȿ�ϰ� lights �迭�� �ش� �ε����� �����ϸ� Activate ȣ��
        if (index != -1 && index < lights.Length && lights[index] != null)
        {
            // Light ��ü���� MovingWall ��ũ��Ʈ�� Activate ȣ��
            lights[index].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
        }
    }
}
