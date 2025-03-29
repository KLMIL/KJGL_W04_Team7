using System.Collections.Generic;
using UnityEngine;

public class CheckLight : MonoBehaviour
{
    public List<GreenLightControl> lightControls = new List<GreenLightControl>(); // GreenLightControl ������Ʈ ����Ʈ
    private int[] correctSequence = { 0,4,1,3 }; // 4���� �ùٸ� ����
    [SerializeField]
    private List<int> playerSequence = new List<int>(); // �÷��̾� �Է� ����
    private int currentStep = 0;
    private bool[] previousLightStates; // ���� �������� ����Ʈ ���� ����
    [SerializeField]
    private bool isResetting = false;

    void Start()
    {
        previousLightStates = new bool[lightControls.Count];
        for (int i = 0; i < lightControls.Count; i++)
        {
            previousLightStates[i] = false;
        }
    }

    void Update()
    {
        if (isResetting) return;

        for (int i = 0; i < lightControls.Count; i++)
        {
            bool currentState = lightControls[i].transform.GetChild(0).GetComponent<Light>().enabled;
            if (currentState && !previousLightStates[i]) // �����ִٰ� ���� ���
            {
                HandleLightActivation(i);
            }
            previousLightStates[i] = currentState;
        }
    }

    private void HandleLightActivation(int lightIndex)
    {
        playerSequence.Add(lightIndex);
        Debug.Log($"Light {lightIndex} activated");

        // ������ ���� ����Ʈ���� Ȯ��
        bool isInvalidLight = true;
        foreach (int correctIndex in correctSequence)
        {
            if (lightIndex == correctIndex)
            {
                isInvalidLight = false;
                break;
            }
        }

        if (isInvalidLight)
        {
            Debug.Log("������ ���� ����Ʈ�� �������ϴ�. ����!");
            Invoke("ResetLights", 0.1f);
            return;
        }

        // ������ �´��� Ȯ��
        if (playerSequence[currentStep] == correctSequence[currentStep])
        {
            currentStep++;
            if (currentStep >= correctSequence.Length)
            {
                Debug.Log("����! 4���� ����Ʈ�� �ùٸ� ������ �׽��ϴ�.");
            }
        }
        else
        {
            Debug.Log("������ Ʋ�Ƚ��ϴ�. ����!");
            Invoke("ResetLights", 0.1f);
        }
    }

    private void ResetLights()
    {
        isResetting = true;
        foreach (GreenLightControl lightControl in lightControls)
        {
            lightControl.Deactivate(); // GreenLightControl�� Deactivate ȣ��
        }
        playerSequence.Clear();
        currentStep = 0;
        Invoke("FinishReset", 0.1f);
    }

    private void FinishReset()
    {
        for (int i = 0; i < previousLightStates.Length; i++)
        {
            previousLightStates[i] = false; // ���� ���� �ʱ�ȭ
        }
        isResetting = false;
        Debug.Log("���� �Ϸ�");
    }
}