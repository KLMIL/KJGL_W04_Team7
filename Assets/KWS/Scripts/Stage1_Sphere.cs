using UnityEngine;

public class Stage1_Sphere : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.SetPlayer2Dead(true);
        }
    }
}
