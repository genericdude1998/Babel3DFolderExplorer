using UnityEngine;

 public class BoxDispatcher : MonoBehaviour
 {
    public GameObject boxPrefab;
    protected GridCell[] grid;
    protected void DispatchBoxes(Node roomNode) 
    {
        grid = GetComponentsInChildren<GridCell>();
   
        Room currentRoom = GetComponentInParent<Room>();
        int filesCount = roomNode.listOfFilesNames.Count;

        float yRot = 0.0f;

        for (int i = 0; i < grid.Length; i++)
        {
            if (i < filesCount)
            {
                if (i <= grid.Length)
                {
                    if (i % 51 == 0)
                    {
                        if (yRot == 120)
                        {
                            yRot = -yRot;
                        }

                        else { yRot += 60.0f; }
                    }

                    Quaternion rotation = Quaternion.Euler(0, yRot, 0);

                    FileBox box = Instantiate(boxPrefab, grid[i].transform.position, rotation).GetComponent<FileBox>(); // check the rotation
                    box.FileBoxInit(roomNode.listOfFilesNames[i]);
                    box.gameObject.transform.parent = grid[i].gameObject.transform;

                  
                    grid[i].CellInit(true);
                }
            }

            else 
            {
                grid[i].CellInit(false); // this is over the files amount so make them not occupied
            }
        }
    }
 }
