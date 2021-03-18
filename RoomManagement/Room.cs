using UnityEngine.UI;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Node roomNode;
    private LibraryTerminal roomTerminal;

    public Canvas roomUIInside;
    private Text folderNameBanner;

    public Canvas corridorUICanvas;
    private Text folderNameBannerOutside;

    public int m_occlusionStackIndex;
    public OcclusionCullingStackManager m_stackManager;


    public void RoomInit(OcclusionCullingStackManager stackManager, int stackIndex, bool isToBeRenderedAtStart)
    {
        m_occlusionStackIndex = stackIndex;
        m_stackManager = stackManager;

        // occlusion culling init
        roomNode = GetComponent<Node>();
        roomTerminal = GetComponentInChildren<LibraryTerminal>();

        roomTerminal.LibraryTerminalInit(); // this also contains the dispatch boxes command
      
        foreach (Canvas canvas in GetComponentsInChildren<Canvas>())
        {
            canvas.worldCamera = Camera.main;
            if (canvas.name == "RoomUIInside") { roomUIInside = canvas; }
            if (canvas.name == "CorridorUICanvas") { corridorUICanvas = canvas; }
        }

        // set the name of folder
        folderNameBanner = roomUIInside.GetComponentsInChildren<Text>()[0];
        folderNameBanner.fontSize = 2;
        folderNameBanner.text = roomNode.m_name;

        folderNameBannerOutside = corridorUICanvas.GetComponentsInChildren<Text>()[0];
        folderNameBannerOutside.fontSize = 2;
        folderNameBannerOutside.text = roomNode.m_name;

        folderNameBannerOutside = corridorUICanvas.GetComponentsInChildren<Text>()[1];
        folderNameBannerOutside.fontSize = 2;
        folderNameBannerOutside.text = m_stackManager.occlusionCullingStackManagerIndex.ToString();

        if (!isToBeRenderedAtStart) { CullRoom(); }
    }


    private void OnTriggerEnter(Collider collider)
    {
        m_stackManager.currentRoomIndex = this.m_occlusionStackIndex;
        m_stackManager.enteringRoom = true;
    }

    public void RenderRoom()
    {
        ActivateRenderers<Renderer>(true);
        ActivateColliders<Collider>(true);
        ActivateBehaviour<Canvas>(true);
        ActivateBehaviour<CanvasScaler>(true);
        ActivateBehaviour<LibraryTerminal>(true);
    }

    public void CullRoom()
    {
        ActivateRenderers<Renderer>(false);
        ActivateColliders<Collider>(false);
        ActivateBehaviour<Canvas>(false);
        ActivateBehaviour<CanvasScaler>(false);
        ActivateBehaviour<LibraryTerminal>(false);
        DestroyBoxMenus();
    }

    private void ActivateBehaviour<T>(bool active) where T:Behaviour
    {
        foreach (T component in GetComponentsInChildren<T>())
        {
            if (!component.gameObject.CompareTag("IgnoreStackCulling"))
            {
                component.enabled = active;
            }
        }
    }
    private void ActivateRenderers<T>(bool active) where T : Renderer
    {
        foreach (T component in GetComponentsInChildren<T>())
        {
            if (!component.gameObject.CompareTag("IgnoreStackCulling"))
            {
                component.enabled = active;
            }
        }
    }

    private void ActivateColliders<T>(bool active) where T : Collider
    {
        foreach (T component in GetComponentsInChildren<T>())
        {
            if (!component.gameObject.CompareTag("IgnoreStackCulling"))
            {
                component.enabled = active;
            }
        }
    }

    private void DestroyBoxMenus()  
    {
        foreach (BoxMenu boxMenu in GetComponentsInChildren<BoxMenu>())
        {
            boxMenu.DestroyBoxMenu();
        }
    }
}
