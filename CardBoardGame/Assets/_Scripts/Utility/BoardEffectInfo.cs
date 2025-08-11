using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardEffectInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    public Sprite sprite
    {
        get => image.sprite;
        set => image.sprite = value;
    }
    public GridData gridData;
    private IEnumerator gridFollowEnumerator;

    [SerializeField]
    private PopUpUI gridPopUp;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ManagerHandler.Instance.gameManager.SetGridInfos(gridData.Title, gridData.Info, sprite);
        ManagerHandler.Instance.gameManager.GridInfosUIActive(true);
        gridFollowEnumerator = ManagerHandler.Instance.gameManager.PopUpFollowMousePoint();
        StartCoroutine(gridFollowEnumerator);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ManagerHandler.Instance.gameManager.GridInfosUIActive(false);
        gridFollowEnumerator = ManagerHandler.Instance.gameManager.PopUpFollowMousePoint();
        StartCoroutine(gridFollowEnumerator);
    }
}
