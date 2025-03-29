using System;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class ColorWallManager : MonoBehaviour
{
    public GameObject[] buttons;    //0~2 : 벽버튼(RGB), 3 : 올라오는 발판 버튼, 4 : 문 버튼
    public GameObject[] wallRed;    //Red 버튼 눌리면 열리는 벽
    public GameObject[] wallBlue;   //Blue 버튼 눌리면 열리는 벽
    public GameObject[] wallGreen;  //Green 버튼 눌리면 열리는 벽
    public GameObject[] upperfloor;
    public GameObject[] doors;     //문 버튼 눌리면 열릴 문
    

    ButtonShapeRed[] redShapeScript;
    ButtonShapeBlue[] blueShapeScript;
    ButtonShapeGreen[] greenShapeScript;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        redShapeScript = FindObjectsByType<ButtonShapeRed>(FindObjectsSortMode.InstanceID);
        blueShapeScript = FindObjectsByType<ButtonShapeBlue>(FindObjectsSortMode.InstanceID);
        greenShapeScript = FindObjectsByType<ButtonShapeGreen>(FindObjectsSortMode.InstanceID);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ButtonListen(GameObject button)
    {
        Debug.Log("버튼 눌림");
        int buttonIndex = System.Array.IndexOf(buttons, button);

        if (buttonIndex == -1) return;

        Debug.Log($"Button Index: {buttonIndex}");

        switch (buttonIndex)
        {
            case 0:     //Red Wall

                BlueDoorClose();
                GreenDoorClose();
                foreach (var script in redShapeScript)
                {
                    script.gameObject.SetActive(false);
                }
                foreach (GameObject redWall in wallRed)
                {
                    redWall.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    //Debug.Log("빨간 문 열림");
                }

                break;

            case 1:     //Blue Wall
                RedDoorClose();
                GreenDoorClose();
                foreach (var script in blueShapeScript)
                {
                    script.gameObject.SetActive(false);
                }
                foreach (GameObject wall in wallBlue)
                {

                    wall.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    //Debug.Log("파란 문 열림");
                }

                break;

            case 2:     //Green Wall
                RedDoorClose();
                BlueDoorClose();

                foreach (GameObject wall in wallGreen)
                {
                    wall.GetComponentInChildren<ButtonShapeGreen>().gameObject.SetActive(false);
                    wall.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    //Debug.Log("초록 문 열림");
                }
                break;

            case 3:     //올라오는 발판

/*                foreach (GameObject floor in upperfloor)
                {
                    floor.SendMessage("HandleButtonState", SendMessageOptions.DontRequireReceiver);
                }*/
                break;

            case 4:     //문 열리게 하는 버튼
                foreach (GameObject door in doors)
                {
                    door.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                }
                break;

        }
    }

    void RedDoorClose()
    {
        foreach (var script in redShapeScript)
        {
            script.gameObject.SetActive(true);
        }
        foreach (GameObject wall in wallRed)
        {
            wall.SendMessage("DeActivate", SendMessageOptions.DontRequireReceiver);
            //Debug.Log("빨강 문 닫힘");
        }
    }
    void BlueDoorClose()
    {
        foreach (var script in blueShapeScript)
        {
            script.gameObject.SetActive(true);
        }
        foreach (GameObject wall in wallBlue)
        {
            wall.SendMessage("DeActivate", SendMessageOptions.DontRequireReceiver);
            //Debug.Log("파랑 문 닫힘");
        }
    }

    void GreenDoorClose()
    {
        foreach (var script in greenShapeScript)
        {
            script.gameObject.SetActive(true);
        }

        foreach (GameObject wall in wallGreen)
        {
            wall.SendMessage("DeActivate", SendMessageOptions.DontRequireReceiver);
            //Debug.Log("초록 문 닫힘");
        }
    }


}