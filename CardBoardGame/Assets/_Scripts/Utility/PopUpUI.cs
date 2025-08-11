using TMPro;
using UnityEngine;

public class PopUpUI : MonoBehaviour
{

    public RectTransform _Rt;
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI infos;

    private void Awake()
    {
        _Rt = GetComponent<RectTransform>();
    }
    public void SetPopUpInfos(string title, string infos, Sprite sprite, Vector2 pivot)
    {
        this.title.text = title;
        this.infos.text = infos;
        _Rt.pivot = pivot;
    }

}
