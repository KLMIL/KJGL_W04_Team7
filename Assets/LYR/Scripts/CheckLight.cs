using UnityEngine;

public class CheckLight : MonoBehaviour
{

    private Light objectLight;           // ������Ʈ�� Light ������Ʈ
    private bool isActive = false;       // Ȱ��ȭ ���� üũ
    private float activeTimer = 0f;      // Ÿ�̸�

    void Start()
    {


        objectLight = GetComponent<Light>();
        if (objectLight != null)
        {
            objectLight.enabled = false;     // ó������ ���� ����
        }
    }

    void Update()
    {
        //if (isActive)
        //{
        //    activeTimer += Time.deltaTime;

            
        //    if (activeTimer >= activeDuration)
        //    {

        //        if (objectLight != null)
        //        {
        //            objectLight.enabled = false;    // ����Ʈ ����
        //        }
        //        isActive = false;
        //        activeTimer = 0f;
        //    }
        //}

    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            activeTimer = 0f;


            if (objectLight != null)
            {
                objectLight.enabled = true;
            }
        }
    }
}
