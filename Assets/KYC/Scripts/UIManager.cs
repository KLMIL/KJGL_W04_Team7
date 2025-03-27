using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameStartScreen;
    public GameObject gameOverScreen;
    public GameObject howtoplayScreen;
    public TextMeshProUGUI stageText;
    public GameObject escapeSuccess;
    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        howtoplayScreen.SetActive(false);
    }

    public void ShowHowToPlay()
    {
        howtoplayScreen.SetActive(true);
    }
}
