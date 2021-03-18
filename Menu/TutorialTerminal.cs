using UnityEngine.UI;
using UnityEngine;

public class TutorialTerminal : MonoBehaviour
{
    LibraryArchitect architect;
    Text content;


    private void Start()
    {
        architect = FindObjectOfType<LibraryArchitect>();
        content = GetComponentInChildren<Text>();

        int numberOfFolders = architect.generatedNodeList.Count;
        int numberOfFiles = 0;

        foreach (Node node in architect.generatedNodeList) 
        {
            foreach (string file in node.listOfFilesNames)
            {
                numberOfFiles++;
            }
        }


        content.text += string.Format("This is the {0} Babel Library: \n\r" + "\n\r It currently contains {1} folder(s) and {2} files \n\r Use the WASD keys to move and the mouse to click. Press Esc to leave", RootFolderPath.rootFolderPathString, numberOfFolders, numberOfFiles);
    }


}
