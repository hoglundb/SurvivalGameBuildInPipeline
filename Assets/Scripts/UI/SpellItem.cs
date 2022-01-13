using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    //Represents a spell item in the Inventory UI
    public class SpellItem : MonoBehaviour
    {
        [SerializeField] private SpellScriptableObjectCreator _spellInfo;  //Scriptable object that contains the spell info

        private SpellDetailsManager _spellDetails;  //Globally reference the spell details UI. We update with this spells info if this item is clicked. 

        //Register onclick handler for this item. 
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnThisItemClick);
        }


        private void Start()
        {
            //Singleton reference to the spell details manager class
            _spellDetails = SpellDetailsManager.GetInstance();
        }


        //Called when user clicks the button associated with this UI element. When this happens, display the details for this spell in the spell details UI section.
        private void OnThisItemClick()
        {
            _spellDetails.UpdateText(_spellInfo.name, _spellInfo.description, _spellInfo.ingredients);
        }
    }
}

