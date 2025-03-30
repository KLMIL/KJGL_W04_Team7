using UnityEngine;

public class SevenSegmentDisplay : MonoBehaviour
{
    [SerializeField] private Renderer segmentA; // 상단 가로
    [SerializeField] private Renderer segmentB; // 우측 상단 세로
    [SerializeField] private Renderer segmentC; // 우측 하단 세로
    [SerializeField] private Renderer segmentD; // 하단 가로
    [SerializeField] private Renderer segmentE; // 좌측 하단 세로
    [SerializeField] private Renderer segmentF; // 좌측 상단 세로
    [SerializeField] private Renderer segmentG; // 중앙 가로

    [SerializeField] private Material onMaterial;  // 켜짐 상태 매터리얼
    [SerializeField] private Material offMaterial; // 꺼짐 상태 매터리얼

    private int currentDigit = 0; // 현재 표시된 숫자
    public int CurrentDigit => currentDigit;

    // 숫자별 세그먼트 패턴 (true = 켜짐, false = 꺼짐)
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
        // 매터리얼이 설정되었는지 확인
        if (onMaterial == null || offMaterial == null)
        {
            Debug.LogError("OnMaterial 또는 OffMaterial이 설정되지 않았습니다.");
            return;
        }

        // 초기화: 모든 세그먼트 꺼짐 상태로 설정 후 0 표시
        SetAllSegments(false);
        DisplayDigit(currentDigit);
    }

    // 숫자 표시 메서드
    public void DisplayDigit(int digit)
    {
        if (digit < 0 || digit > 9)
        {
            Debug.LogError("숫자는 0-9 사이여야 합니다.");
            return;
        }

        currentDigit = digit; // 현재 숫자 업데이트
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

    // 숫자를 올리는 메서드
    public void IncreaseDigit()
    {
        currentDigit = (currentDigit + 1) % 10; // 9 다음은 0으로 순환
        DisplayDigit(currentDigit);
    }

    // 숫자를 내리는 메서드
    public void DecreaseDigit()
    {
        currentDigit = (currentDigit - 1 + 10) % 10; // 0 이전은 9로 순환
        DisplayDigit(currentDigit);
    }

    // 버튼 이벤트를 처리하는 메서드
    public void ButtonListen(GameObject button)
    {
        if (button == null)
        {
            Debug.LogWarning("ButtonListen: 전달된 버튼이 null입니다.");
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
            Debug.LogWarning($"ButtonListen: 알 수 없는 버튼 이름입니다 - {buttonName}");
        }
    }

    // 모든 세그먼트 상태 설정
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

    // 테스트용: Inspector에서 호출 가능
    [ContextMenu("Test Digit 0")]
    private void TestDigit0() => DisplayDigit(0);
    [ContextMenu("Test Digit 1")]
    private void TestDigit1() => DisplayDigit(1);
    [ContextMenu("Increase Digit")]
    private void TestIncreaseDigit() => IncreaseDigit();
    [ContextMenu("Decrease Digit")]
    private void TestDecreaseDigit() => DecreaseDigit();
}