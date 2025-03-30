using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class ThreeDigitDisplay : MonoBehaviour
{
    [SerializeField] private SevenSegmentDisplay displayHundreds; // 100의 자리
    [SerializeField] private SevenSegmentDisplay displayTens;     // 10의 자리
    [SerializeField] private SevenSegmentDisplay displayUnits;    // 1의 자리

    [SerializeField] private Renderer uObject;  // U 오브젝트
    [SerializeField] private Renderer pObject;  // P 오브젝트
    [SerializeField] private Renderer dObject;  // D 오브젝트
    [SerializeField] private Renderer oObject;  // O 오브젝트
    [SerializeField] private Renderer wObject;  // W 오브젝트
    [SerializeField] private Renderer nObject;  // N 오브젝트

    [SerializeField] private Material upOnMaterial;   // UP 상태 켜짐 매터리얼
    [SerializeField] private Material downOnMaterial; // DOWN 상태 켜짐 매터리얼
    [SerializeField] private Material offMaterial;    // 꺼짐 상태 매터리얼

    [SerializeField] private GameObject[] doors;

    private const int TARGET_NUMBER = 516; // 비교 대상 숫자

    void Start()
    {
        // 전광판 및 오브젝트가 모두 설정되었는지 확인
        if (displayHundreds == null || displayTens == null || displayUnits == null)
        {
            Debug.LogError("ThreeDigitDisplay: 하나 이상의 SevenSegmentDisplay가 설정되지 않았습니다.");
            return;
        }
        if (uObject == null || pObject == null || dObject == null || oObject == null || wObject == null || nObject == null)
        {
            Debug.LogError("ThreeDigitDisplay: 하나 이상의 문자 오브젝트가 설정되지 않았습니다.");
            return;
        }
        if (upOnMaterial == null || downOnMaterial == null || offMaterial == null)
        {
            Debug.LogError("ThreeDigitDisplay: UpOnMaterial, DownOnMaterial, 또는 OffMaterial이 설정되지 않았습니다.");
            return;
        }

        // 초기 비교 수행
        CompareWithTarget();
    }

    // 버튼 이벤트를 받아 각 전광판에 전달
    public void ButtonListen(GameObject button)
    {
        if (button == null) return;

        string buttonName = button.name;
        Debug.Log($"ThreeDigitDisplay: Button {buttonName} pressed");

        // 버튼 이름에 따라 특정 전광판 제어
        if (buttonName.Contains("Hundreds"))
        {
            Debug.Log("Hundreds button pressed");
            if (buttonName.Contains("Up")) displayHundreds.IncreaseDigit();
            else if (buttonName.Contains("Down")) displayHundreds.DecreaseDigit();
        }
        else if (buttonName.Contains("Tens"))
        {
            if (buttonName.Contains("Up")) displayTens.IncreaseDigit();
            else if (buttonName.Contains("Down")) displayTens.DecreaseDigit();
        }
        else if (buttonName.Contains("Units"))
        {
            if (buttonName.Contains("Up")) displayUnits.IncreaseDigit();
            else if (buttonName.Contains("Down")) displayUnits.DecreaseDigit();
        }

        // 버튼 처리 후 비교 수행
        CompareWithTarget();
    }

    // 현재 3자리 숫자를 계산하고 516과 비교
    private void CompareWithTarget()
    {
        int totalNumber = (displayHundreds.CurrentDigit * 100) +
                         (displayTens.CurrentDigit * 10) +
                         displayUnits.CurrentDigit;

        Debug.Log($"Current number: {totalNumber}");

        if (totalNumber < TARGET_NUMBER)
        {
            Debug.Log($"Number {totalNumber} is Above or Equal to {TARGET_NUMBER}");
            SetAboveObjects(true);  // U, P 켜기 (upOnMaterial)
            SetBelowObjects(false); // D, O, W, N 끄기
        }
        else if(totalNumber > TARGET_NUMBER)
        {
            Debug.Log($"Number {totalNumber} is Below {TARGET_NUMBER}");
            SetAboveObjects(false); // U, P 끄기
            SetBelowObjects(true);  // D, O, W, N 켜기 (downOnMaterial)
        }
        else
        {
            SetAboveObjects(true); // U, P 끄기
            SetBelowObjects(true);  // D, O, W, N 켜기 (downOnMaterial)
            foreach (var door in doors)
            {
                if (door != null)
                {
                    door.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    Debug.Log($"{door.name} 열림 시도");
                }
                else
                {
                    Debug.LogError("doors 배열에 null 요소가 있습니다!");
                }
            }
        }
    }

    // U, P 오브젝트 상태 설정
    private void SetAboveObjects(bool isOn)
    {
        Material targetMaterial = isOn ? upOnMaterial : offMaterial;
        uObject.material = targetMaterial;
        pObject.material = targetMaterial;
    }

    // D, O, W, N 오브젝트 상태 설정
    private void SetBelowObjects(bool isOn)
    {
        Material targetMaterial = isOn ? downOnMaterial : offMaterial;
        dObject.material = targetMaterial;
        oObject.material = targetMaterial;
        wObject.material = targetMaterial;
        nObject.material = targetMaterial;
    }

    // 테스트용
    [ContextMenu("Compare Now")]
    private void TestCompare() => CompareWithTarget();
}