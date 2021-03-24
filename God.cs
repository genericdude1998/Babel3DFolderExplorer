using UnityEngine;
using System;

public class God : Singleton<God>
{
    public GameObject architectGO;
    private LibraryArchitect architect;
    public GameObject builderGO;
    private LibraryBuilder builder;
    public GameObject exceptionRaiserGO;
    private ExceptionRaiser exceptionRaiser;

    private int maxNumberOfFoldersAllowed = 299;
    private int maxNumberOfFilesInAFolderAllowed = 204;

    public override void Awake()
    {
        architect = Instantiate(architectGO).GetComponent<LibraryArchitect>();
        exceptionRaiser = FindObjectOfType<ExceptionRaiser>();
        exceptionRaiser.ExceptionRaiserInit();

        CheckLibraryGenerationExceptions(); // this starts library generation procedures and also checks for exceptions
        CheckLibraryGenerationLimitations(architect); // checks if number of files and folders are within limits
    }

    private void Start()
    {
        exceptionRaiser.CheckIfExceptionIsRaised();

        if (!exceptionRaiser.exceptionRaised)
        {
            builder = Instantiate(builderGO).GetComponent<LibraryBuilder>();
            builder.Build(); // this gets the architect and reads the blueprint
        }
    }

    private void CheckLibraryGenerationExceptions() 
    {
        try // check if generation causes exceptions
        {
            architect.CreateBlueprint();// this generates a blueprint
        }

        catch (Exception) // this ensures both unauthorisedexceptions and directorynotfound exception are catched
        {
            exceptionRaiser.folderExceptionRaised = true;
        }

    }

    private void CheckLibraryGenerationLimitations(LibraryArchitect arch) 
    {
        if (arch.generatedNodeList.Count > maxNumberOfFoldersAllowed)
        {
            exceptionRaiser.maxNumberOfFoldersReached = true;
        }   

        foreach (Node node in arch.generatedNodeList) // files
        {
            if (node.listOfFilesNames.Count > maxNumberOfFilesInAFolderAllowed)
            {
                exceptionRaiser.maxNumberOfFilesReached = true;
                break;
            }
        }
    }
}
