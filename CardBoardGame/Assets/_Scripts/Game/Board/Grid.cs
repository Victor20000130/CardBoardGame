using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    private GridData gridData;
    private Image _image;
    public Sprite gridSprite => _image.sprite;
    public GridData GridData
    {
        get { return gridData; }
        set { gridData = value; }
    }

    private void Awake()
    {
        // Initialize the grid data if needed
        gridData = new GridData();
        _image = transform.GetChild(0).GetComponent<Image>();
    }

}
