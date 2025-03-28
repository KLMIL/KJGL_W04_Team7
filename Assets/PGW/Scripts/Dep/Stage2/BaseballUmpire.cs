using UnityEngine;

public class BaseballUmpire : MonoBehaviour
{
    private BaseballPuzzle puzzle; // BaseballPuzzle ��ũ��Ʈ ����
    private int[] targetNumbers = { 6, 3, 7 }; // ���� ���� �迭
    private int currentIndex = 0; // ���� ���� ������ �ε���
    private int activateCallCount = 0; // Activate ȣ�� Ƚ��

    void Start()
    {
        // BaseballPuzzle ������Ʈ�� ������
        puzzle = GetComponent<BaseballPuzzle>();
        if (puzzle == null)
        {
            Debug.LogError("BaseballPuzzle ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    // Activate �޼���: ��ư �Է�(0~9)�� �޾� ī��Ʈ�� ������Ʈ
    public void Activate(int inputNumber)
    {
        // �Է� ��ȿ�� �˻�
        if (inputNumber < 0 || inputNumber > 9)
        {
            Debug.LogError("�Է� ���ڴ� 0~9 ���̿��� �մϴ�.");
            return;
        }

        // ���� �� ��� ����
        int currentTarget = targetNumbers[currentIndex];

        if (inputNumber == currentTarget)
        {
            // �Է��� ���� ���� ������ �׸� ī��Ʈ ����
            puzzle.AddGreenCount();
            Debug.Log($"�Է�: {inputNumber}, ���: {currentTarget} �� Green Count ����");
        }
        else if (inputNumber == targetNumbers[0] || inputNumber == targetNumbers[1] || inputNumber == targetNumbers[2])
        {
            // �Է��� ���� ���� �ٸ��� �ٸ� �� ���� �� �ϳ��� ������ ���ο� ī��Ʈ ����
            puzzle.AddYellowCount();
            Debug.Log($"�Է�: {inputNumber}, ���: {currentTarget} �� Yellow Count ����");
        }
        else if (currentIndex == 2 && puzzle.GetGreenCount() == 0 && puzzle.GetYellowCount() == 0)
        {
            // �ε����� 2�̰� �׸�/���ο� ī��Ʈ�� 0�� ���� ���� ī��Ʈ ����
            puzzle.AddRedCount();
            Debug.Log($"�Է�: {inputNumber}, ���: {currentTarget} �� Red Count ���� (�ε��� 2, �׸�/���ο� 0)");
        }
        else
        {
            Debug.Log($"�Է�: {inputNumber}, ���: {currentTarget} �� ���ǿ� ���� ����, ī��Ʈ ��ȭ ����");
        }

        // ���� �� ������� �̵� (0, 1, 2 ��ȯ)
        currentIndex = (currentIndex + 1) % 3;

        // ȣ�� Ƚ�� ���� �� 3������ TurnOnLights ȣ��
        activateCallCount++;
        if (activateCallCount >= 3)
        {
            puzzle.TurnOnLights();
            activateCallCount = 0; // ȣ�� Ƚ�� �ʱ�ȭ
            Debug.Log("Activate�� 3�� ȣ��Ǿ� TurnOnLights�� �����߽��ϴ�.");
        }
    }
}