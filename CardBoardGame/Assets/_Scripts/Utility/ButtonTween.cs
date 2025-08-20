using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleFactor = 1.2f; // Scale factor when hovered
    [SerializeField] private float duration = 0.2f; // Duration of the tween effect
    private RectTransform m_RectTransform;
    private readonly float originalScale = 1f;
    private int originalRenderIndex = 0;
    private float originalYPos;
    public float moveYvalue = 0;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        m_RectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        transform.DOScale(originalScale, 0);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            return;
        }
        // Scale up the button when hovered
        transform.DOScale(scaleFactor, duration).SetEase(Ease.OutBack);

        originalRenderIndex = m_RectTransform.GetSiblingIndex();
        m_RectTransform.SetSiblingIndex(m_RectTransform.parent.childCount - 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            return;
        }
        // Scale back to original size when not hovered
        transform.DOScale(originalScale, duration).SetEase(Ease.OutBack);
        m_RectTransform.SetSiblingIndex(originalRenderIndex);
    }
}

