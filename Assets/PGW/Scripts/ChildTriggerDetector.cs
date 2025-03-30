using UnityEngine;

public class ChildTriggerDetector : MonoBehaviour
{
    public delegate void TriggerEvent(Collider other);
    public event TriggerEvent onTriggerEnter;
    public event TriggerEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExit?.Invoke(other);
    }
}
