using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviour
{

    public RectTransform _Rt;
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI infos;
    [SerializeField]
    private Image image;

    public void SetPopUpInfos(string title, string infos, Sprite sprite, Vector2 pivot)
    {
        this.title.text = title;
        this.infos.text = infos;
        this.image.sprite = sprite;
        _Rt.pivot = pivot;
    }

}
