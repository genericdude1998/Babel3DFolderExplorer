using System.Collections.Generic;
using UnityEngine;

public class OcclusionCullingStacksManagerBuilder : Singleton<OcclusionCullingStacksManagerBuilder>
{ 
    protected void GenerateOcclusionCullingStacks(List<Node> listOfClientNodes, GameObject ovPrefab, int distanceBetweenRooms, int roomWidth, int roomHeight ,int corridorWidth)
    {
        int amountOfOcclusionStacks = GetNumberOfOcclusionStacks(listOfClientNodes, distanceBetweenRooms);
        //Debug.Log("AMOUNT OF STACKS" + amountOfOVStacks);

        for (int i = 0; i < amountOfOcclusionStacks; i++)
        {
            OcclusionCullingStackManager occlusionStackManager = Instantiate(ovPrefab).AddComponent<OcclusionCullingStackManager>();
            int depthOfStack = GetDepthOfTheStack(listOfClientNodes, i * distanceBetweenRooms);

            float xPosOfStack = i * distanceBetweenRooms;
            float yPosOfStack = depthOfStack / 2;

            occlusionStackManager.transform.position = new Vector3(xPosOfStack, yPosOfStack, 0);

            List<Room> stackRoomsList = GetComponentsInStack<Room>(listOfClientNodes, (int)xPosOfStack);
  
            occlusionStackManager.OcclusionCullingStackManagerInit(stackRoomsList,i);

            BoxCollider occlusionStackCollider = occlusionStackManager.gameObject.AddComponent<BoxCollider>();
            occlusionStackCollider.center = Vector3.zero;  // zero centers the box with his parent (local space)
            
           
            occlusionStackCollider.size = new Vector3(distanceBetweenRooms, Mathf.Abs(depthOfStack) + 2 * roomHeight, roomWidth + corridorWidth);
            
           

            // depth is ABS because you still need a positive number when setting sizes of BOXcollider depth of stack is negative
            // + one room height because the first room on the y=0 axis is not included (similar to the x axis = 0 situation below) 

            occlusionStackCollider.isTrigger = true;
        }
    }

    private int GetNumberOfOcclusionStacks(List<Node> listOfNodes, int distanceBetweenRooms) // max value must have some possible improvement
    {
        float highestXCoord = 0.0f;
        if (listOfNodes.Count > 0)
        {
            highestXCoord = listOfNodes[listOfNodes.Count - 1].m_position.x;
            return ((int)highestXCoord / distanceBetweenRooms) + 1;
        }   // this transforms the world space x coord into the number of stacks + 1 because the first stack counts as 0 on x coord

        return 0;
    }

    private int GetDepthOfTheStack(List<Node> listOfNodes, int stackXPos) // faster way is possible with max value
    {
        int depthOfTheStack = 0;
        foreach (Node clientNode in listOfNodes)
        {
            if ((int)clientNode.m_position.x == stackXPos)
            {
                if ((int)clientNode.m_position.y < depthOfTheStack)
                {
                    depthOfTheStack = (int)clientNode.m_position.y;
                }
            }
        }
        return depthOfTheStack;
    }

    private List<T> GetComponentsInStack<T>(List<Node> listOfClientNodes, int stackXPos) 
    {
        List<T> goList = new List<T>();
        foreach (Node node in listOfClientNodes) 
        {
              if (node.m_position.x == stackXPos)
              {
                    goList.Add(node.GetComponent<T>());
              }
        }

        return goList;
    }
}
