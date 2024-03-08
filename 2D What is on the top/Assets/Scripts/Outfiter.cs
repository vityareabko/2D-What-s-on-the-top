using System;
using System.Collections.Generic;
using UI.MainMenu.ShopSkinsScreen;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;

public class Outfitter : MonoBehaviour
{
    [FormerlySerializedAs("resolvers")] [SerializeField] private List<SpriteResolver> _resolvers;

    private int _currentCategoryIndex = 0;
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

    private void OnEnable()
    {
        EventAggregator.Subscribe<TryOnSkinEvent>(OnTryOnSkinHandler);
        EventAggregator.Subscribe<ApplySelectedSkinEvent>(OnApplyCurrentSkin);
    }
    
    private void OnDisable()
    {
        EventAggregator.Unsubscribe<TryOnSkinEvent>(OnTryOnSkinHandler);
        EventAggregator.Unsubscribe<ApplySelectedSkinEvent>(OnApplyCurrentSkin);
    }

    private void OnApplyCurrentSkin(object sender, ApplySelectedSkinEvent eventData) => ChangeSkinByType(eventData.CurrentSkin);
    
    private void OnTryOnSkinHandler(object sender, TryOnSkinEvent eventData) => ChangeSkinByType(eventData.TypeSkin);

    private void ChangeSkinByType(ShopSkinType type)
    {
        foreach (var resolver in _resolvers)
        {
            resolver.SetCategoryAndLabel(type.ToString(), resolver.GetLabel());
        }
    }













    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeSkin();
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

///
/// как я хочу чтобы было :
/// я хочу чтобы у меня был скрипт типо примерка скинов, он ни коем образом не меняет выбраный скин только после покупки становится выбраным
///
/// Что у меня есть :
/// у меня есть скприпт который меняет по нажатия клавищы скины
///
///
/// Что я могу сделать :
/// я могу сделать скрипт в котором будет доставаться из данных выбраный скин и потом после выхода из примерочной то меняем скин который был выбран
///
///
///
/// 