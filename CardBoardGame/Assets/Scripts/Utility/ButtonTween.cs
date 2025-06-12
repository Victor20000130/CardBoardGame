using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleFactor = 1.2f; // Scale factor when hovered
    [SerializeField] private float duration = 0.2f; // Duration of the tween effect
    private float originalScale = 1f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Scale up the button when hovered
        transform.DOScale(scaleFactor, duration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Scale back to original size when not hovered
        transform.DOScale(originalScale, duration).SetEase(Ease.OutBack);
    }
}

