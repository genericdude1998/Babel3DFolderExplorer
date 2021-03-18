using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


public class LibraryArchitect : MonoBehaviour
{
    private int maxNumberOfFoldersAllowed = 299;
    private int maxNumberOfFilesInAFolderAllowed = 204;
    
    private ExceptionRaiser exceptionRaiser;

    public float sizeOfGapHorizontal = 12;// this is the distance on x axis between two neighbour rooms considering also the roof
    public float sizeOfGapVertical = 4.35f; // this is the distance on y axis 
    public float sizeOfGapCorridorFromRoom = 6.7f; // this is the distance from the centre on the room to the centre of the corridor

    public List<Node> generatedNodeList { get; private set; }

    [HideInInspector]
    public int roomWidth = 10;
    [HideInInspector]
    public int roomHeight = 10;
    [HideInInspector]
    public int corridorUnitLenght = 6;
    [HideInInspector]
    public int corridorUnitWidth = 6; // this is equivalent to the lenght of the side of the hexagon room


    private List<string> childrenOfRootFolderList;
    private int count = 0; // this has to be a global variable for it to work with more than 2 nested folders


    string rootFolderPath = string.Empty;
    [HideInInspector]
    public bool isBlueprintReady = false;

    public void CreateBlueprint()
    {
        System.DateTime start = System.DateTime.Now;

        exceptionRaiser = FindObjectOfType<ExceptionRaiser>();

        rootFolderPath = RootFolderPath.rootFolderPathString;

        CheckIfTheFolderIsValid(rootFolderPath);

        generatedNodeList = new List<Node>();
        childrenOfRootFolderList = Directory.GetDirectories(rootFolderPath).ToList<string>();
        RootFolderInit(rootFolderPath, childrenOfRootFolderList);
        GenerateLibrary(childrenOfRootFolderList, Vector3.zero); count = 0;
        isBlueprintReady = true;


        System.DateTime end;
        end = System.DateTime.Now;
        Debug.Log("Architect ended in" + end.Subtract(start).ToString());


        if (generatedNodeList.Count > maxNumberOfFoldersAllowed)
        {
            exceptionRaiser.maxNumberOfFoldersReached = true;
        }   // folders


        foreach (Node node in generatedNodeList) // files
        {
            if (node.listOfFilesNames.Count > maxNumberOfFilesInAFolderAllowed) 
            {
                exceptionRaiser.maxNumberOfFilesReached = true;
                break;
            }
        }
    }

    void RootFolderInit(string rootFolderPath, List<string> childrenOfRootFolderList)
    {
        if (childrenOfRootFolderList.Count > 0)
        {
            Node rootNode = new Node(rootFolderPath, sizeOfGapVertical * Vector3.up, 0, false, false, true, false, false, true);
            PopulateNodeFileList(rootNode, rootFolderPath);
            generatedNodeList.Add(rootNode);
        }

        else
        {
            Node rootNode = new Node(rootFolderPath, sizeOfGapVertical * Vector3.up, 0, false, false, false, false, false, true);
            PopulateNodeFileList(rootNode, rootFolderPath);
            generatedNodeList.Add(rootNode);
        }
    }

