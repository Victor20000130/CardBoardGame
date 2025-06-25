using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private Button _button;
    public Button Button
    {
        get { return _button; }
    }
    private bool isClicked;
    public ColorBlock clickedColors;
    public void OnClick()
    {
        isClicked = !isClicked;
        if (isClicked)
        {
            _button.colors = clickedColors;
        }
        else
        {
            _button.colors = ColorBlock.defaultColorBlock;
        }
    }
    public void Initialize()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }
}

