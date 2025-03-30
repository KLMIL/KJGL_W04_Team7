using UnityEngine;

public class Stage6Manager : MonoBehaviour
{
    private int isRoomCleared = 0;
    [SerializeField] private DoorControl door1;
    [SerializeField] private DoorControl door2;


    public void ClearRoom()
    {
        isRoomCleared++;
        if (isRoomCleared == 2)
        {
            door1.Activate();
            door2.Activate();
        }
    }
}
