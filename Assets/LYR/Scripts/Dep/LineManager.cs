using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public List<GreenLineControl> lines; // ������ ��� GreenLineControl ������Ʈ

    private bool isButtonPressed = false; // ��ư�� ���ȴ��� ����

    public string methodName = "Activate"; // ȣ���� �޼ҵ� �̸�
    public List<GameObject> targetObjects;

    void Start()
    {

    }

    void Update()
    {
        CheckConditions();
    }

    // ��� ������ Ȱ��ȭ�ǰ� ��ư�� ���ȴ��� Ȯ��
    private void CheckConditions()
    {
        if (AreAllLinesActive())
        {
            InvokeMethod();
            enabled = false; // �� �̻� üũ���� �ʵ��� ��ũ��Ʈ ��Ȱ��ȭ (���û���)
        }
    }

    // ��� ������ Ȱ��ȭ�Ǿ����� Ȯ��
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


    private void InvokeMethod()
    {
        if (targetObjects != null)
        {
            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                {
                    obj.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
                    Debug.Log($"{methodName} �޼ҵ尡 ȣ��Ǿ����ϴ�!");
                }
            }

        }
        else
        {
            Debug.LogWarning("Ÿ�� ������Ʈ�� �������� �ʾҽ��ϴ�.");
        }
    }




}