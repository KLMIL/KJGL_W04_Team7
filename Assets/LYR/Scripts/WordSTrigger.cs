using UnityEngine;

public class WordSTrigger : MonoBehaviour
{
    WordSManager wordSManager;
    [SerializeField] private DoorControl doorControl;
    void Start()
    {
        wordSManager = FindAnyObjectByType<WordSManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        wordSManager.isClosing = true;
        doorControl.CloseDoor();
    }

    private void OnTriggerStay(Collider other)
    {
        wordSManager.isClosing = true;
        doorControl.CloseDoor();
    }


}
