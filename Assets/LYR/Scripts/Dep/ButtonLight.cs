using UnityEngine;

public class Button : MonoBehaviour
{
    public Square_Button button; // ������ WallButtonOnce ��ũ��Ʈ
    public Light targetLight; // ������ Light ������Ʈ

    void Start()
    {
        // ���� �� ���� ���� �ʱ�ȭ (������)
        if (targetLight != null)
        {
            targetLight.enabled = false; // �⺻������ ����
        }
    }

    void Update()
    {
        if (button == null)
        {
            Debug.LogWarning("Square_Button ��ũ��Ʈ�� ������� �ʾҽ��ϴ�.");
            return;
        }

        if (targetLight == null)
        {
            Debug.LogWarning("Light ������Ʈ�� ������� �ʾҽ��ϴ�.");
            return;
        }

        // IsButtonPressed�� true�� ������ �Ѱ�, false�� ��
        targetLight.enabled = button.IsButtonPressed;
    }
}
