using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSelectionRect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rt;
    [SerializeField]
    private PopUpUI gridPopUp;
    IEnumerator gridPopUpEnumerator;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ManagerHandler.Instance.gameManager.GridInfosUIActive(true);
        gridPopUpEnumerator = ManagerHandler.Instance.gameManager.PopUpFollowMousePoint();
        StartCoroutine(gridPopUpEnumerator);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ManagerHandler.Instance.gameManager.GridInfosUIActive(false);
        StopCoroutine(gridPopUpEnumerator);
    }

}
