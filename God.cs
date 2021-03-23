using UnityEngine;
using System;

public class God : Singleton<God>
{
    public GameObject architectGO;
    private LibraryArchitect architect;
    public GameObject builderGO;
    private LibraryBuilder builder;
    public GameObject exceptionRaiserGO;
    ExceptionRaiser exceptionRaiser;

    public override void Awake()
    {
        exceptionRaiser = FindObjectOfType<ExceptionRaiser>();
        exceptionRaiser.ExceptionRaiserInit();

        architect = Instantiate(architectGO).GetComponent<LibraryArchitect>();

        try
        {
            architect.CreateBlueprint();// this generates a blueprint
        }

        catch (UnauthorizedAccessException) 
        {
            Debug.Log("ACCESS DENIED!");
        }

        exceptionRaiser.folderExceptionRaised = true;
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
}
