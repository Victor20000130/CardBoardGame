using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardEffectInfo : MonoBehaviour
{
    public Camera renderCam;              // RawImage를 비추는 카메라
    public RawImage rawImage;             // UI 상에서 보이는 RawImage
    public EventSystem eventSystem;
    public GraphicRaycaster raycaster;    // renderCam과 연결된 Canvas의 GraphicRaycaster

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print(1);
            // 마우스 위치가 RawImage 안인지 확인
            if (!RectTransformUtility.RectangleContainsScreenPoint(rawImage.rectTransform, Input.mousePosition, renderCam))
                return;
            print(2);
            // RawImage에서의 마우스 좌표 → RenderTexture 상의 좌표로 변환
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rawImage.rectTransform, Input.mousePosition, null, out localPoint);
            print(3);
            // RawImage의 pivot 고려하여 정규화된 위치로 변환 (0~1)
            Vector2 normalized = Rect.PointToNormalized(rawImage.rectTransform.rect, localPoint);

            // RenderTexture 상의 픽셀 좌표 계산
            Vector2 renderTexPos = new Vector2(
                normalized.x * renderCam.pixelWidth,
                normalized.y * renderCam.pixelHeight
            );
            print(4);
            // 이벤트 데이터 구성
            PointerEventData eventData = new PointerEventData(eventSystem);
            eventData.position = renderTexPos;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);
            print(5);
            foreach (var result in results)
            {
                print(000);
                Debug.Log("UI Hit: " + result.gameObject.name);
                // 여기에 이벤트 처리 추가
            }
            print(6);
        }
    }
}
