using UnityEngine;

public class Trigger : MonoBehaviour
{
    

    public WordSManager Controller; // DeviceController 참조

    private void Start()
    {
        Controller = FindAnyObjectByType<WordSManager>();
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter!");
        if (other.CompareTag("Player")) // 플레이어가 트리거를 발생시켰다고 가정
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



