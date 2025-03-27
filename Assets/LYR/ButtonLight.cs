using UnityEngine;

public class ButtonLight : MonoBehaviour
{
    public WallButtonOnce button; // ������ WallButtonOnce ��ũ��Ʈ
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
            Debug.LogWarning("WallButtonOnce ��ũ��Ʈ�� ������� �ʾҽ��ϴ�.");
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
