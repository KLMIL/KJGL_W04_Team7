using UnityEngine;
using System.Collections.Generic;

public class LavaStageManager : MonoBehaviour
{
    [SerializeField] private BottomButton[] buttons; // �ٴ� ��ư 4��
    [SerializeField] private StairBottom[] paths;    // �� 4��
    [SerializeField] private WallButton wallButton;  // �߰�: �� ��ư
    [SerializeField] private GameObject[] doors;     // �߰�: �� 2��

    private Queue<(bool pressed, GameObject buttonObject, int index)> inputQueue = new Queue<(bool pressed, GameObject, int)>();

    void Start()
    {
        // ���� �迭 ũ�� �� null üũ
        if (buttons.Length != 4 || paths.Length != 4)
        {
            Debug.LogError("buttons�� paths�� ���� 4������ �մϴ�!");
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null)
            {
                Debug.LogError($"buttons[{i}]�� null�Դϴ�!");
            }
            if (paths[i] == null)
            {
                Debug.LogError($"paths[{i}]�� null�Դϴ�!");
            }
        }

        // �߰�: WallButton�� doors üũ
        if (wallButton == null)
        {
            Debug.LogError("wallButton�� null�Դϴ�!");
            return;
        }
        if (doors.Length != 2 || doors[0] == null || doors[1] == null)
        {
            Debug.LogError("doors�� 2������ �ϸ�, null�� �ƴϾ�� �մϴ�!");
            return;
        }

        // ��ư�� �� ����
        ConnectButtonsToPaths();
    }

    private void ConnectButtonsToPaths()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null && paths[i] != null)
            {
                int index = i; // Ŭ���� ���� ����
                buttons[i].onPressedStateChanged += (pressed, buttonObject) =>
                {
                    // �Է��� ť�� �߰�
                    inputQueue.Enqueue((pressed, buttonObject, index));
                    Debug.Log($"Button[{index}] �Է� ����: {pressed}, ť�� �߰���");
                };
            }
        }
    }

    void FixedUpdate()
    {
        // ť�� ���� �Է��� ���������� ó��
        while (inputQueue.Count > 0)
        {
            var (pressed, buttonObject, index) = inputQueue.Dequeue();
            if (paths[index] != null)
            {
                paths[index].HandleButtonState(pressed, buttonObject);
                Debug.Log($"Button[{index}] ���� ����: {pressed}, ����� Path[{index}] ����");
            }
        }
    }

    public void ButtonListen(GameObject button)
    {
        // WallButton ó��
        if (button == wallButton.gameObject)
        {
            foreach (var door in doors)
            {
                if (door != null)
                {
                    door.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    Debug.Log($"{door.name} ����");
                }
            }
        }
    }
}