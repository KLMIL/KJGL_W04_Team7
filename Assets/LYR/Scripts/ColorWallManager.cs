using System;
using UnityEngine;

public class ColorWallManager : MonoBehaviour
{
    public WallButton[] buttons;    //0~2 : 벽버튼(RGB), 3 : 올라오는 발판 버튼, 4 : 문 버튼
    public GameObject[] wallRed;    //Red 버튼 눌리면 열리는 벽
    public GameObject[] wallBlue;   //Blue 버튼 눌리면 열리는 벽
    public GameObject[] wallGreen;  //Green 버튼 눌리면 열리는 벽
    public DoorControl[] doors;     //문 버튼 눌리면 열릴 문



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ButtonListen(GameObject button)
    {
        Debug.Log("버튼 눌림");
        int buttonIndex = Array.IndexOf(buttons, button);
        
        if (buttonIndex == -1) return;
        Debug.Log($"Button Index: {buttonIndex}");

        switch (buttonIndex)
        {
            case 0:     //Red Wall
                foreach (GameObject wall in wallRed)
                {
                    wall.GetComponentInChildren<ButtonShape>().gameObject.SetActive(false);
                    wall.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    Debug.Log("빨간 문 열림");
                }
                break;

            case 1:     //Green Wall

                break;

            case 2:     //Blue Wall

                break;

            case 3:     //올라오는 발판

                break;

            case 4:     //문 열리게 하는 버튼

                break;

        }
    }
}
