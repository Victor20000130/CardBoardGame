using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayButtonInfos : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [TextArea(2, 5)]
    public string title;
    [TextArea(2, 5)]
    public string infos;
    [SerializeField]
    private Image image;
    private IEnumerator popUpFollowEnumerator;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ManagerHandler.Instance.gameManager.PopUpActivation(true);
        popUpFollowEnumerator = ManagerHandler.Instance.gameManager.PopUpFollowMousePoint();
        StartCoroutine(popUpFollowEnumerator);
        ManagerHandler.Instance.gameManager.SetPopUpInfos(title, infos, image.sprite, new Vector2(0, 0));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ManagerHandler.Instance.gameManager.PopUpActivation(false);
        StopCoroutine(popUpFollowEnumerator);
    }
}
