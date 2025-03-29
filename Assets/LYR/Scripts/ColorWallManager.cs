using System;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class ColorWallManager : MonoBehaviour
{
    public GameObject[] buttons;    //0~2 : ����ư(RGB), 3 : �ö���� ���� ��ư, 4 : �� ��ư
    public GameObject[] wallRed;    //Red ��ư ������ ������ ��
    public GameObject[] wallBlue;   //Blue ��ư ������ ������ ��
    public GameObject[] wallGreen;  //Green ��ư ������ ������ ��
    public GameObject[] upperfloor;
    public GameObject[] doors;     //�� ��ư ������ ���� ��
    

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
        Debug.Log("��ư ����");
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
                    //Debug.Log("���� �� ����");
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
                    //Debug.Log("�Ķ� �� ����");
                }

                break;

            case 2:     //Green Wall
                RedDoorClose();
                BlueDoorClose();

                foreach (GameObject wall in wallGreen)
                {
                    wall.GetComponentInChildren<ButtonShapeGreen>().gameObject.SetActive(false);
                    wall.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
                    //Debug.Log("�ʷ� �� ����");
                }
                break;

            case 3:     //�ö���� ����

/*                foreach (GameObject floor in upperfloor)
                {
                    floor.SendMessage("HandleButtonState", SendMessageOptions.DontRequireReceiver);
                }*/
                break;

            case 4:     //�� ������ �ϴ� ��ư
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
            //Debug.Log("���� �� ����");
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
            //Debug.Log("�Ķ� �� ����");
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
            //Debug.Log("�ʷ� �� ����");
        }
    }


}