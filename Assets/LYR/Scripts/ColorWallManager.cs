using System;
using UnityEngine;

public class ColorWallManager : MonoBehaviour
{
    public WallButton[] buttons;    //0~2 : ����ư(RGB), 3 : �ö���� ���� ��ư, 4 : �� ��ư
    public GameObject[] wallRed;    //Red ��ư ������ ������ ��
    public GameObject[] wallBlue;   //Blue ��ư ������ ������ ��
    public GameObject[] wallGreen;  //Green ��ư ������ ������ ��
    public DoorControl[] doors;     //�� ��ư ������ ���� ��



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
        Debug.Log("��ư ����");
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
                    Debug.Log("���� �� ����");
                }
                break;

            case 1:     //Green Wall

                break;

            case 2:     //Blue Wall

                break;

            case 3:     //�ö���� ����

                break;

            case 4:     //�� ������ �ϴ� ��ư

                break;

        }
    }
}
