using System;
using DG.Tweening;
using UnityEngine;

public class LoadingGameScreen : MonoBehaviour
{
    [SerializeField] private RectTransform _loadingBG;
    [SerializeField] private RectTransform _point;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _loadingTime = 1f;
    
    [SerializeField] private float _delay = 0.3f ;
    [SerializeField] private float _step = 44.5f;
    
    private float _timer;

    public float LoadingTime => _loadingTime;

    private void Awake()
    {
        Hide();
        _loadingBG.transform.SetAsLastSibling();
     
        var acnchor = _point.anchoredPosition;
        acnchor.x = _point.sizeDelta.x / 2;
        _point.anchoredPosition = acnchor;
    }

    private void OnEnable()
    {
        _timer = 0;
        _canvasGroup.alpha = 1f;
        // _canvasGroup.DOFade(1f, 3f).SetEase(Ease.Linear);
    }

    private void Update()
    {
        var anchor = _point.anchoredPosition;

        if (_timer < _delay)
        {
            _timer += Time.deltaTime;
            return;
        } 
        UpdateLoadingBar(anchor);
    }

    private void UpdateLoadingBar(Vector2 anchor)
    {
        
        if (_point.anchoredPosition.x >= 256 - _point.sizeDelta.x)
            anchor.x = _point.sizeDelta.x / 2;
        else
            anchor.x += _step;

        _point.anchoredPosition = anchor;
        _timer = 0f;
    }

    public void HideSmooth(Action callback)
    {
        if (_canvasGroup == null)
            return;
        
        _canvasGroup.alpha = 1;
        
        _canvasGroup.DOFade(0f, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            callback?.Invoke();
            gameObject.SetActive(false);
        });
    }
    
    

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}
