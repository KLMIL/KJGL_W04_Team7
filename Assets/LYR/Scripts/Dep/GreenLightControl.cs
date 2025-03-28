using UnityEngine;

public class GreenLightControl : MonoBehaviour
{
    //private Material material;           // ������Ʈ�� ���͸���
    //private Color originalColor;         // ���� ���� ����
    private Light objectLight;           // ������Ʈ�� Light ������Ʈ
    private bool isActive = false;       // Ȱ��ȭ ���� üũ
    private float activeTimer = 0f;      // Ÿ�̸�

    // ���� ����
    public float activeDuration = 2f;    // Ȱ��ȭ ���� ���� �ð� (��)

    void Start()
    {
        // ���͸���� ����Ʈ ������Ʈ ��������
        /*        Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    material = renderer.material;
                    originalColor = material.color;  // ���� ���� ����
                    // ���� �� #11820A �������� ���� (16���� -> RGB ��ȯ)
                    material.color = new Color32(17, 130, 10, 255); // #11820A
                }*/

        objectLight = transform.GetChild(0).GetComponent<Light>();
        if (objectLight != null)
        {
            objectLight.enabled = false;     // ó������ ���� ����
        }
    }

    void Update()
    {
        if (isActive)
        {
            activeTimer += Time.deltaTime;

            // ������ �ð��� ������ ���� ���·� ����
            if (activeTimer >= activeDuration)
            {
                /*                if (material != null)
                                {
                                    material.color = new Color32(17, 130, 10, 255); // #11820A�� ����
                                }*/
                if (objectLight != null)
                {
                    objectLight.enabled = false;    // ����Ʈ ����
                }
                isActive = false;
                activeTimer = 0f;
            }
        }
        //Activate();
    }

    public void ButtonListen()
    {
        if (!isActive)
        {
            isActive = true;
            activeTimer = 0f;

            /*            if (material != null)
                        {
                            // ���� �������� ����
                            material.color = originalColor;
                        }*/

            if (objectLight != null)
            {
                // ����Ʈ �ѱ�
                objectLight.enabled = true;
            }
        }
    }

    public void Deactivate()
    {
        if (objectLight != null)
        {
            objectLight.enabled = false; // ����Ʈ ����
        }
        isActive = false;
        activeTimer = 0f;
    }

}
