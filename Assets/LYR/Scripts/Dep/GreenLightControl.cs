using UnityEngine;

public class GreenLightControl : MonoBehaviour
{

    private Light objectLight;           // ������Ʈ�� Light ������Ʈ
    private bool isActive = false;       // Ȱ��ȭ ���� üũ
    private float activeTimer = 0f;      // Ÿ�̸�


    public bool autoOff = false;         // �ð� ������ �ڵ� Off
    public float activeDuration = 2f;    // Ȱ��ȭ ���� ���� �ð� (��)

    void Start()
    {
        objectLight = transform.GetChild(0).GetComponent<Light>();
        if (objectLight != null)
        {
            objectLight.enabled = false;     // ó������ ���� ����
        }
    }

    void Update()
    {
        if (isActive && autoOff)
        {
            activeTimer += Time.deltaTime;
            // ������ �ð��� ������ ���� ���·� ����
            if (activeTimer >= activeDuration)
            {
                if (objectLight != null)
                {
                    objectLight.enabled = false;    // ����Ʈ ����
                }
                isActive = false;
                activeTimer = 0f;
            }
        }
    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            activeTimer = 0f;


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
