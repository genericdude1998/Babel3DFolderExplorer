using System.Collections.Generic;
using UnityEngine;


public class LibraryBuilder : OcclusionCullingStacksManagerBuilder
{
    public List<Node> listOfClientNodes;

    public GameObject m_parentAndSiblingRoom;
    public GameObject m_parentAndNotSiblingRoom;
    public GameObject m_notParentAndSiblingRoom;
    public GameObject m_notParentAndNotSiblingRoom;


    public GameObject m_startCorridorPrefab;
    public GameObject m_connectionCorridorPrefab;
    public GameObject m_midCorridorPrefab;
    public GameObject m_endCorridorPrefab;

    private int m_distanceBetweenRooms;// this is the min distance on the x axis between rooms considering the ridiculous roof ftr
    private int m_corridorUnitLenght;
    private int m_numberOfConnectionCorridorsNeeded;
    private float m_sizeOfGapCorridorToRoom;

    public GameObject occlusionStackPrefab;

    //public bool DEBUG_MODE;
    public bool ENABLE_OCCLUSIONSTACKS;

    public void Build()
    {
        System.DateTime start = System.DateTime.Now;

        LibraryArchitect arch = FindObjectOfType<LibraryArchitect>();
        if (arch.generatedNodeList.Count != 0)
        {
            BuildLibrary(arch.generatedNodeList, arch.roomWidth, arch.roomHeight, (int)arch.sizeOfGapHorizontal, arch.corridorUnitLenght, arch.corridorUnitWidth, arch.sizeOfGapCorridorFromRoom);
        }

        System.DateTime end = System.DateTime.Now;
        Debug.Log("Builder Ended in " + end.Subtract(start).ToString());

    }

    public void BuildLibrary(List<Node> listOfArchitectNodes, int roomWidth, int roomHeight, int sizeOfGapHorizontal, int corridorUnitLenght, int corridorUnitWidth, float sizeOfGapCorridorFromRoom)
    {

        // these settings will make some of the parameters global variables of the class
        m_distanceBetweenRooms = sizeOfGapHorizontal; // this is the min distance on the x axis between rooms considering the ridiculous roof ftr
        m_sizeOfGapCorridorToRoom = sizeOfGapCorridorFromRoom; // this is a global variable for simplicity in the next helper methods
        m_corridorUnitLenght = corridorUnitLenght;
        m_numberOfConnectionCorridorsNeeded = m_distanceBetweenRooms / corridorUnitLenght;

        listOfClientNodes = new List<Node>();

        foreach (Node architectNode in listOfArchitectNodes)
        {
            InstantiateNodeClient(architectNode);
            InstantiateCorridor(architectNode);
        }

        if (ENABLE_OCCLUSIONSTACKS) GenerateOcclusionCullingStacks(listOfClientNodes, occlusionStackPrefab, m_distanceBetweenRooms, roomWidth, roomHeight, corridorUnitWidth);
    }

    private void InstantiateNodeClient(Node architectNode) // this creates a new set of nodes for the client
    {
        GameObject roomToSpawn = GetRoomClient(architectNode.m_isRootFolder, architectNode.m_isParent, architectNode.m_isSibling);
        Node m_clientNode = Instantiate(roomToSpawn).GetComponent<Node>();

        CopyNetworkNodeInfoToClientNode(m_clientNode, architectNode); // this copies all the properties of the network node into the client 

        listOfClientNodes.Add(m_clientNode);
    }

    private GameObject GetRoomClient(bool isRoot, bool isParent, bool isSibling)
    {
        if (isRoot)
        {
            if (isParent) { return m_parentAndNotSiblingRoom; }
            else { return m_notParentAndNotSiblingRoom; }
        }

        else
        {
            if (isParent && isSibling)
            {
                return m_parentAndSiblingRoom;
            }

            if (isParent && !isSibling)
            {
                return m_parentAndNotSiblingRoom;
            }

            if (!isParent && isSibling)
            {
                return m_notParentAndSiblingRoom;
            }


            if (!isParent && !isSibling)
            {
                return m_notParentAndNotSiblingRoom;
            }
        }

        Debug.Log("Can't find room to spawn");
        return null;
    }

    void CopyNetworkNodeInfoToClientNode(Node clientNode, Node architectNode) // sets the client node = to network one
    {
        clientNode.m_name = architectNode.m_name;
        clientNode.m_position = architectNode.m_position;
        clientNode.m_distanceToNextSibling = architectNode.m_distanceToNextSibling;
        clientNode.m_isFirstFolder = architectNode.m_isFirstFolder;
        clientNode.m_isParent = architectNode.m_isParent;
        clientNode.m_isSibling = architectNode.m_isSibling;
        clientNode.m_isSecondLast = architectNode.m_isSecondLast;

        clientNode.listOfFilesNames = architectNode.listOfFilesNames; // ????? maybe

    }

    void InstantiateCorridor(Node architectNode)
    {
        if (!architectNode.m_isRootFolder)
        {
            if (architectNode.m_isSibling)
            {
                if (architectNode.m_isFirstFolder)
                {
                    GenerateCorridor(architectNode, m_startCorridorPrefab);
                }

                else if (!architectNode.m_isLast)
                {
                    GenerateCorridor(architectNode, m_midCorridorPrefab);
                }

                else if (architectNode.m_isLast)
                {
                    GenerateCorridor(architectNode, m_endCorridorPrefab);
                }
            }
        }
    }

    void GenerateCorridor(Node currentNode, GameObject corridorToInstantiate)
    {
        Vector3 positionToGenerateCorridor = currentNode.m_position + new Vector3(0, 0, -m_sizeOfGapCorridorToRoom);
        Quaternion orientationOfTheCorridor = Quaternion.identity;

        int distance = currentNode.m_distanceToNextSibling;
        if (corridorToInstantiate != null)
        {
            if (corridorToInstantiate == m_startCorridorPrefab)
            {
                Instantiate(corridorToInstantiate, positionToGenerateCorridor, orientationOfTheCorridor); // instantiate first corridor

                for (int i = 1; i < distance * m_numberOfConnectionCorridorsNeeded; i++)
                {
                    Instantiate(m_connectionCorridorPrefab, positionToGenerateCorridor + new Vector3(i * m_corridorUnitLenght, 0, 0), orientationOfTheCorridor); // Instantiate connection corridor
                }
            }

            if (corridorToInstantiate == m_midCorridorPrefab)
            {
                Instantiate(corridorToInstantiate, positionToGenerateCorridor, orientationOfTheCorridor);
                for (int i = 1; i < distance * m_numberOfConnectionCorridorsNeeded; i++)
                {
                    Instantiate(m_connectionCorridorPrefab, positionToGenerateCorridor + new Vector3(i * m_corridorUnitLenght, 0, 0), orientationOfTheCorridor);
                }
            }

            if (corridorToInstantiate == m_endCorridorPrefab)
            {
                Instantiate(m_endCorridorPrefab, positionToGenerateCorridor, orientationOfTheCorridor);
            }
        }
    }
}