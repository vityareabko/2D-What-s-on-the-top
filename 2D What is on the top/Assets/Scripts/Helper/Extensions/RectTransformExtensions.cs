using DG.Tweening;
using UnityEngine;

namespace Extensions
{
    public static class RectTransformExtensions
    {
        //
        // public static void AnimateToPosition(this RectTransform rectTransform, Vector2 targetPosition, float duration = 0.3f, Ease ease = Ease.OutQuad, bool flipY = true, bool flipX = true, System.Action callback = null)
        // {
        //     rectTransform = FlipPosition(rectTransform, flipX, flipY); // - IMPORTANT: - moved to flip position cause we need to animatied rectTransform to original position
        //     rectTransform.DOAnchorPos(targetPosition, duration).SetEase(ease).OnComplete(() => callback?.Invoke());
        // }
        //
        // public static void AnimateToHidePosition(this RectTransform rectTransform, Vector2 targetPosition, float duration = 0.3f, Ease ease = Ease.OutQuad, bool flipY = true, bool flipX = true, System.Action callback = null)
        // {
        //     rectTransform.DOAnchorPos(targetPosition, duration).SetEase(ease).OnComplete(() =>
        //     {
        //         rectTransform = FlipPosition(rectTransform, flipX, flipY);
        //         callback?.Invoke();
        //     });
        // }
        //
        // private static RectTransform FlipPosition(RectTransform rectTransform, bool flipX,bool flipY)
        // {
        //     var anchorPos = rectTransform.anchoredPosition;
        //
        //     if (flipY) anchorPos.y *= -1f;
        //     if (flipX) anchorPos.x *= -1f;
        //     
        //     rectTransform.anchoredPosition = anchorPos;
        //     return rectTransform;
        // }
        //
        
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
            
            rectTransform.DOAnchorPos(originalPosition, duration).SetEase(Ease.OutQuad).OnComplete(() => callback?.Invoke());
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

        // private static Vector2 GetStartPositionOutsideScreen(RectTransform rectTransform, Direction direction)
        // {
        //     Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
        //     
        //     if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        //     {
        //         Debug.LogError("Canvas must be in Screen Space - Overlay mode for correct outside start positions.");
        //         return Vector2.zero;
        //     }
        //
        //     float width = canvas.GetComponent<RectTransform>().sizeDelta.x;
        //     float height = canvas.GetComponent<RectTransform>().sizeDelta.y;
        //
        //     switch (direction)
        //     {
        //         case Direction.Up:
        //             return new Vector2(0, height / 2 + rectTransform.sizeDelta.y);
        //         case Direction.Down:
        //             return new Vector2(0, -(height / 2 + rectTransform.sizeDelta.y));
        //         case Direction.Left:
        //             return new Vector2(-(width / 2 + rectTransform.sizeDelta.x), 0);
        //         case Direction.Right:
        //             return new Vector2(width / 2 + rectTransform.sizeDelta.x, 0);
        //         default:
        //             return rectTransform.anchoredPosition;
        //     }
        // }

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

            switch (direction)
            {
                case Direction.Up:
                    return new Vector2(0, height / 2 + rectTransform.sizeDelta.y);
                case Direction.Down:
                    return new Vector2(0, -(height / 2 + rectTransform.sizeDelta.y));
                case Direction.Left:
                    return new Vector2(-(width / 2 + rectTransform.sizeDelta.x), 0);
                case Direction.Right:
                    return new Vector2(width / 2 + rectTransform.sizeDelta.x, 0);
                default:
                    return rectTransform.anchoredPosition;
            }
        }
    }
    
}