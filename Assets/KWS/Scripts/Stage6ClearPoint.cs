using UnityEngine;

public class Stage6ClearPoint : MonoBehaviour
{
    [SerializeField] private Stage6Manager stage6Manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            stage6Manager.ClearRoom();
        }
    }
}
