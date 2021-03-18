using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExceptionRaiser : MonoBehaviour
{
    Canvas exceptionCanvas;
    RawImage exceptionBackground;
    Text exceptionContent;
    Button ReturnToMenuButton;

    Canvas exitCanvas;
    RawImage exitBackground;
    Text exitContent;
    Button yes;
    Button no;


    public bool exceptionRaised = false;
    public bool folderExceptionRaised = false;
    public bool maxNumberOfFoldersReached = false;
    public bool maxNumberOfFilesReached = false;



    public void ExceptionRaiserInit()
    {
        exceptionCanvas = GetComponentsInChildren<Canvas>()[0];
        exceptionBackground = GetComponentsInChildren<RawImage>()[0];
        exceptionContent = GetComponentsInChildren<Text>()[0];
        ReturnToMenuButton = GetComponentsInChildren<Button>()[0];
        ReturnToMenuButton.onClick.AddListener(ReturnToMenu);

        exceptionCanvas.enabled = false;

        exitCanvas = GetComponentsInChildren<Canvas>()[1];
        exitBackground = GetComponentsInChildren<RawImage>()[1];
        exitContent = GetComponentsInChildren<Text>()[2];
        yes = GetComponentsInChildren<Button>()[1];
        yes.onClick.AddListener(Application.Quit);
        no = GetComponentsInChildren<Button>()[2];
        no.onClick.AddListener(GoBackToTheGame);
    }

    private void OnGUI()
    {
        if (exceptionCanvas.enabled == true)
        {
            exceptionBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            exceptionContent.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        if (exitCanvas.enabled == true)
        {
            exitBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            exitContent.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        }

    }


    public void CheckIfExceptionIsRaised()
    {
        if(folderExceptionRaised || maxNumberOfFilesReached || maxNumberOfFoldersReached) { exceptionRaised = true; }
    }

    public void Update()
    {
        if (folderExceptionRaised) 
        {
           Camera.main.farClipPlane = 0.1f; // to black out all the objects infront fo the camera except the UI
           Cursor.lockState = CursorLockMode.None;
           Cursor.lockState = CursorLockMode.Confined;
           exceptionCanvas.enabled = true;
           exceptionCanvas.GetComponentInChildren<Text>().text = string.Empty;
           exceptionCanvas.GetComponentInChildren<Text>().text = "A folder in your library is not accessible or does not exist please return to the library menu";
           folderExceptionRaised = false;

        }

       else if (maxNumberOfFoldersReached)
       {
           Camera.main.farClipPlane = 0.1f; // to black out all the objects infront fo the camera except the UI
           Cursor.lockState = CursorLockMode.None;
           Cursor.lockState = CursorLockMode.Confined;
           exceptionCanvas.enabled = true;
           exceptionCanvas.GetComponentInChildren<Text>().text = string.Empty;
           exceptionCanvas.GetComponentInChildren<Text>().text = "You have reached the max number of folders (300) reduce the folder count, please return to the library menu";
           maxNumberOfFoldersReached = false;

       }

       else if (maxNumberOfFilesReached)
       {
           Camera.main.farClipPlane = 0.1f; // to black out all the objects infront fo the camera except the UI
           Cursor.lockState = CursorLockMode.None;
           Cursor.lockState = CursorLockMode.Confined;
           exceptionCanvas.enabled = true;
           exceptionCanvas.GetComponentInChildren<Text>().text = string.Empty;
           exceptionCanvas.GetComponentInChildren<Text>().text = "You have reached the max number of files in one folder (204) reduce the file count, please return to the library menu";
           maxNumberOfFoldersReached = false;

       }

       else if (maxNumberOfFilesReached && maxNumberOfFoldersReached)
       {
           Camera.main.farClipPlane = 0.1f; // to black out all the objects infront fo the camera except the UI
           Cursor.lockState = CursorLockMode.None;
           Cursor.lockState = CursorLockMode.Confined;
           exceptionCanvas.enabled = true;
           exceptionCanvas.GetComponentInChildren<Text>().text = string.Empty;
           exceptionCanvas.GetComponentInChildren<Text>().text = "You have reached the max number of files in one folder (204) and the max number of folders (300), please return to the library menu";
           maxNumberOfFoldersReached = false;

       }

       if (Input.GetKeyDown(KeyCode.Escape))
       {
            Camera.main.farClipPlane = 0.1f; // to black out all the objects infront fo the camera except the UI
            Cursor.lockState = CursorLockMode.Confined;
            exitCanvas.enabled = true;
       }
    }

    void ReturnToMenu() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void GoBackToTheGame() 
    {
        exitCanvas.enabled = false;
        Camera.main.farClipPlane = 1000;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
