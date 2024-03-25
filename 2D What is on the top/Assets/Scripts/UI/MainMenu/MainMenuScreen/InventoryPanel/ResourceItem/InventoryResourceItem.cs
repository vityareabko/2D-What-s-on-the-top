using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryResourceItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [FormerlySerializedAs("_amount")] [SerializeField] private TMP_Text _amountText;
    [SerializeField] private Image _frameAmount;

    private ResourceTypes _type;
    private ResourceCategory _category;
    private int _amount;
    
    private Color _colorNormal = Color.white;
    private Color _colorRare;
    private Color _colorEpic;
    private Color _colorLegendary;
    
    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#008BC8", out _colorRare);
        ColorUtility.TryParseHtmlString("#FF14AD", out _colorEpic);
        ColorUtility.TryParseHtmlString("#FF9D14", out _colorLegendary);
    }
    
    private void OnValidate() => UpdateColorByType(_type);

    public void Initialize(int amount, ResourceTypes ResourceType)
    {
        _amount = amount;
        SetTextResourceAmount(amount);
        SetResourceCategory(ResourceType);
        SetResourceIcon();
        UpdateColorByType(_type);
    }

    public int Amount => _amount;

    public ResourceCategory Category => _category;
    
    private void SetResourceIcon() => _icon.sprite = ResourceService.LoadSpriteByType(_type);
    
    private void SetTextResourceAmount(int amount) => _amountText.text = amount.ToString();

    private void SetResourceCategory(ResourceTypes type) => _type = type;
    
    private void UpdateColorByType(ResourceTypes type)
    {
        _category = ResourceDistributor.GetCategoryResourceByResourceType(type);
            
        switch (_category)
        {
            case ResourceCategory.Normal:
                _frameAmount.color = _colorNormal;
                break;
            case ResourceCategory.Rare:
                _frameAmount.color = _colorRare;
                break;
            case ResourceCategory.Epic:
                _frameAmount.color = _colorEpic;
                break;
            case ResourceCategory.Legendary:
                _frameAmount.color = _colorLegendary;
                break;
            case ResourceCategory.Common:
                _frameAmount.color = _colorNormal;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}