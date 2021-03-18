using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class LibraryTerminal : BoxDispatcher
{
    public GameObject boxCanvasPrefab;
    
    private Node roomNode;

    private Text monitorText;
    private Text pageNumberText;
    public List<TerminalInteractible> interactiblesList;
    private TerminalInteractible previousButton;
    private TerminalInteractible nextButton;
    
    private int m_maxPageIndex;
    private int m_maxNumberOfLinesPerPage = 18;
    private int m_maxNumberOfCharsPerLine = 97; // this the maximun amount of characters per line - the ... characters(3)

    private int m_currentPageIndex = 0;

    public void LibraryTerminalInit()
    {
        roomNode = gameObject.GetComponentInParent<Node>();
        DispatchBoxes(roomNode);

        Text[] texts = gameObject.GetComponentsInChildren<Text>();
        monitorText = texts[0];
        pageNumberText = texts[1];

        interactiblesList = gameObject.GetComponentsInChildren<TerminalInteractible>().ToList();

        previousButton = interactiblesList[0];
        nextButton = interactiblesList[1];

        previousButton.type = InteractibleType.previousPageButton;
        nextButton.type = InteractibleType.nextPageButton;


        for (int i = 2; i < interactiblesList.Count; i++)
        {
            interactiblesList[i].type = InteractibleType.fileBox; // this sets all the boxes to type box
        }


        int filesCount = roomNode.listOfFilesNames.Count;
        m_maxPageIndex = Mathf.FloorToInt(filesCount / m_maxNumberOfLinesPerPage);

        OpenFileNamePage(0); // this has to be last
    }

    private void Update() // maybe use a different game loop? instead of GUI
    {
        foreach (TerminalInteractible button in interactiblesList)
        {
            if (button.isButtonPressed) 
            {
                InteractibleType type = button.type;
                switch (type)
                {
                    case InteractibleType.previousPageButton: OpenPreviousPage(ref m_currentPageIndex);
                        break;
                    case InteractibleType.nextPageButton: OpenNextPage(ref m_currentPageIndex);
                        break;
                    case InteractibleType.fileBox: OpenBoxMenu(button);
                        break;
                    case InteractibleType.boxMenuOpen:OpenFile(button);
                        break;
                    case InteractibleType.boxMenuClose:CloseBoxMenu(button);
                        break;
                    default:
                        break;
                }

                button.isButtonPressed = false;

                break;
            }
        }

    }

    void OpenBoxMenu(TerminalInteractible btn) 
    {
        Vector3 pos = btn.transform.position + (-btn.transform.forward * 1);
        float xRot = btn.transform.rotation.eulerAngles.x;
        float yRot = btn.transform.rotation.eulerAngles.x;
        float zRot = btn.transform.rotation.eulerAngles.x;

       // Vector3 orientationEuler = new Vector3(btn.transform.rotation.eulerAngles.x, btn.transform.rotation.eulerAngles.y, btn.transform.rotation.eulerAngles.z);
       
        BoxMenu boxMenu = Instantiate(boxCanvasPrefab, pos, btn.transform.rotation).GetComponent<BoxMenu>(); // probably have a script attached to the box canvas itself

        boxMenu.transform.parent = btn.transform; // set it child to the box

        boxMenu.BoxMenuInit();
        
    }

    void OpenFile(TerminalInteractible btn) 
    {
        string filename = btn.GetComponentInParent<FileBox>().m_fileName;
        string path = roomNode.m_name;

        string file = path + @"\" + filename;

        Application.OpenURL(file);
    }

    void CloseBoxMenu(TerminalInteractible btn)
    {
        btn.GetComponentInParent<BoxMenu>().DestroyBoxMenu();
    }
    
    void OpenNextPage(ref int currentPageIndex) 
    {
        if (currentPageIndex != m_maxPageIndex)
        {
            OpenFileNamePage(currentPageIndex + 1);
            currentPageIndex++;
        }
    }

    void OpenPreviousPage(ref int currentPageIndex)
    { 
        if (currentPageIndex != 0)
        {
            OpenFileNamePage(currentPageIndex - 1);
            currentPageIndex--;
        }
    }


    void OpenFileNamePage(int pageindex) 
    {
        string fileNamesToDisplay = string.Empty;

        if (pageindex > m_maxPageIndex) { return; }

        //check if to disable the buttons they could be put in the following if statements but for readability is better like this

        if (pageindex == 0)
        {
            previousButton.gameObject.SetActive(false); //  maybe a better way to disable the button?
        }

        else 
        {
            previousButton.gameObject.SetActive(true); 
        }

        if (pageindex == m_maxPageIndex)
        {
            nextButton.gameObject.SetActive(false); //  maybe a better way to disable the button?
        }

        else 
        {
            nextButton.gameObject.SetActive(true);
        }


        if (pageindex == m_maxPageIndex)
        {
            for (int i = pageindex * m_maxNumberOfLinesPerPage; i < roomNode.listOfFilesNames.Count; i++)
            {
                if (roomNode.listOfFilesNames[i].Length > m_maxNumberOfCharsPerLine)
                {
                    fileNamesToDisplay += "\n\r" + roomNode.listOfFilesNames[i].Substring(0, m_maxNumberOfCharsPerLine) + "...";
                }

                else
                {
                    fileNamesToDisplay += "\n\r" + roomNode.listOfFilesNames[i];
                }
            }
        }

        else 
        {
            for (int i = pageindex * m_maxNumberOfLinesPerPage; i < (pageindex * m_maxNumberOfLinesPerPage) + m_maxNumberOfLinesPerPage; i++)
            {
                if (roomNode.listOfFilesNames[i].Length > m_maxNumberOfCharsPerLine)
                {
                    fileNamesToDisplay += "\n\r" + roomNode.listOfFilesNames[i].Substring(0, m_maxNumberOfCharsPerLine) + "...";
                }

                else
                {
                    fileNamesToDisplay += "\n\r" + roomNode.listOfFilesNames[i];
                }
            }
        }

        monitorText.text = string.Format("Welcome to the Babel Terminal System©\n\r\n\rThis folder contains {0} file(s):\n\r-> {1}", roomNode.listOfFilesNames.Count, fileNamesToDisplay);
        pageNumberText.text = string.Format("Page {0} of {1}", (pageindex + 1).ToString(), (m_maxPageIndex + 1).ToString());
    }
}
