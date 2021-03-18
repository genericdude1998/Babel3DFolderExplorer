using System.Collections.Generic;
using UnityEngine;

public class OcclusionCullingStackManager : MonoBehaviour
{
    public int occlusionCullingStackManagerIndex { get; private set; }
    public int numberOfAdjacentRoomsToRenderDown = 1; // this will give you the number of rooms other than the one you're in to be rendered
    public int numberOfAdjacentRoomsToRenderUp = 2; // this will give you the number of rooms other than the one you're in to be rendered


    public List<Room> listOfStackRooms;
    public int currentRoomIndex = -1;
    public bool enteringRoom = false;
    public bool exitingRoom = false;
   


    // Start is called before the first frame update
    public void OcclusionCullingStackManagerInit(List<Room> roomsListInTheStack, int cullingStackIndex)
    {
        listOfStackRooms = roomsListInTheStack;
        occlusionCullingStackManagerIndex = cullingStackIndex;
        enteringRoom = false;
        currentRoomIndex = -1;
        gameObject.name += occlusionCullingStackManagerIndex.ToString();

        for (int stackIndex = 0; stackIndex < listOfStackRooms.Count; stackIndex++)
        {
            bool isToBeRenderedAtStart = occlusionCullingStackManagerIndex == 0 && stackIndex <= 1; // should leave the fist two rooms in the first stack rendered at start

            listOfStackRooms[stackIndex].RoomInit(this, stackIndex, isToBeRenderedAtStart); // set refernce to this manager and sets the index
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RefreshRooms(-1);
    }

    private void Update()
    {
        if (enteringRoom) 
        {
            RefreshRooms(currentRoomIndex);
            enteringRoom = false;
        }

        if (exitingRoom) 
        {
            RefreshRooms(-1);
            exitingRoom = false;
        }
    }

    void RefreshRooms(int index) 
    {
        if (index < 0)
        {
            foreach (Room room in listOfStackRooms)
            {
                room.CullRoom(); // clears all rooms in the stack
            }

            return;
        }

        else
        {
            bool isLastRoomInTheStack;
            bool isFirstRoomInTheStack;

            isLastRoomInTheStack = index == listOfStackRooms.Count - 1;
            isFirstRoomInTheStack = index == 0;

            foreach (Room room in listOfStackRooms)
            {
                room.CullRoom(); // clears all rooms in the stack then checks which to render
            }

            listOfStackRooms[index].RenderRoom();

            if (!isLastRoomInTheStack) 
            { 
                listOfStackRooms[index + numberOfAdjacentRoomsToRenderDown].RenderRoom();
            }

            if(!isFirstRoomInTheStack)
            {
                if (index - numberOfAdjacentRoomsToRenderUp < 0)
                {
                    listOfStackRooms[0].RenderRoom();
                }

                else
                {
                    for (int i = 1; i <= numberOfAdjacentRoomsToRenderUp ; i++)
                    {
                        listOfStackRooms[index - i].RenderRoom();
                        Debug.Log(i);
                    }
                }
            }
        }
    }
}
