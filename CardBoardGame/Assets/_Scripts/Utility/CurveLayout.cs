using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CurveLayout : MonoBehaviour
{
    private RectTransform[] childrenRectTrans;
    private int childCount = 0;
    private RectTransform _rect;
    [SerializeField]
    private HorizontalLayoutGroup _horizontalLayoutGroup;
    public float moveYvalue;
    public bool isReverse;
    public float radius = 1000f;
    public float startAngle = 120f;
    public float endAngle = 60f;
    public float upSideDeg = 90f;
    public bool isInLobby = false;
    private void Awake()
    {

        if (isReverse)
        {
            upSideDeg *= -1f;
            startAngle *= -1f;
            endAngle *= -1f;
        }
        childCount = transform.childCount;
        childrenRectTrans = new RectTransform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            childrenRectTrans[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }

        _rect = GetComponent<RectTransform>();

        SetCurveLayout();
        if (!isInLobby)
        {
            _rect.position += new Vector3(0, moveYvalue, 0);
        }
    }
    public IEnumerator SortLayout()
    {
        _horizontalLayoutGroup.enabled = true;
        _horizontalLayoutGroup.enabled = false;
        yield return null;
        SetCurveLayout();
    }
    public void SetCurveLayout()
    {

        childCount = 0;
        foreach (RectTransform rect in childrenRectTrans)
        {
            if (rect.gameObject.activeSelf == false)
            {
                break;
            }
            childCount++;
        }

        for (int i = 0; i < childCount; i++)
        {
            childrenRectTrans[i] = transform.GetChild(i).GetComponent<RectTransform>();
            childrenRectTrans[i].anchorMax.Set(0.5f, 0.5f);
            childrenRectTrans[i].anchorMin.Set(0.5f, 0.5f);
            childrenRectTrans[i].pivot.Set(0.5f, 0.5f);

        }

        Vector2 parentCenter = _rect.rect.center;

        for (int i = 0; i < childCount; i++)
        {
            // 보간값 t: 0~1
            float t = (childCount == 1) ? 0.5f : (float)i / (childCount - 1);
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad) * radius + parentCenter.x;
            float y = Mathf.Sin(rad) * radius + parentCenter.y;

            childrenRectTrans[i].anchoredPosition = new Vector2(x, y);
            // UI의 위쪽이 원의 바깥을 향하도록 회전
            childrenRectTrans[i].rotation = Quaternion.Euler(0, 0, angle - upSideDeg);

            if (isReverse == false)
            {
                childrenRectTrans[i].localPosition -= new Vector3(0, radius, 0);
            }
            else
            {
                childrenRectTrans[i].localPosition += new Vector3(0, radius, 0);
            }
        }
    }
}
