using DG.Tweening;
using UnityEngine;

namespace Extensions
{
        public static class RectTransformExtensions
        {
            public enum Direction
            {
                Up,
                Down,
                Left,
                Right
            }
            
            public static void AnimateFromOutsideToPosition(this RectTransform rectTransform, Vector2 originalPosition, Direction direction, float duration = 0.3f, Ease ease = Ease.OutQuad, System.Action callback = null)
            {
                Vector2 outsideStartPosition = GetScreenPosition(rectTransform, direction);
                rectTransform.anchoredPosition = outsideStartPosition;
                
                rectTransform.DOAnchorPos(originalPosition, duration).SetEase(ease).OnComplete(() => callback?.Invoke());
            }
            
            public static void AnimateBackOutsideScreen(this RectTransform rectTransform, Direction direction, float duration = 0.3f, Ease ease = Ease.OutQuad, System.Action callback = null)
            {
                var startPosition = rectTransform.anchoredPosition;
                Vector2 targetPosition = GetScreenPosition(rectTransform, direction);
                
                rectTransform.DOAnchorPos(targetPosition, duration).SetEase(ease).OnComplete(() =>
                {
                    rectTransform.anchoredPosition = startPosition;
                    callback?.Invoke();
                });
            }
            
            private static Vector2 GetScreenPosition(RectTransform rectTransform, Direction direction)
            {
                Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
                if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                {
                    Debug.LogError("Canvas must be in Screen Space - Overlay mode for correct calculations.");
                    return Vector2.zero;
                }

                float width = canvas.GetComponent<RectTransform>().sizeDelta.x;
                float height = canvas.GetComponent<RectTransform>().sizeDelta.y;
                Vector2 currentPosition = rectTransform.anchoredPosition;

                switch (direction)
                {
                    case Direction.Up:
                        return new Vector2(currentPosition.x, height / 2 + rectTransform.sizeDelta.y);
                    case Direction.Down:
                        return new Vector2(currentPosition.x, -(height / 2 + rectTransform.sizeDelta.y));
                    case Direction.Left:
                        return new Vector2(-(width / 2 + rectTransform.sizeDelta.x / 2), currentPosition.y);
                    case Direction.Right:
                        return new Vector2(width / 2 + rectTransform.sizeDelta.x / 2, currentPosition.y);
                    default:
                        return currentPosition;
                }
            }
        }
    
}