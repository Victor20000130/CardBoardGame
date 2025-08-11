using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected GridData gridData;
    protected Image _image;
    public Sprite gridSprite => _image.sprite;
    public GridData GridData
    {
        get { return gridData; }
        set { gridData = value; }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ManagerHandler.Instance.gameManager.SetGridInfos(gridData.Title, gridData.Info, _image.sprite);

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        ManagerHandler.Instance.gameManager.SetGridInfos("", "", null);
    }

    protected virtual void Awake()
    {
        // Initialize the grid data if needed
        _image = transform.GetChild(0).GetComponent<Image>();
    }

}
