using UnityEngine;

public class GridCell : MonoBehaviour
{
    public bool m_isOccupied = false;
    private MeshRenderer meshRend;
    public Material selectedMat;
    public Material defaultMat;

    public void CellInit(bool isOccupied) 
    {
        meshRend = GetComponent<MeshRenderer>();
        defaultMat = meshRend.material;
        m_isOccupied = isOccupied;

        if (!m_isOccupied)
        { 
            gameObject.SetActive(false);
            //Debug.Log(meshRend.enabled);
        }
    }

    public void SelectCell() 
    {
        meshRend.material = selectedMat;
    }

    public void DeselectCell() 
    {
        meshRend.material = defaultMat;

    }

}
