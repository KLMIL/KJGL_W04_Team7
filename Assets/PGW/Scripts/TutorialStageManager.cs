using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TutorialStageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] lights;
    [SerializeField] private GameObject door;

    private List<int> activationOrder; // ���� Ȱ��ȭ ������ ���
    private int[] correctOrder = { 2, 1, 0, 3 }; // �ùٸ� ���� (����, �ʿ信 ���� ����)
    private bool isPuzzleComplete = false; // ���� �Ϸ� ����

    void Start()
    {
        activationOrder = new List<int>(); // ���� ����Ʈ �ʱ�ȭ
    }

    public void ButtonListen(GameObject button)
    {
        if (isPuzzleComplete) return; // ���� �Ϸ� �� �� �̻� �۵����� ����

        // buttons �迭���� button�� �ε��� ã��
        int index = System.Array.IndexOf(buttons, button);

        // �ε����� ��ȿ�ϰ� lights �迭�� �ش� �ε����� �����ϸ� ó��
        if (index != -1 && index < lights.Length && lights[index] != null)
        {
            // �̹� Ȱ��ȭ�� ��� �ߺ� üũ ����
            if (!activationOrder.Contains(index))
            {
                lights[index].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                activationOrder.Add(index); // ������ �߰�
                CheckOrder(); // ���� Ȯ��
            }
        }
    }

    // ������ �´��� Ȯ��
    private void CheckOrder()
    {
        Debug.Log($"���� ����: {string.Join(", ", activationOrder)}");

        // ��� ��ư�� ���ȴ��� Ȯ��
        if (activationOrder.Count == correctOrder.Length)
        {
            bool isCorrect = activationOrder.SequenceEqual(correctOrder);
            if (isCorrect)
            {
                Debug.Log("������ �½��ϴ�! ���� �Ϸ�!");
                isPuzzleComplete = true;

                // ��� �� ���� (������)
                if (door != null)
                {
                    door.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                }

                // 3�� �� ���� (������)
                StartCoroutine(ResetAfterDelay(3f));
            }
            else
            {
                Debug.Log("������ Ʋ�Ƚ��ϴ�. �ʱ�ȭ�մϴ�.");
                ResetPuzzle();
            }
        }
    }

    // ���� �ʱ�ȭ
    private void ResetPuzzle()
    {
        activationOrder.Clear();
        foreach (var light in lights)
        {
            if (light != null)
            {
                light.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver); // ��Ȱ��ȭ �޼ҵ� �ʿ�
            }
        }
        isPuzzleComplete = false;
        Debug.Log("������ �ʱ�ȭ�Ǿ����ϴ�.");
    }

    // ������ �ð� �� �����ϴ� �ڷ�ƾ
    private System.Collections.IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetPuzzle();
    }
}
