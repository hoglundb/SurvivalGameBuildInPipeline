using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Magic
{
    public class WandController : MonoBehaviour
    {
        [SerializeField] private Transform _wandTip;
        private GlobalResourceManager _globalResourceManager;

        private void Awake()
        {
            _globalResourceManager = GlobalResourceManager.GetInstance();
        }

        //Called by the player when their character is shooting a spell.
        public void ShootSpell(Vector3 dir, int spellTypeIndex = 1)
        {
            //Get the spell game obj from the obj pool in the globalResourceManager
            GameObject spell = GlobalResourceManager.GetInstance().GetSpell(spellTypeIndex);
            SpellBehavior spellBehaviorComponent = spell.GetComponent<SpellBehavior>();

            spellBehaviorComponent.LaunchSpell(_wandTip.position, dir);
        }
    }
}

