using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Outfitter : MonoBehaviour
{
    [SerializeField] private List<SpriteResolver> _resolvers;
    
    [SerializeField] private SpriteRenderer _shieldSkin;

    [SerializeField] private SpriteResolver _resolverShieldSkin;
    
    private void OnEnable()
    {
        EventAggregator.Subscribe<TryOnSkinEvent>(OnTryOnSkinHandler);
        EventAggregator.Subscribe<TryOnShieldSkinEvent>(OnTryOnShieldSkinHandler);
        EventAggregator.Subscribe<ApplySelectedHeroSkinEvent>(OnApplyCurrentSkin);
        EventAggregator.Subscribe<ApplySelectedShieldSkinEvent>(OnApplyCurrentShielSkin);
    }
    
    private void OnDisable()
    {
        EventAggregator.Unsubscribe<TryOnSkinEvent>(OnTryOnSkinHandler);
        EventAggregator.Unsubscribe<TryOnShieldSkinEvent>(OnTryOnShieldSkinHandler);
        EventAggregator.Unsubscribe<ApplySelectedHeroSkinEvent>(OnApplyCurrentSkin);
        EventAggregator.Unsubscribe<ApplySelectedShieldSkinEvent>(OnApplyCurrentShielSkin);
    }

    private void ChangeHerSkinSetByType(ShopSkinType type)
    {
        foreach (var resolver in _resolvers)
        {
            resolver.SetCategoryAndLabel(type.ToString(), resolver.GetLabel());
        }
    }


    private void OnApplyCurrentSkin(object sender, ApplySelectedHeroSkinEvent eventData) => ChangeHerSkinSetByType(eventData.CurrentSkin);
    
    private void OnTryOnSkinHandler(object sender, TryOnSkinEvent eventData) => ChangeHerSkinSetByType(eventData.Skin);

    
    

    private void ChangeShieldSkinByType(ShopSkinType type) =>
        _resolverShieldSkin.SetCategoryAndLabel(type.ToString(), _resolverShieldSkin.GetLabel());
    
    private void OnApplyCurrentShielSkin(object sender, ApplySelectedShieldSkinEvent eventData) => ChangeShieldSkinByType(eventData.CurrentShieldSkin);
    
    private void OnTryOnShieldSkinHandler(object sender, TryOnShieldSkinEvent eventData) => ChangeShieldSkinByType(eventData.Skin);












    
    
    
    
    
    
    
    private int _currentCategoryIndex = 0;
    private int _currentShieldIndex = 0;
    private string[] _categories = 
    {
        "FlameWarrior",
        "GreenWarrior",
        "KnightSky",
        "SacredGuardian",
        "SilentShuriken",
        "WildKin",
        "DesertNomad",
        "CrimsonWarrior",
        "VerdanKeeper",
        "DragonsHonor",
        "DwellerArmor",
        "WarlockArmor",
    };

    [SerializeField] private List<Sprite> _spritesShields;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeSkin();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_currentShieldIndex >= _spritesShields.Count)
                _currentShieldIndex = 0;
            
            _shieldSkin.sprite = _spritesShields[_currentShieldIndex];
            _currentShieldIndex++;
        }
    }

    private void ChangeSkin()
    {
        if (_currentCategoryIndex >= _categories.Length)
            _currentCategoryIndex = 0;

        Debug.Log(_currentCategoryIndex);

        foreach (var resolver in _resolvers)
        {
            Debug.Log($"category - {resolver.GetCategory() }  label - {resolver.GetLabel()}");
            resolver.SetCategoryAndLabel(_categories[_currentCategoryIndex], resolver.GetLabel());
        }
        
        _currentCategoryIndex++;
    }
     
}

