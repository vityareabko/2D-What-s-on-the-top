using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStatItem : MonoBehaviour
{
    public event System.Action<PlayerStatType> UpgradeButtonClicked;
    
    [field: SerializeField] public PlayerStatType StatType { get; private set; }
    
    [Header("Title")]
    // [SerializeField] private TMP_Text _statName;
    [SerializeField] private TMP_Text _statLevel;

    [Header("Content")] 
    [SerializeField] private Image _resourceIcon;
    [SerializeField] private TMP_Text _priceUpgrade;

    [SerializeField] private TMP_Text _currentStatLevelValue;
    [SerializeField] private TMP_Text _nextStatLevelValue;
    
    [SerializeField] private Button _button;
    
    private string _statLevelFormat;
    
    private void Awake() => _button.onClick.AddListener(OnClickButton);
    
    private void OnDestroy() => _button.onClick.RemoveListener(OnClickButton);

    public void UpdateStatData(int statLevel, Sprite spriteResource, int priceToUpgrade, float currentLevelStatValue, float? nextLevelStatVaule)
    {
        if (_statLevelFormat == null) _statLevelFormat = _statLevel.text;
        
        _resourceIcon.sprite = spriteResource;
        
        _statLevel.text =  string.Format(_statLevelFormat, statLevel.ToString());
        
        _priceUpgrade.text = priceToUpgrade.ToString();
        _currentStatLevelValue.text = currentLevelStatValue.ToString();
        
        if (nextLevelStatVaule == null)
            _nextStatLevelValue.text = "MAX";
        else
            _nextStatLevelValue.text = nextLevelStatVaule.ToString();
    }
    
    private void OnClickButton() => UpgradeButtonClicked?.Invoke(StatType);

}
