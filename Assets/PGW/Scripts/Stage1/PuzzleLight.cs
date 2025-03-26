using System;
using UnityEngine;

public class PuzzleLight : MonoBehaviour
{
    private Material material;           // ������Ʈ�� ���͸���
    private Color originalColor;         // ���� ���� ����
    private Light objectLight;           // ������Ʈ�� Light ������Ʈ
    private bool isActive = false;       // Ȱ��ȭ ���� üũ

    public Action<bool, GameObject> onSetActive; // SetActive ȣ�� �� �߻��ϴ� �̺�Ʈ

    
    void Start()
    {
        // ���͸���� ����Ʈ ������Ʈ ��������
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            originalColor = material.color;  // ���� ���� ����
            // ���� �� #11820A �������� ���� (16���� -> RGB ��ȯ)
            //material.color = new Color32(17, 130, 10, 255); // #11820A
        }

        objectLight = GetComponent<Light>();
        if (objectLight != null)
        {
            objectLight.enabled = false;     // ó������ ���� ����
        }
    }


    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;

            if (material != null)
            {
                // ���� �������� ����
                material.color = originalColor;
            }

            if (objectLight != null)
            {
                // ����Ʈ �ѱ�
                SetActive(true);
            }
        }
    }

    public void SetActive(bool active)
    {
        objectLight.enabled = active;
        isActive = false;
        onSetActive?.Invoke(active, gameObject); // �̺�Ʈ �߻�
    }
}
