using UnityEngine;

public class Stage1_SphereKillPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RollingStone"))
        {
            GameManager.Instance.SetPlayer2Dead(true);
        }
    }
}
