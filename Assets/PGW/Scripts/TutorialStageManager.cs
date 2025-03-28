using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class TutorialStageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons; // �� 6�� ��ư (0, 1: �� �̵�, 2~5: ����)
    [SerializeField] private PuzzleLight[] puzzleLights; // ����� ����Ʈ 4�� (�ε��� 2~5�� ����)
    [SerializeField] private GameObject[] doors; // ���� �Ϸ� �� ���� �� (ũ�� 2)
    [SerializeField] private GameObject[] walls; // ��ư 0, 1�� ������ �� (ũ�� 2)

    private List<int> activationOrder;
    private int[] correctOrder = { 2, 1, 0, 3 }; // ���� ����
    private bool isPuzzleComplete = false;

    void Start()
    {
        activationOrder = new List<int>();
        if (buttons.Length != 6 || puzzleLights.Length != 4 || walls.Length != 2 || doors.Length != 2)
        {
            Debug.LogError("buttons�� 6��, puzzleLights�� 4��, walls�� 2��, doors�� 2������ �մϴ�!");
        }
        // puzzleLights �ʱ�ȭ Ȯ��
        for (int i = 0; i < puzzleLights.Length; i++)
        {
            if (puzzleLights[i] == null)
            {
                Debug.LogError($"puzzleLights[{i}]�� null�Դϴ�!");
            }
        }
        // doors �ʱ�ȭ Ȯ��
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i] == null)
            {
                Debug.LogError($"doors[{i}]�� null�Դϴ�!");
            }
        }
    }

    public void ButtonListen(GameObject button)
    {
        if (isPuzzleComplete) return;

        int index = System.Array.IndexOf(buttons, button);
        Debug.Log($"Button Index: {index}");

        if (index == -1) return;

        // �ε��� 0, 1: �� ����
        if (index == 0)
        {
            if (walls[0] != null)
            {
                walls[0].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Wall 1 �̵� �õ�");
            }
            else
            {
                Debug.LogError("walls[0]�� null�Դϴ�!");
            }
        }
        else if (index == 1)
        {
            if (walls[1] != null)
            {
                walls[1].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Wall 2 �̵� �õ�");
            }
            else
            {
                Debug.LogError("walls[1]�� null�Դϴ�!");
            }
        }
        // �ε��� 2~5: ���� ���� üũ
        else if (index >= 2 && index < buttons.Length && index - 2 < puzzleLights.Length)
        {
            int lightIndex = index - 2;
            if (puzzleLights[lightIndex] != null)
            {
                if (!activationOrder.Contains(lightIndex))
                {
                    puzzleLights[lightIndex].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    Debug.Log($"puzzleLights[{lightIndex}] Ȱ��ȭ �õ�");
                    activationOrder.Add(lightIndex);
                    CheckOrder();
                }
                else
                {
                    Debug.Log($"puzzleLights[{lightIndex}]�� �̹� Ȱ��ȭ��");
                }
            }
            else
            {
                Debug.LogError($"puzzleLights[{lightIndex}]�� null�Դϴ�!");
            }
        }
    }

    private void CheckOrder()
    {
        Debug.Log($"���� ����: {string.Join(", ", activationOrder)}");

        if (activationOrder.Count == correctOrder.Length)
        {
            bool isCorrect = activationOrder.SequenceEqual(correctOrder);
            if (isCorrect)
            {
                Debug.Log("������ �½��ϴ�! ���� �Ϸ�!");
                isPuzzleComplete = true;
                // ��� �� ����
                foreach (var door in doors)
                {
                    if (door != null)
                    {
                        door.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                        Debug.Log($"{door.name} ���� �õ�");
                    }
                    else
                    {
                        Debug.LogError("doors �迭�� null ��Ұ� �ֽ��ϴ�!");
                    }
                }
            }
            else
            {
                Debug.Log("������ Ʋ�Ƚ��ϴ�. �ʱ�ȭ�մϴ�.");
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
                Debug.Log($"{light.name} ��Ȱ��ȭ �õ�");
            }
        }
        isPuzzleComplete = false;
        Debug.Log("������ �ʱ�ȭ�Ǿ����ϴ�.");
    }

    private System.Collections.IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetPuzzle();
    }
}