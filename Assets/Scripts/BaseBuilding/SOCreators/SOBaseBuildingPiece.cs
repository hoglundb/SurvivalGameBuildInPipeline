using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewBuildingPiece", menuName = "ScriptableObjects/BaseBuildingPiece", order = 1)]
public class SOBaseBuildingPiece : ScriptableObject
{
    [Header("The item category for filtering")]
    [SerializeField] public BaseBuilding.BaseBuildingPieceCategoriesEnum itemCategory;

    [Header("Prefab to be spawned when crafted")]
    [SerializeField] public GameObject piecePrefab;

    [Header("Ingredients required to craft this item")]
    [SerializeField] public List<Crafting.CraftingIngredient> ingredientsList;
}
