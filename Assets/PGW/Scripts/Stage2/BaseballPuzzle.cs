using UnityEngine;
using System.Collections;

public class BaseballPuzzle : MonoBehaviour
{
    // ī��Ʈ ����
    private int greenCount = 0;  // �ʷ� ī��Ʈ (�ִ� 3)
    private int yellowCount = 0; // ��� ī��Ʈ (�ִ� 3)
    private int redCount = 0;    // ���� ī��Ʈ (�ִ� 1)

    // �ڽ� ������Ʈ�� �� �迭 (PuzzleLight ������Ʈ ����)
    private PuzzleLight[] greenLights;  // �ʷ� �� Robinhood puzzles�� �����ϴ� ��찡 ����. �ʷϻ��� ������� �ִ� 3��, �������� �ִ� 1������ ���� �� �ִ�.
    private PuzzleLight[] yellowLights;
    private PuzzleLight[] redLights;
    private float lightDuration = 5f; // ���� ���� �� ���� �������� �ð� (��)

    void Start()
    {
        // �ڽ� ������Ʈ���� �� �ʱ�ȭ
        InitializeLights();
    }

    // �� ������Ʈ �ʱ�ȭ
    private void InitializeLights()
    {
        // �ڽ� ������Ʈ�� �� 7��(�ʷ� 3, ��� 3, ���� 1)��� ����
        greenLights = new PuzzleLight[3];
        yellowLights = new PuzzleLight[3];
        redLights = new PuzzleLight[1];

        // �ڽ� ������Ʈ���� PuzzleLight ������Ʈ ��������
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Transform lightTransform = child.Find("light"); // �ڽ��� light ������Ʈ
            if (lightTransform != null)
            {
                PuzzleLight light = child.GetComponent<PuzzleLight>();
                if (light != null)
                {
                    // �ε����� �ʷ�, ���, ���� ����
                    if (i < 3) // 0~2: �ʷ�
                    {
                        greenLights[i] = light;
                    }
                    else if (i < 6) // 3~5: ���
                    {
                        yellowLights[i - 3] = light;
                    }
                    else if (i == 6) // 6: ����
                    {
                        redLights[0] = light;
                    }
                }
                else
                {
                    Debug.LogError($"�ڽ� ������Ʈ {child.name}�� PuzzleLight ������Ʈ�� �����ϴ�.");
                }
            }
            else
            {
                Debug.LogError($"light ������Ʈ�� ã�� �� �����ϴ�: {child.name}");
            }
            
            
        }

        // �ʱ� ���¿��� ��� �� ����
        TurnOffAllLights();
    }

    // ��� �� ����
    private void TurnOffAllLights()
    {
        foreach (var light in greenLights)
        {
            if (light != null) light.SetActive(false);
        }
        foreach (var light in yellowLights)
        {
            if (light != null) light.SetActive(false);
        }
        foreach (var light in redLights)
        {
            if (light != null) light.SetActive(false);
        }
    }

    // ī��Ʈ �״� �޼���
    public void AddGreenCount()
    {
        if (greenCount < 3)
        {
            greenCount++;
            Debug.Log($"Green Count: {greenCount}");
        }
        else
        {
            Debug.Log("Green Count�� �̹� �ִ�ġ(3)�Դϴ�.");
        }
    }

    public void AddYellowCount()
    {
        if (yellowCount < 3)
        {
            yellowCount++;
            Debug.Log($"Yellow Count: {yellowCount}");
        }
        else
        {
            Debug.Log("Yellow Count�� �̹� �ִ�ġ(3)�Դϴ�.");
        }
    }

    public void AddRedCount()
    {
        if (redCount < 1)
        {
            redCount++;
            Debug.Log($"Red Count: {redCount}");
        }
        else
        {
            Debug.Log("Red Count�� �̹� �ִ�ġ(1)�Դϴ�.");
        }
    }

    // ���� �Ѵ� �޼���
    public void TurnOnLights()
    {
        // ��� �� ���� ����
        TurnOffAllLights();

        // �ʷ� �� �ѱ�
        for (int i = 0; i < greenCount && i < greenLights.Length; i++)
        {
            if (greenLights[i] != null)
            {
                greenLights[i].SetActive(true);
            }
        }

        // ��� �� �ѱ�
        for (int i = 0; i < yellowCount && i < yellowLights.Length; i++)
        {
            if (yellowLights[i] != null)
            {
                yellowLights[i].SetActive(true);
            }
        }

        // ���� �� �ѱ�
        for (int i = 0; i < redCount && i < redLights.Length; i++)
        {
            if (redLights[i] != null)
            {
                redLights[i].SetActive(true);
            }
        }

        Debug.Log($"���� �������ϴ� - Green: {greenCount}, Yellow: {yellowCount}, Red: {redCount}");

        // �� ���� �� ������ �ð� �Ŀ� ������ ī��Ʈ �ʱ�ȭ
        StartCoroutine(ResetPuzzleAfterDelay());
    }

    // ������ �ð� �� �� ���� ī��Ʈ �ʱ�ȭ
    private IEnumerator ResetPuzzleAfterDelay()
    {
        yield return new WaitForSeconds(lightDuration);
        TurnOffAllLights();
        greenCount = 0;
        yellowCount = 0;
        redCount = 0;
        Debug.Log("���� ������ ī��Ʈ�� �ʱ�ȭ�Ǿ����ϴ�.");
    }

    // ������: ���� ī��Ʈ Ȯ��
    public void LogCounts()
    {
        Debug.Log($"���� ī��Ʈ - Green: {greenCount}, Yellow: {yellowCount}, Red: {redCount}");
    }
}