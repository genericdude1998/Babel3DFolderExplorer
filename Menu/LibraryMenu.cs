using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LibraryMenu : MonoBehaviour
{
    private RawImage background;
    private Text content;
    private InputField inputField;

    private bool finishedPrinting = false;
    private bool hasConnected = false;
  
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 90;

        Screen.fullScreen = true;

        Cursor.lockState = CursorLockMode.None;

        background = GetComponentInChildren<RawImage>();
        content = GetComponentInChildren<Text>();
        inputField = GetComponentInChildren<InputField>();

        string textToPrint = " BabelDrive(C)  All Rights Reserved 2021 \n\r" + 
            " \n\r Welcome to Babel Drive \n\r Press Enter to run the Program \n\r \n\r -> Press Enter to Start or Escape to Exit\n\r \n\r /run BabelDrive.exe \n\r";
        content.text = string.Empty;
        inputField.gameObject.SetActive(false);


        StartCoroutine(WriteTextLetterByLetter(textToPrint));
    }

    private void OnGUI()
    {
        background.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        content.fontSize = Screen.height / 60;
    }
    // Update is called once per frame
    void Update()
    {
        if (finishedPrinting)
        {
            if (Input.GetKeyDown(KeyCode.Return) && !hasConnected)
            {
               // StopCoroutine(dashCoroutine); 
                content.text += "\n\r Connecting... \n\r";
                string textToPrint = " " + "\n\r BabelDrive login accepted current time:  \n\r" + "\n\r " + System.DateTime.Now + "\n\r" + "\n\r" + " UserName: " + System.Environment.MachineName + "\n\r" + "\n\r" +
                        "Babel Drive is a 3D Folder Explorer that allows you to look at your files in a new light \n\r \n\r" + "Enter the library root folder then press enter! Or Press Escape to Exit" + "\n\r" + "\n\r" +"\n\r";

                StartCoroutine(WriteTextLetterByLetter(textToPrint));

                inputField.gameObject.SetActive(true);

                hasConnected = true;
            }

            if (Input.GetKeyDown(KeyCode.Return) && inputField.text.Length > 0)
            {
                content.text += "\n\r" + "\n\r" + "Generating Library this may take a few moments...";
                RootFolderPath.rootFolderPathString = inputField.text;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   
            }

            if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        }
    }

    IEnumerator WriteTextLetterByLetter(string textToBePrinted)
    {
        finishedPrinting = false;

        foreach (char c in textToBePrinted)
        {
            content.text += c;
            yield return null;
          
        }

        finishedPrinting = true;
    }
}
