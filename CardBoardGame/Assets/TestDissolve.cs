using UnityEngine;
using UnityEngine.UI;

public class TestDissolve : MonoBehaviour
{

    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private Material _material;
    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _material.SetTexture("_MainTex", sprite.texture);
        _image.material = _material;

    }
}
