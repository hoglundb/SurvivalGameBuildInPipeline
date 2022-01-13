using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Inventory", menuName = "SpellItem")]
public class SpellScriptableObjectCreator : ScriptableObject
{
    [SerializeField] public string spellName;
    [SerializeField] public List<CraftingIngredient> ingredients;

    [Multiline] [SerializeField]  public string description;
}