    void GenerateLibrary(List<string> listOfFoldersAtTheSameLevel, Vector3 pointer)  // IMPORTANT EVERYTIME YOU USE THIS FUNCTION YOU HAVE TO SET GLOBAL COUNT TO 0
    {
        List<string> currentList = listOfFoldersAtTheSameLevel;
        List<string> nextList = new List<string>();

        foreach (string nameOfTheCurrentFolder in currentList)
        {

            CheckIfTheFolderIsValid(nameOfTheCurrentFolder);

            bool isFolderAccessible = Directory.Exists(nameOfTheCurrentFolder);

            if (!isFolderAccessible) // if a folder is not accessible
            {
                exceptionRaiser.folderExceptionRaised = true; // RAISE EXCEPTION
            }


            // these are booleans for the current folder
            bool m_isFirstFolder = false;
            bool m_isSibling = false;
            bool m_isParent = false;

            // these are for stopping generating more rooms when is last and not generating more corridors when is secondlast
            bool m_isLast = false;
            bool m_isSecondLast = false;

            int m_distanceFirstFolderToNextSibling = 0;
            string m_nameOfNextSiblingFolder;


            CheckFolderBooleans(nameOfTheCurrentFolder, currentList, ref m_isFirstFolder, ref m_isSibling, ref m_isParent, ref m_isSecondLast, ref m_isLast);

            m_distanceFirstFolderToNextSibling = GetDistanceToNextSibling(nameOfTheCurrentFolder); count = 0;  // this is used for first siblings as a current node basis

            if (m_isFirstFolder)
            {
                Node firstNode = new Node(nameOfTheCurrentFolder, pointer, m_distanceFirstFolderToNextSibling, m_isFirstFolder, m_isSibling, m_isParent, m_isSecondLast, m_isLast, false);
                PopulateNodeFileList(firstNode, nameOfTheCurrentFolder);
                generatedNodeList.Add(firstNode);
            }

            if (m_isParent)
            {
                nextList.Clear();
                nextList.AddRange(Directory.GetDirectories(nameOfTheCurrentFolder).ToList());

                Vector3 nextPointer = pointer - new Vector3(0, sizeOfGapVertical, 0);
                GenerateLibrary(nextList, nextPointer);

            }

            if (m_isSibling) // this is all for the next room coming in the hierarchy and therefore requires new pointer, distance and booleans
            {
                if (!m_isLast)
                {
                    pointer += new Vector3(m_distanceFirstFolderToNextSibling * sizeOfGapHorizontal, 0, 0);

                    m_nameOfNextSiblingFolder = currentList[currentList.IndexOf(nameOfTheCurrentFolder) + 1];

                    CheckIfTheFolderIsValid(m_nameOfNextSiblingFolder);

                    int m_distanceSiblingToNextFolder = GetDistanceToNextSibling(m_nameOfNextSiblingFolder); count = 0; // this is used for siblings not first as a next node basis

                    bool m_isNextRoomFirstFolder = false;
                    bool m_isNextRoomSibling = false;
                    bool m_isNextRoomParent = false; //booleans for the next folder
                    bool m_isNextRoomSecondLast = false;
                    bool m_isNextRoomLast = false;

                    CheckFolderBooleans(m_nameOfNextSiblingFolder, currentList, ref m_isNextRoomFirstFolder, ref m_isNextRoomSibling, ref m_isNextRoomParent, ref m_isNextRoomSecondLast, ref m_isNextRoomLast);

                    Node nextNode = new Node(m_nameOfNextSiblingFolder, pointer, m_distanceSiblingToNextFolder, m_isNextRoomFirstFolder, m_isNextRoomSibling, m_isNextRoomParent, m_isNextRoomSecondLast, m_isNextRoomLast, false);
                    PopulateNodeFileList(nextNode, m_nameOfNextSiblingFolder);
                    generatedNodeList.Add(nextNode);


                }
            }
        }
    }
    int GetDistanceToNextSibling(string rootFolder) // everytime you use this function in this class you must reset manually the count = 0
    {
        string[] subfolders = Directory.GetDirectories(rootFolder);
        //Debug.Log(subfolders.Length);
        if (subfolders.Length > 0)
        {
            foreach (string element in subfolders)
            {
                if (Directory.GetDirectories(element).Length == 0) { count++; }
                else { GetDistanceToNextSibling(element); }
            }
        }

        else { count++; }
        return count;
    }

    void CheckFolderBooleans(string currentFolder, List<string> currentList, ref bool isFirstFolder, ref bool isSibling, ref bool isParent, ref bool isSecondLastSibling, ref bool isLastSibling)
    {
        if (currentFolder == currentList.First()) { isFirstFolder = true; } // is it the first folder on the level?
        if (Directory.GetDirectories(currentFolder).Length > 0) { isParent = true; } // is it a parent?
        if (currentList.Count > 1) { isSibling = true; } // is it a sibling?
        if (currentList[currentList.Count - 1] == currentFolder) { isLastSibling = true; } //Is it the last of the siblings on the level?
        if (currentList.Count >= 2)
        {
            if (currentList[currentList.Count - 2] == currentFolder) { isSecondLastSibling = true; } //Is it the last of the siblings on the level? is this even needed?
        }
        else { isSecondLastSibling = false; }
    }

    void PopulateNodeFileList(Node targetNode, string targetFolderPath) // this fills the file list contained in the folder
    {
        string[] filesPath = Directory.GetFiles(targetFolderPath);
        foreach (string filePath in filesPath)
        {
            targetNode.listOfFilesNames.Add(Path.GetFileName(filePath));
        }
    }

    void CheckIfTheFolderIsValid(string nameOfTheFolder)
    {
        if (!Directory.Exists(nameOfTheFolder)) // if a folder is not accessible
        {
            exceptionRaiser.folderExceptionRaised = true; // RAISE EXCEPTION
        }
    }
}

