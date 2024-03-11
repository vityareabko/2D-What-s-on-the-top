

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TabsUIController : MonoBehaviour {
    
    [Header("Tabs Customization")]
    [SerializeField] private Color _deactiveCollor = Color.gray;
    [SerializeField] private Color _activeTabCollor = Color.gray;
    // [SerializeField] private Color _contentCollor = Color.gray;
    [SerializeField] private Image _frameContent;
    [SerializeField] private Image _uiTabPanel;
    [Space]
    
    [Header("Tabs Elements")]
    [SerializeField] private TabButtonUI1[] _tabButtons; // Settable in Inspector
    [SerializeField] private GameObject[] _tabContent; // Settable in Inspector
    [Space] 
    
    private int currentTabIndex = 0;

    private void Awake() => InitializeTabs();
    
    private void InitializeTabs() 
    {
        if (_tabButtons.Length != _tabContent.Length) 
        {
            Debug.LogError("Number of Buttons is not the same as number of Contents (" + _tabButtons.Length + " buttons & " + _tabContent.Length + " contents)");
            return;
        }

        for (int i = 0; i < _tabButtons.Length; i++)
        {
            int index = i; // Local copy for lambda capture
            _tabButtons[i].TabButton.onClick.AddListener(() => OnTabButtonClicked(index));
            _tabContent[i].SetActive(i == currentTabIndex);
        }

        UpdateUIState();
    }

    public void OnTabButtonClicked(int tabIndex) 
    {
        if (currentTabIndex != tabIndex) 
        {
            _tabContent[currentTabIndex].SetActive(false);
            _tabContent[tabIndex].SetActive(true);
            
            currentTabIndex = tabIndex;
            UpdateUIState();
        }
    }

    private void UpdateUIState() {
        for (int i = 0; i < _tabButtons.Length; i++)
        {
            _tabButtons[i].TabButton.interactable = (i != currentTabIndex);
            _tabButtons[i].ActiveTabArrow.gameObject.SetActive(i == currentTabIndex);
            // Вызываем ActivateTabMover с правильным флагом активности для каждого таба.
            ActivateTabMover(_tabButtons[i].gameObject.GetComponent<RectTransform>(), i == currentTabIndex);
        }
    }

    private void ActivateTabMover(RectTransform tabButton, bool isActive)
    {
        // Если таб активен, его позиция должна быть (0, y, 0), иначе (56, y, 0).
        float x = isActive ? 0f : 56f;

        // Новая позиция для RectTransform.
        Vector2 targetPosition = new Vector2(x, tabButton.anchoredPosition.y);

        // Анимация движения таба с использованием DOTween.
        tabButton.DOAnchorPos(targetPosition, 0.3f).SetEase(Ease.InOutQuad);
    }
    
}

