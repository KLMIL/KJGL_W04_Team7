using UnityEngine;

public class WallPassed : MonoBehaviour
{
    private GameObject closingWall;

    private void Awake()
    {
        closingWall = transform.parent.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            closingWall.GetComponent<ClosingWall_1>().isPlayerPassed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            closingWall.GetComponent<ClosingWall_1>().isPlayerPassed = false;
        }
    }
}
