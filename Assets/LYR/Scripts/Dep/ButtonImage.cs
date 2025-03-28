using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour
{
    public WallButtonOnce button; // 참조할 WallButtonOnce 스크립트
    public Image interactionImage; // 표시할 UI 이미지

    void Start()
    {
        // 시작 시 이미지 비활성화
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (button == null)
        {
            Debug.LogWarning("WallButtonOnce 스크립트가 연결되지 않았습니다.");
            return;
        }

        // 플레이어가 범위 안에 있고 버튼이 눌리지 않았으면 이미지 표시
        if (button.IsPlayerInRange)
        {
            ShowImage();
        }
        else
        {
            HideImage();
        }
    }

    // 이미지 표시
    private void ShowImage()
    {
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(true);
        }
    }

    // 이미지 숨김
    private void HideImage()
    {
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(false);
        }
    }
}
