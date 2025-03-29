using UnityEngine;

public class Stage1Manager : MonoBehaviour
{
    [SerializeField] private GameObject rollingStone;
    [SerializeField] private GameObject room1LockWall;
    [SerializeField] private GameObject room2LockWall;
    [SerializeField] private GameObject downHatch;
    [SerializeField] private GameObject upHatch;
    [SerializeField] private GameObject[] floors;

    [SerializeField] private float sphereVelocity = 10f;
    //[SerializeField] private Transform savePoint; 
    //[SerializeField] private GameObject Player1;
    //[SerializeField] private GameObject Player2;

    private Vector3 rollingStoneStartPos;


    private void Start()
    {
        room1LockWall.SetActive(true);
        room2LockWall.SetActive(false);
        upHatch.SetActive(true);
        downHatch.SetActive(true);
        rollingStone.SetActive(false);
        rollingStoneStartPos = rollingStone.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartStage();
        }
    }


    private void StartStage()
    {
        room1LockWall.SetActive(false);
        room2LockWall.SetActive(true);
        upHatch.SetActive(false);
        foreach (GameObject floor in floors)
        {
            floor.SetActive(false);
        }
        rollingStone.SetActive(true);
        Invoke("AddVelocityToSphere", 3f);
        Debug.Log("Stage 1 Start");
    }

    private void AddVelocityToSphere()
    {
        rollingStone.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, -1 * sphereVelocity);
        Debug.Log("Velocity Added");
    }

    public void OpenDownHatch()
    {
        downHatch.SetActive(false);
    }

    public void ResetStage()
    {
        room1LockWall.SetActive(true);
        room2LockWall.SetActive(false);
        upHatch.SetActive(true);
        downHatch.SetActive(true);
        rollingStone.SetActive(false);
        rollingStone.transform.position = rollingStoneStartPos;

        Rigidbody rb = rollingStone.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Stage 1 Reset");
    }
}
