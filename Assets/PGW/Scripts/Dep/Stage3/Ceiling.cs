using UnityEngine;

public class Ceiling : MonoBehaviour
{
    public PlayerController playerController;   

    private void OnTriggerEnter(Collider other)
    {
        playerController.HandleDie();
    }
}
