
using UnityEngine;




public class TabsUI : MonoBehaviour {
    [Header("Tabs Customization")]
    [SerializeField] private Color themeColor = Color.gray;
    [Space]
    
    [Header("Tabs Elements")]
    [SerializeField] private TabButtonUI[] tabBtns; // Settable in Inspector
    [SerializeField] private GameObject[] tabContent; // Settable in Inspector
    [Space]

    private int currentTabIndex = 0;

    private void Awake() 
    {
        ValidateTabs();
        InitializeTabs();
    }

    private void InitializeTabs() 
    {
        if (tabBtns.Length != tabContent.Length) 
        {
            Debug.LogError("Number of Buttons is not the same as number of Contents (" + tabBtns.Length + " buttons & " + tabContent.Length + " contents)");
            return;
        }

        for (int i = 0; i < tabBtns.Length; i++)
        {
            int index = i; // Local copy for lambda capture
            tabBtns[i].uiButton.onClick.AddListener(() => OnTabButtonClicked(index));
            tabContent[i].SetActive(i == currentTabIndex);
        }

        UpdateUIState();
    }

    private void OnTabButtonClicked(int tabIndex) 
    {
        if (currentTabIndex != tabIndex) 
        {
            tabContent[currentTabIndex].SetActive(false);
            tabContent[tabIndex].SetActive(true);

            currentTabIndex = tabIndex;
            UpdateUIState();
        }
    }

    private void UpdateUIState() {
        for (int i = 0; i < tabBtns.Length; i++) 
        {
            tabBtns[i].uiButton.interactable = (i != currentTabIndex);
            tabBtns[i].uiImage.color = (i == currentTabIndex) ? themeColor : DarkenColor(themeColor, 0.2f);
        }
    }

    private Color DarkenColor(Color color, float amount) 
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        v = Mathf.Max(0, v - amount);
        return Color.HSVToRGB(h, s, v);
    }

    protected void ValidateTabs() 
    {
        UpdateUIState();
    }
}

