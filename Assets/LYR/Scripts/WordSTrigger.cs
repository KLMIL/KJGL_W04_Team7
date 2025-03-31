using UnityEngine;

public class WordSTrigger : MonoBehaviour
{
    WordSManager wordSManager;
    void Start()
    {
        wordSManager = FindAnyObjectByType<WordSManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        wordSManager.isClosing = true;
    }


}
