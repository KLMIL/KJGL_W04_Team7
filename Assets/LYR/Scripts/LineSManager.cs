using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class LineSManager : MonoBehaviour
{
    public GameObject[] buttons;    //��ư 1~6
    public List<GreenLineControl> lines;    //�̾����� ��
    public GameObject[] lights;     //������ ��
    public GameObject[] checkLights; //����
    private GreenLightControl[] lightConditions;
    public GameObject[] doors;

    private int[] correctSequence = { 5, 3, 1, 4 }; // 4���� �ùٸ� ����
    [SerializeField]
    private List<int> playerSequence = new List<int>(); // �÷��̾� �Է� ����
    private int currentStep = 0;
    private bool[] previousLightStates; // ���� �������� ����Ʈ ���� ����
    private bool isResetting = false;



    void Start()
    {

        lightConditions = new GreenLightControl[checkLights.Length];

        // �� GameObject���� LightCondition ��ũ��Ʈ ��������
        for (int i = 0; i < checkLights.Length; i++)
        {
            if (checkLights[i] != null)
            {
                lightConditions[i] = checkLights[i].GetComponent<GreenLightControl>();

            }
        }

        previousLightStates = new bool[lightConditions.Length];
        for (int i = 0; i < lightConditions.Length; i++)
        {
            previousLightStates[i] = false;
        }
    }

    void Update()
    {
        CheckLineConditions();
        CheckState();
    } 




    public void ButtonListen(GameObject button) 
    {
        int buttonIndex = Array.IndexOf(buttons, button);

        if (buttonIndex < 0) return;

        foreach (GameObject light in lights)
        {
            light.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver);
        }

        // ���õ� ��ư�� �ش��ϴ� �Һ��� �� (Activate ȣ��)
        if (buttonIndex < lights.Length) // lights �迭 ���� üũ
        {
            lights[buttonIndex].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
            checkLights[buttonIndex].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
        }
    }

    private bool AreAllLinesActive()
    {
        if (lines == null || lines.Count == 0) return false;

        foreach (GreenLineControl line in lines)
        {
            if (!line.IsCorrect) // public getter�� ����� ���� Ȯ��
            {
                return false;
            }
        }
        return true;
    }

    private void CheckLineConditions()
    {
        if (AreAllLinesActive())
        {
            DoorOpen();
        }
    }

    void DoorOpen()
    {
        if (doors != null)
        {
            foreach (GameObject obj in doors)
            {
                if (obj != null)
                {
                    obj.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    Debug.Log("�� ����!");
                }
            }

        }
        else
        {
            Debug.LogWarning("doors�� �������� �ʾҽ��ϴ�.");
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
        foreach (GameObject cLight in checkLights)
        {
            cLight.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver);

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


    void CheckState()
    {
        if (isResetting) return;
        for (int i = 0; i < lightConditions.Length; i++)
        {
            bool currentState = lightConditions[i].transform.GetChild(0).GetComponent<Light>().enabled;
            if (currentState && !previousLightStates[i]) // �����ִٰ� ���� ���
            {
                HandleLightActivation(i);
            }
            previousLightStates[i] = currentState;
        }
    }


}


