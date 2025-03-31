using UnityEngine;

public class Trigger : MonoBehaviour
{
    

    public WordSManager Controller; // DeviceController ����

    private void Start()
    {
        Controller = FindAnyObjectByType<WordSManager>();
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter!");
        if (other.CompareTag("Player")) // �÷��̾ Ʈ���Ÿ� �߻����״ٰ� ����
        {
            Controller.OnWallTriggerEnter(gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Controller.OnWallTriggerExit(gameObject.name);
        }
    }
}



