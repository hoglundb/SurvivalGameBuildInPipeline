using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Items
{
    /*
     This component is on any prefab that is a melee item. It messages the player to move with the player's hand and trigger the correct animations.
     */
    public class MeleeItemController : AbstractEquipableItem
    {

        public override void Update()
        {
         //   Debug.LogError(_playerParentControllerComponent.playerMovementComponent.IsMovementInabled());
            //don't do anything if player movement is not enabled. 
            if (!_playerParentControllerComponent.playerMovementComponent.IsMovementInabled()) return;

            //invoke the base class that moves the weapon in accordance to the player's hand bone parent
            base.Update();
  
            if (!_isEquiped) return;

            //Respond to player input to perform malee actions. 
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Player.PlayerControllerParent.GetInstance().SetAnimationTrigger("MeleeAxe");
                StartCoroutine(_MeleeActionCoroutine());
            }
        }


        public override void Equip()
        {
            //Call the base class that parents this to the player's hand
            base.Equip();
            GetComponent<MeleeItemController>().enabled = true;
            Player.PlayerControllerParent.GetInstance().SetAnimationTrigger("EquipAxe");           
        }


        private IEnumerator _MeleeActionCoroutine()
        {
            yield return new WaitForSeconds(_equipableItemInfo._actionDelay);
            _OnWeaponStrike();

        }


        //Called when the weapon swing is fully extended and can apply damage to something. 
        private void _OnWeaponStrike()
        {
            //Get the game object the player is looking at
            Transform lookAtGameObj = _playerParentControllerComponent.playerEnvironmentDetectorComponent.GetLookAtGameObject();
            if (lookAtGameObj == null) return;

            IMeleeAbleItem meleeableComponent = lookAtGameObj.GetComponent<IMeleeAbleItem>();
            if (meleeableComponent == null) return;

            meleeableComponent.TakeDamage(10);
        }
    }
}

