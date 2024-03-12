using DG.Tweening;
using UnityEngine;


public class TabsUIController : MonoBehaviour {
    
    [Header("Custimize")]
    [SerializeField] private float _offsetXDeactivateTab = 0f;
    [SerializeField] private float _offsetXActiveTab = -30f;
    [Space] 
    
    [Header("Tabs Elements")]
    [SerializeField] private TabButtonUI1[] _tabButtons;
    [SerializeField] private GameObject[] _tabContent; 
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
            int index = i;
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
     
            // ActivateTabMover(_tabButtons[i].gameObject.GetComponent<RectTransform>(), i == currentTabIndex);
        }
    }

    private void ActivateTabMover(RectTransform tabButton, bool isActive)
    {
        float x = isActive ? _offsetXActiveTab : _offsetXDeactivateTab;

        Vector2 targetPosition = new Vector2(x, tabButton.anchoredPosition.y);

        tabButton.DOAnchorPos(targetPosition, 0.3f).SetEase(Ease.InOutQuad);
    }
    
}

