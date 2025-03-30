using UnityEngine;

public class Stage1_SphereSurvivePlayer : MonoBehaviour
{
    [SerializeField] private Stage1Manager stage1Manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RollingStone"))
        {
            stage1Manager.EndStage();
        }
    }
}
