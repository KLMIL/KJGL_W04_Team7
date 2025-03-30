using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class ThreeDigitDisplay : MonoBehaviour
{
    [SerializeField] private SevenSegmentDisplay displayHundreds; // 100�� �ڸ�
    [SerializeField] private SevenSegmentDisplay displayTens;     // 10�� �ڸ�
    [SerializeField] private SevenSegmentDisplay displayUnits;    // 1�� �ڸ�

    [SerializeField] private Renderer uObject;  // U ������Ʈ
    [SerializeField] private Renderer pObject;  // P ������Ʈ
    [SerializeField] private Renderer dObject;  // D ������Ʈ
    [SerializeField] private Renderer oObject;  // O ������Ʈ
    [SerializeField] private Renderer wObject;  // W ������Ʈ
    [SerializeField] private Renderer nObject;  // N ������Ʈ

    [SerializeField] private Material upOnMaterial;   // UP ���� ���� ���͸���
    [SerializeField] private Material downOnMaterial; // DOWN ���� ���� ���͸���
    [SerializeField] private Material offMaterial;    // ���� ���� ���͸���

    [SerializeField] private GameObject[] doors;

    private const int TARGET_NUMBER = 516; // �� ��� ����

    void Start()
    {
        // ������ �� ������Ʈ�� ��� �����Ǿ����� Ȯ��
        if (displayHundreds == null || displayTens == null || displayUnits == null)
        {
            Debug.LogError("ThreeDigitDisplay: �ϳ� �̻��� SevenSegmentDisplay�� �������� �ʾҽ��ϴ�.");
            return;
        }
        if (uObject == null || pObject == null || dObject == null || oObject == null || wObject == null || nObject == null)
        {
            Debug.LogError("ThreeDigitDisplay: �ϳ� �̻��� ���� ������Ʈ�� �������� �ʾҽ��ϴ�.");
            return;
        }
        if (upOnMaterial == null || downOnMaterial == null || offMaterial == null)
        {
            Debug.LogError("ThreeDigitDisplay: UpOnMaterial, DownOnMaterial, �Ǵ� OffMaterial�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �ʱ� �� ����
        CompareWithTarget();
    }

    // ��ư �̺�Ʈ�� �޾� �� �����ǿ� ����
    public void ButtonListen(GameObject button)
    {
        if (button == null) return;

        string buttonName = button.name;
        Debug.Log($"ThreeDigitDisplay: Button {buttonName} pressed");

        // ��ư �̸��� ���� Ư�� ������ ����
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

        // ��ư ó�� �� �� ����
        CompareWithTarget();
    }

    // ���� 3�ڸ� ���ڸ� ����ϰ� 516�� ��
    private void CompareWithTarget()
    {
        int totalNumber = (displayHundreds.CurrentDigit * 100) +
                         (displayTens.CurrentDigit * 10) +
                         displayUnits.CurrentDigit;

        Debug.Log($"Current number: {totalNumber}");

        if (totalNumber < TARGET_NUMBER)
        {
            Debug.Log($"Number {totalNumber} is Above or Equal to {TARGET_NUMBER}");
            SetAboveObjects(true);  // U, P �ѱ� (upOnMaterial)
            SetBelowObjects(false); // D, O, W, N ����
        }
        else if(totalNumber > TARGET_NUMBER)
        {
            Debug.Log($"Number {totalNumber} is Below {TARGET_NUMBER}");
            SetAboveObjects(false); // U, P ����
            SetBelowObjects(true);  // D, O, W, N �ѱ� (downOnMaterial)
        }
        else
        {
            SetAboveObjects(true); // U, P ����
            SetBelowObjects(true);  // D, O, W, N �ѱ� (downOnMaterial)
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
    }

    // U, P ������Ʈ ���� ����
    private void SetAboveObjects(bool isOn)
    {
        Material targetMaterial = isOn ? upOnMaterial : offMaterial;
        uObject.material = targetMaterial;
        pObject.material = targetMaterial;
    }

    // D, O, W, N ������Ʈ ���� ����
    private void SetBelowObjects(bool isOn)
    {
        Material targetMaterial = isOn ? downOnMaterial : offMaterial;
        dObject.material = targetMaterial;
        oObject.material = targetMaterial;
        wObject.material = targetMaterial;
        nObject.material = targetMaterial;
    }

    // �׽�Ʈ��
    [ContextMenu("Compare Now")]
    private void TestCompare() => CompareWithTarget();
}