using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour
{
    public WallButtonOnce button; // ������ WallButtonOnce ��ũ��Ʈ
    public Image interactionImage; // ǥ���� UI �̹���

    void Start()
    {
        // ���� �� �̹��� ��Ȱ��ȭ
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (button == null)
        {
            Debug.LogWarning("WallButtonOnce ��ũ��Ʈ�� ������� �ʾҽ��ϴ�.");
            return;
        }

        // �÷��̾ ���� �ȿ� �ְ� ��ư�� ������ �ʾ����� �̹��� ǥ��
        if (button.IsPlayerInRange)
        {
            ShowImage();
        }
        else
        {
            HideImage();
        }
    }

    // �̹��� ǥ��
    private void ShowImage()
    {
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(true);
        }
    }

    // �̹��� ����
    private void HideImage()
    {
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(false);
        }
    }
}
