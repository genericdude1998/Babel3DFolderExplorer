using UnityEngine;

public class TerminalInteractible : MonoBehaviour
{
    
    public bool isButtonPressed = false;
    public InteractibleType type;

   public void PressButton() 
    {
        isButtonPressed = true;
    }
}

public enum InteractibleType 
{ 
    previousPageButton,
    nextPageButton,
    fileBox,
    boxMenuOpen,
    boxMenuClose
}
