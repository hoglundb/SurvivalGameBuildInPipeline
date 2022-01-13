using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class to manage the inventory button tabs (crafting, placeables, spells ) based on the player action

public class InventoryMenuController : MonoBehaviour
{
    //The transforms that contain the tab buttons for the player's inventory
    [Header("Tab Btn References")] 
    [SerializeField] private Transform _craftingTabBtnTransform;
    [SerializeField] private Transform _placablesTabBtnTransform;
    [SerializeField] private Transform _spellsTabBtnTransform;

    //Btn colors
    [Header("Btn Action Colors")]
    [SerializeField] private Color _tabSelectedBtnColor;
    [SerializeField] private Color _tabUnselectedBtnColor;

    //Reference the UI panels that coorispond to the tab buttons. 
    [Header("Panel References")]
    [SerializeField] private Transform _panelCrafting;
    [SerializeField] private Transform _panelPlacables;
    [SerializeField] private Transform _panelSpells;

    //referenes to the tab buttons themseves;
    private Button _btnCraftingTab;
    private Button _btnPlacablesTab;
    private Button _btnSpellsTab;


    private void Awake()
    {
        //Initialize tab btn referecnes
        _btnCraftingTab = _craftingTabBtnTransform.GetComponent<Button>();
        _btnPlacablesTab = _placablesTabBtnTransform.GetComponent<Button>();
        _btnSpellsTab = _spellsTabBtnTransform.GetComponent<Button>();

        //Register the tab btn event handlers;
        _btnCraftingTab.onClick.AddListener(_OnCraftingTabBtnClick);
        _btnPlacablesTab.onClick.AddListener(_OnPlacablesTabBtnClick);
        _btnSpellsTab.onClick.AddListener(_OnSpellsTabBtnClick);

        //Set the crafting button to be the default selected tab by simulating a click on it. 
        _ResetTabButtons();
        _OnCraftingTabBtnClick();
    }


    //Called when user clicks the Crafting tab button. Brings up the crafting panel and hides any other panels. 
    private void _OnCraftingTabBtnClick()
    {
        _ResetTabButtons();
        _DisableAllInventoryPanels();
        _btnCraftingTab.GetComponent<Image>().color = _tabSelectedBtnColor;
        _panelCrafting.gameObject.SetActive(true);
    }


    //Called when user clicks the placables tab button. Brings up the placables panel and hides any other panels. 
    private void _OnPlacablesTabBtnClick()
    {
        _ResetTabButtons();
        _DisableAllInventoryPanels();
        _btnPlacablesTab.GetComponent<Image>().color = _tabSelectedBtnColor;
        _panelPlacables.gameObject.SetActive(true);
    }


    //Called when user clicks the spells tab button. Brings up the spells panel and hides any other panels. 
    private void _OnSpellsTabBtnClick()
    {
        _ResetTabButtons();
        _DisableAllInventoryPanels();
        _btnSpellsTab.GetComponent<Image>().color = _tabSelectedBtnColor;
        _panelSpells.gameObject.SetActive(true);
    }


    //Sets all tab button colors to their unselected color. Call this prior to setting a button as selected on a spefic button.
    private void _ResetTabButtons()
    {
        _btnCraftingTab.GetComponent<Image>().color = _tabUnselectedBtnColor;
        _btnPlacablesTab.GetComponent<Image>().color = _tabUnselectedBtnColor;
        _btnSpellsTab.GetComponent<Image>().color = _tabUnselectedBtnColor;
    }


    //Disables all the inventory panels. Call this to disable all panels prior to enabling a specific panel. 
    private void _DisableAllInventoryPanels()
    {
        _panelCrafting.gameObject.SetActive(false);
        _panelPlacables.gameObject.SetActive(false);
        _panelSpells.gameObject.SetActive(false);
    }


}
