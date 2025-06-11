using UnityEngine;

public class Grid : MonoBehaviour
{
    private GridData gridData;

    public GridData GridData
    {
        get { return gridData; }
        set { gridData = value; }
    }

    private void Awake()
    {
        // Initialize the grid data if needed
        gridData = new GridData();
    }

}
