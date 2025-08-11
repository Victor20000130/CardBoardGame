using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridSelection : BoardGrid
{
    [SerializeField]
    private GameObject marbleUI_On;

    private Button button;

    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnGridButtonClicked);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    private void OnGridButtonClicked()
    {
        ManagerHandler.Instance.gameManager.ReceiveDiceValue(gridData.Idx);
        ManagerHandler.Instance.gameManager.MarbleUIOff();
    }
}
