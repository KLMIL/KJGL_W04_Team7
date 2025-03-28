using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class NumberPuzzle : MonoBehaviour
{
    private PuzzleLight[] lights; // 4���� light ��ũ��Ʈ �迭
    private List<int> activationOrder; // ���� Ȱ��ȭ ������ ���
    private int[] correctOrder = { 2, 1, 0, 3 }; // �ùٸ� ����
    private bool isPuzzleComplete = false; // ���� �Ϸ� ����
    public GameObject targetObject; // ȣ���� ��� ������Ʈ
    private string methodName = "Activate"; // ȣ���� �޼ҵ� �̸�

    void Start()
    {
        // �ʱ�ȭ
        activationOrder = new List<int>();

        // �ڽ� ������Ʈ���� light ������Ʈ ��������
        lights = new PuzzleLight[4];
        for (int i = 0; i < 4; i++)
        {
            Transform child = transform.GetChild(i); // Buttonboard�� i��° �ڽ�
            Transform lightTransform = child.Find("light"); // �ڽ��� light ������Ʈ
            if (lightTransform != null)
            {
                lights[i] = lightTransform.GetComponent<PuzzleLight>();
                if (lights[i] != null)
                {
                    // Light ��ũ��Ʈ�� �̺�Ʈ �ڵ鷯 �߰� (SetActive ȣ�� ����)
                    lights[i].onSetActive += HandleLightActivation;
                }
                else
                {
                    Debug.LogError($"light ������Ʈ�� PuzzleLight ��ũ��Ʈ�� �����ϴ�: {lightTransform.name}");
                }
            }
            else
            {
                Debug.LogError($"light ������Ʈ�� ã�� �� �����ϴ�: {child.name}");
            }
        }
    }

    // Light�� Ȱ��ȭ�� �� ȣ��Ǵ� �ڵ鷯
    private void HandleLightActivation(bool isActive, GameObject lightObject)
    {
        if (isPuzzleComplete) return; // ������ �Ϸ�Ǹ� �� �̻� üũ���� ����

        if (isActive) // Ȱ��ȭ�� ���� ���� üũ
        {
            // Ȱ��ȭ�� light�� �ε��� ã��
            int index = Array.FindIndex(lights, l => l.gameObject == lightObject);
            if (index != -1 && !activationOrder.Contains(index)) // �ߺ� ����
            {
                activationOrder.Add(index);
                CheckOrder();
            }
        }
    }

    // ������ �´��� Ȯ��
    private void CheckOrder()
    {
        Debug.Log($"���� ����: {string.Join(", ", activationOrder)}");

        if (activationOrder.Count == correctOrder.Length)
        {
            bool isCorrect = activationOrder.SequenceEqual(correctOrder);
            if (isCorrect)
            {
                Debug.Log("������ �½��ϴ�! ���� �Ϸ�!");
                isPuzzleComplete = true; // ���� �Ϸ� ���·� ����

                // Ÿ�� ������Ʈ�� �޼ҵ� ȣ��
                if (targetObject != null)
                {
                    targetObject.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
                    Debug.Log($"{methodName} �޼ҵ尡 ȣ��Ǿ����ϴ�!");
                }
                else
                {
                    Debug.LogWarning("Ÿ�� ������Ʈ�� �������� �ʾҽ��ϴ�.");
                }

                // 3�� �� ������ ���� �ڷ�ƾ ����
                StartCoroutine(ResetAfterDelay(3f));
            }
            else
            {
                Debug.Log("������ Ʋ�Ƚ��ϴ�. �ʱ�ȭ�մϴ�.");
                ResetPuzzle();
            }
        }
    }

    // ���� �ʱ�ȭ
    private void ResetPuzzle()
    {
        activationOrder.Clear(); // �� ���� ȣ��
        foreach (var light in lights)
        {
            if (light != null)
            {
                light.SetActive(false); // ��� ����Ʈ ��Ȱ��ȭ
            }
        }
        isPuzzleComplete = false; // ���� �Ϸ� ���� ����
        Debug.Log("������ �ʱ�ȭ�Ǿ����ϴ�. �ٽ� �õ��ϼ���.");
    }

    // ������ �ð� �� ������ �����ϴ� �ڷ�ƾ
    private System.Collections.IEnumerator ResetAfterDelay(float delay)
    {
        Debug.Log($"���� �Ϸ� �� {delay}�� ��� �� ���µ˴ϴ�.");
        yield return new WaitForSeconds(delay);
        ResetPuzzle();
    }
}