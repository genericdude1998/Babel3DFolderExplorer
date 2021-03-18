using UnityEngine;

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
        architect.CreateBlueprint(); // this generates a blueprint
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
