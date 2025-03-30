using UnityEngine;

public class BottomButtonTrigger : MonoBehaviour
{
    private BottomButton parentButton;

    void Start()
    {
        parentButton = GetComponentInParent<BottomButton>();
        if (parentButton == null)
        {
            Debug.LogError("BottomButton 부모를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentButton != null)
        {
            parentButton.HandleTriggerEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (parentButton != null)
        {
            parentButton.HandleTriggerExit(other);
        }
    }
}
