using UnityEngine;

public class ButtonEmission : MonoBehaviour
{
    private WallButton wallButtonScript;
    //private WallButtonOnce wallButtonScript;
    private Material material;
    private Color emissionColor;



    void Start()
    {
        wallButtonScript = GetComponentInParent<WallButton>();
        //wallButtonScript = GetComponent<WallButtonOnce>();

        //���͸��� �ִ��� Ȯ���ϰ� ������ ������Ʈ�� ���͸��� ��������
        if (material == null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                material = renderer.material;
            }
        }
    }



    void Update()
    {
        bool isPressed = wallButtonScript.IsButtonPressed;

        if (isPressed) EmissonOn();
        else EmissonOff();
    }

    void EmissonOn()
    {
        material.EnableKeyword("_EMISSION");
        //emissionColor = new Color(255f, 255f, 255f);
        //material.SetColor("_EmissionColor", emissionColor);
    }

    void EmissonOff()
    {
        material.DisableKeyword("_EMISSION");

    }



}
