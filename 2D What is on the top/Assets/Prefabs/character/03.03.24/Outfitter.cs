using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Outfitter : MonoBehaviour
{
    [SerializeField] private List<SpriteResolver> resolvers;

    private int currentCategoryIndex = 0;
    private string[] categories = 
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
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeSkin();
        }



    }

    private void ChangeSkin()
    {
        if (currentCategoryIndex >= categories.Length)
            currentCategoryIndex = 0;

        Debug.Log(currentCategoryIndex);

        foreach (var resolver in resolvers)
        {
            Debug.Log($"category - {resolver.GetCategory() }  label - {resolver.GetLabel()}");
            resolver.SetCategoryAndLabel(categories[currentCategoryIndex], resolver.GetLabel());
        }
        
        currentCategoryIndex++;
    }
}

