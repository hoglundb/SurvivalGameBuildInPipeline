using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    //A global class for displaying the spell details in the inventory UI panel. 
    public class SpellDetailsManager : MonoBehaviour
    {
        [SerializeField] [Multiline] private string _defaultText;

        //reference the UI that contains each textbox
        [SerializeField] private Transform _spellNameUIElement;
        [SerializeField] private Transform _spellDescriptionUIElement;
        [SerializeField] private Transform _spellIngredientsUIElement;

        //reference the text boxes that display the spell info
        private Text _textSpellName;
        private Text _textSpellDescription;
        private Text _textSpellIngredients;


        private static SpellDetailsManager _instance;


        public static SpellDetailsManager GetInstance()
        {
            return _instance;
        }


        private void Awake()
        {
            //Init the references to the actual text boxes that contain the spell info
            _textSpellName = _spellNameUIElement.GetComponent<Text>();
            _textSpellDescription = _spellDescriptionUIElement.GetComponent<Text>();
            _textSpellIngredients = _spellIngredientsUIElement.GetComponent<Text>();

            //Setup the singleton reference
            _instance = this;

            //Clear the text out and put the default text in here
            ClearText();
        }


        //Clears all the text from the spell details text boxes.
        public void ClearText()
        {
            _textSpellName.text = string.Empty;
            _textSpellDescription.text = string.Empty;
            _textSpellIngredients.text = string.Empty;
        }


        //Updates the spell details text fields with the provided info. Called when player clicks to view details on a spell in the inventory. 
        public void UpdateText(string spellName, string spellDescription, List<CraftingIngredient> spellIngredientsList)
        {
            ClearText();

            _textSpellName.text = spellName;
            _textSpellDescription.text = spellDescription;

            string spellIngredientsText = "Requires ";
            foreach (var i in spellIngredientsList)
            {
                spellIngredientsText += i.quantity.ToString() + " " + i.ingredient + ", ";
            }
            _textSpellIngredients.text = spellIngredientsText;
        }
    }
}

