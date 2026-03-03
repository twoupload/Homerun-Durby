using UnityEngine;

// 카메라 관련 좌표 변환 유틸리티 클래스
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-04

namespace Util
{
    public static class CameraTranform 
    {
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // 월드 좌표를 UI 캔버스의 로컬 좌표로 변환하는 함수
        ///////////////////////////////////////////////////////////////
        public static Vector3 WorldToScreenSpaceCamera(Camera worldCamera, Camera canvasCamera, RectTransform canvasRectTransform, Vector3 worldPosition)
        {
            // 월드 좌표를 스크린 좌표로 변환
            var screenPoint = RectTransformUtility.WorldToScreenPoint(cam: worldCamera, worldPoint: worldPosition);
            // 스크린 좌표를 캔버스 RectTransform의 로컬 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect: canvasRectTransform, screenPoint: screenPoint, cam: canvasCamera, localPoint: out var localPoint);

            return localPoint;
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // UI 요소(RectTransform)의 위치를 월드 좌표로 변환하는 함수
        ///////////////////////////////////////////////////////////////
        public static Vector3 ScreenToWorldPointCamera(Camera canvasCamera, RectTransform rect)
        {
            // RectTransform의 위치를 스크린 좌표로 변환
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvasCamera, rect.position);

            // 스크린 좌표를 월드 좌표로 변환
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPos, canvasCamera, out var result);

            return result;
        }
    }
}
