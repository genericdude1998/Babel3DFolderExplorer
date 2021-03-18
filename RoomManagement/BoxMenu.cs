using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoxMenu : MonoBehaviour
{
    private TerminalInteractible[] interactiblesArr;
    private Text fileNameText;
    private Coroutine textPrintingCoroutine;
    public void BoxMenuInit()
    {
        Canvas boxMenuCanvas = GetComponentInChildren<Canvas>();
        boxMenuCanvas.worldCamera = Camera.main;

        string fileName = GetComponentInParent<FileBox>().m_fileName;

        fileNameText = GetComponentInChildren<Text>(); // the first text

        fileNameText.text = string.Empty;

        textPrintingCoroutine = StartCoroutine(WriteFileName(fileName));

        interactiblesArr = GetComponentsInChildren<TerminalInteractible>();

        interactiblesArr[0].type = InteractibleType.boxMenuOpen;
        interactiblesArr[1].type = InteractibleType.boxMenuClose;


        LibraryTerminal terminal = GetComponentInParent<LibraryTerminal>(); // add the buttons to the terminal list
        terminal.interactiblesList.AddRange(interactiblesArr);

        GridCell cell = GetComponentInParent<GridCell>();
        cell.SelectCell(); // makes the cell red
    }


    public void DestroyBoxMenu() 
    {
        LibraryTerminal terminal = GetComponentInParent<LibraryTerminal>(); // remove the button as well as the menu

        foreach (TerminalInteractible interactible in interactiblesArr)
        {
            terminal.interactiblesList.Remove(interactible);
        }

        GridCell cell = GetComponentInParent<GridCell>();
        cell.DeselectCell(); // makes the cell green

        textPrintingCoroutine = null;

        Destroy(this.gameObject);

    }

    private IEnumerator WriteFileName(string textToBePrinted) 
    {
        foreach (char c in textToBePrinted)
        {
            fileNameText.text += c;
            yield return null;
        }

        StopCoroutine(textPrintingCoroutine);
    }
}
