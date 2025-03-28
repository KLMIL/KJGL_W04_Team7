using System.Collections;
using UnityEngine;

public class WallButton : MonoBehaviour
{
    public GameObject targetObject; // ȣ���� ��� ������Ʈ
    public string methodName = "Activate"; // ȣ���� �޼ҵ� �̸�

    public bool IsButtonPressed { get; private set; } = false;

    public void PressButton()
    {
        if (targetObject != null)
        {
            targetObject.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
            Debug.Log($"{methodName} �޼ҵ尡 ȣ��Ǿ����ϴ�!");
        }
        else
        {
            Debug.LogWarning("Ÿ�� ������Ʈ�� �������� �ʾҽ��ϴ�.");
        }
        IsButtonPressed = true;
        StartCoroutine(ResetButtonAfterDelay(1f)); // �ڷ�ƾ ����
    }


    private IEnumerator ResetButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð�(1��) ���
        IsButtonPressed = false; // ��ư ���� ����
        Debug.Log("��ư ���°� ���µǾ����ϴ�.");
    }
}
