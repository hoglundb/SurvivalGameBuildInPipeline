using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaterialSlot : MonoBehaviour, IGroupableItemUISlot
{
    [SerializeField] SOMaterial _soInventoryItem;

    [SerializeField] private GameObject _quantityTextGameObj;
    private Text _quantityTextElement;

    private void Awake()
    {
        _quantityTextElement = _quantityTextGameObj.GetComponent<Text>();
    }


    public void UpdateItemQuantityText()
    {
        _quantityTextElement.text = _soInventoryItem.quantity.ToString();
    }
}
