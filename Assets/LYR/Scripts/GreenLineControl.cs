using System.Collections.Generic;
using UnityEngine;

public class GreenLineControl : MonoBehaviour
{
    private Material material;
    private Color originalEmissionColor;
    private float activeTimer = 0f;
    private bool isCorrect = false;

    public float activeDuration = 2f;
    public List<GameObject> lightsToCheck;
    public GreenLineControl dependentLine;

    // ���¸� �ܺο��� ���� �� �ֵ��� public getter �߰�
    public bool IsCorrect => isCorrect;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            originalEmissionColor = material.GetColor("_EmissionColor");
            material.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        // ����Ʈ ���� üũ �� ���� ������Ʈ
        if (isCorrect)
        {
            activeTimer += Time.deltaTime;

            // �� ����Ʈ�� ��� �������� ������ ��� ���� ���·� ����
            if (!AreAssignedLightsActive())
            {
                ResetColor();

            }
            // ������ �ð��� ������ ���� ���·� ����
            else if (activeTimer >= activeDuration)
            {
                ResetColor();

            }
        }
        else
        {
            Activate();

        }
    }

    public void Activate()
    {
        if (!isCorrect && AreAssignedLightsActive())
        {
            isCorrect = true;
            activeTimer = 0f;
            Color newEmissionColor = new Color(7f / 255f, 120f / 255f, 0f / 255f);

            if (material != null)
            {
                material.SetColor("_EmissionColor", newEmissionColor);
            }
        }
    }

    private void ResetColor()
    {
        if (material != null)
        {
            material.SetColor("_EmissionColor", originalEmissionColor);
        }
        isCorrect = false;
        activeTimer = 0f;
    }

    private bool AreAssignedLightsActive()
    {
        if (lightsToCheck == null || lightsToCheck.Count != 2) return false;

        // �� ������Ʈ���� Light ������Ʈ ��������
        Light light1 = lightsToCheck[0].GetComponent<Light>();
        Light light2 = lightsToCheck[1].GetComponent<Light>();

        // Light ������Ʈ�� ���ų� ��Ȱ��ȭ ���¸� false ��ȯ
        if (light1 == null || light2 == null) return false;

        return light1.enabled && light2.enabled;
    }

    private bool IsDependentLineActive()
    {
        // dependentLine�� null�̸� ������ ���� (true ��ȯ)
        if (dependentLine == null) return true;

        // dependentLine�� IsCorrect�� false�� false ��ȯ
        return dependentLine.IsCorrect;
    }

    private void TurnOffSecondLight()
    {
        if (lightsToCheck == null || lightsToCheck.Count != 2) return;

        Light light2 = lightsToCheck[1].GetComponent<Light>();
        if (light2 != null)
        {
            light2.enabled = false;
            Debug.Log($"{gameObject.name}�� �� ��° ����Ʈ�� �������ϴ�.");
        }
    }



}
