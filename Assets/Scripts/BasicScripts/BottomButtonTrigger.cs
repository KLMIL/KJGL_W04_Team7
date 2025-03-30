using UnityEngine;

public class BottomButtonTrigger : MonoBehaviour
{
    private BottomButton parentButton;

    void Start()
    {
        parentButton = GetComponentInParent<BottomButton>();
        if (parentButton == null)
        {
            Debug.LogError("BottomButton �θ� ã�� �� �����ϴ�.");
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
