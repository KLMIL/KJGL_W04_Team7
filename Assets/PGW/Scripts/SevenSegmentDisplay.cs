using UnityEngine;

public class SevenSegmentDisplay : MonoBehaviour
{
    [SerializeField] private Renderer segmentA; // ��� ����
    [SerializeField] private Renderer segmentB; // ���� ��� ����
    [SerializeField] private Renderer segmentC; // ���� �ϴ� ����
    [SerializeField] private Renderer segmentD; // �ϴ� ����
    [SerializeField] private Renderer segmentE; // ���� �ϴ� ����
    [SerializeField] private Renderer segmentF; // ���� ��� ����
    [SerializeField] private Renderer segmentG; // �߾� ����

    [SerializeField] private Material onMaterial;  // ���� ���� ���͸���
    [SerializeField] private Material offMaterial; // ���� ���� ���͸���

    private int currentDigit = 0; // ���� ǥ�õ� ����
    public int CurrentDigit => currentDigit;

    // ���ں� ���׸�Ʈ ���� (true = ����, false = ����)
    private bool[][] digitPatterns = new bool[][]
    {
        new bool[] { true, true, true, true, true, true, false },  // 0
        new bool[] { false, true, true, false, false, false, false }, // 1
        new bool[] { true, true, false, true, true, false, true }, // 2
        new bool[] { true, true, true, true, false, false, true }, // 3
        new bool[] { false, true, true, false, false, true, true }, // 4
        new bool[] { true, false, true, true, false, true, true }, // 5
        new bool[] { true, false, true, true, true, true, true }, // 6
        new bool[] { true, true, true, false, false, false, false }, // 7
        new bool[] { true, true, true, true, true, true, true }, // 8
        new bool[] { true, true, true, true, false, true, true }  // 9
    };

    void Start()
    {
        // ���͸����� �����Ǿ����� Ȯ��
        if (onMaterial == null || offMaterial == null)
        {
            Debug.LogError("OnMaterial �Ǵ� OffMaterial�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �ʱ�ȭ: ��� ���׸�Ʈ ���� ���·� ���� �� 0 ǥ��
        SetAllSegments(false);
        DisplayDigit(currentDigit);
    }

    // ���� ǥ�� �޼���
    public void DisplayDigit(int digit)
    {
        if (digit < 0 || digit > 9)
        {
            Debug.LogError("���ڴ� 0-9 ���̿��� �մϴ�.");
            return;
        }

        currentDigit = digit; // ���� ���� ������Ʈ
        bool[] pattern = digitPatterns[digit];
        segmentA.material = pattern[0] ? onMaterial : offMaterial;
        segmentB.material = pattern[1] ? onMaterial : offMaterial;
        segmentC.material = pattern[2] ? onMaterial : offMaterial;
        segmentD.material = pattern[3] ? onMaterial : offMaterial;
        segmentE.material = pattern[4] ? onMaterial : offMaterial;
        segmentF.material = pattern[5] ? onMaterial : offMaterial;
        segmentG.material = pattern[6] ? onMaterial : offMaterial;

        Debug.Log($"Displaying digit: {digit}");
    }

    // ���ڸ� �ø��� �޼���
    public void IncreaseDigit()
    {
        currentDigit = (currentDigit + 1) % 10; // 9 ������ 0���� ��ȯ
        DisplayDigit(currentDigit);
    }

    // ���ڸ� ������ �޼���
    public void DecreaseDigit()
    {
        currentDigit = (currentDigit - 1 + 10) % 10; // 0 ������ 9�� ��ȯ
        DisplayDigit(currentDigit);
    }

    // ��ư �̺�Ʈ�� ó���ϴ� �޼���
    public void ButtonListen(GameObject button)
    {
        if (button == null)
        {
            Debug.LogWarning("ButtonListen: ���޵� ��ư�� null�Դϴ�.");
            return;
        }

        string buttonName = button.name;
        Debug.Log($"ButtonListen: Received button {buttonName}");

        if (buttonName == "Button_Up")
        {
            IncreaseDigit();
        }
        else if (buttonName == "Button_Down")
        {
            DecreaseDigit();
        }
        else
        {
            Debug.LogWarning($"ButtonListen: �� �� ���� ��ư �̸��Դϴ� - {buttonName}");
        }
    }

    // ��� ���׸�Ʈ ���� ����
    private void SetAllSegments(bool isOn)
    {
        Material targetMaterial = isOn ? onMaterial : offMaterial;
        segmentA.material = targetMaterial;
        segmentB.material = targetMaterial;
        segmentC.material = targetMaterial;
        segmentD.material = targetMaterial;
        segmentE.material = targetMaterial;
        segmentF.material = targetMaterial;
        segmentG.material = targetMaterial;
    }

    // �׽�Ʈ��: Inspector���� ȣ�� ����
    [ContextMenu("Test Digit 0")]
    private void TestDigit0() => DisplayDigit(0);
    [ContextMenu("Test Digit 1")]
    private void TestDigit1() => DisplayDigit(1);
    [ContextMenu("Increase Digit")]
    private void TestIncreaseDigit() => IncreaseDigit();
    [ContextMenu("Decrease Digit")]
    private void TestDecreaseDigit() => DecreaseDigit();
}