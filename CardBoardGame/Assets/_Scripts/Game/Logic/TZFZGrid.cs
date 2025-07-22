using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TZFZGrid : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private Image img;
    public TextMeshProUGUI Tmp
    {
        get => tmp;
        set => tmp = value;
    }
    public Color Color
    {
        get => img.color;
        set => img.color = value;
    }

    public void Init()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        img = GetComponent<Image>();
        Tmp.text = "";
    }
}
