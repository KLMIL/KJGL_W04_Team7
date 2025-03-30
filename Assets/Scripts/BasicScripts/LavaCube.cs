using UnityEngine;

public class LavaCube : MonoBehaviour
{
    [SerializeField] private float sinkSpeed = 1f;
    [SerializeField] private float damageDelay = 0.5f;
    private GameObject player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("DisablePlayer"))
        {
            player = other.gameObject;

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
            }

            Invoke("KillPlayer", damageDelay);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("DisablePlayer"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, -sinkSpeed, rb.linearVelocity.z);
            }
            else
            {
                other.transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("DisablePlayer"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
            }
            CancelInvoke("KillPlayer");
            player = null;
        }
    }

    private void KillPlayer()
    {
        if (player != null)
        {
            GameManager.Instance.SetPlayerDead(true);
        }
    }
}