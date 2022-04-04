using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Component to be present on any game object that the player can use as a melee weapon. 
/// Contains the meta-data pertaining the the particular melee item. 
/// </summary>
[RequireComponent(typeof(DamagableItem))]
[RequireComponent(typeof(InventoryItem))]
public class MeleeItem : MonoBehaviour
{
    [SerializeField] private MeleeItemScriptableObjCreator _meleeItemData;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        enabled = false;
    }

    /// <summary>
    /// Called from the EquipableItem component after this script is activate when the melee
    /// item is equiped. 
    /// </summary>
    public void OnEquipMeleeWeapon()
    {
        var colliders = GetComponents<Collider>();
        foreach (var c in colliders)
        {
            c.isTrigger = true;
        }
    }


    public void OnUnEquipMeleeWeapon()
    {
        var colliders = GetComponents<Collider>();
        foreach (var c in colliders)
        {
            c.isTrigger = true;
        }
        enabled = false;
    }


    /// <summary>
    /// Every frame the melee component is activated, respond to player input hand handle actions accordingly
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Player.PlayerControllerParent.GetInstance().SetAnimationTrigger(_meleeItemData.meleeAnimationTrigger);
            StartCoroutine(PlayerMeleeAction());
        }
    }



    /// <summary>
    /// Handles the player interaction with the environment with a melee is performed. Calling this function needs to be timed
    /// with the melee animation via a coroutine. 
    /// </summary>
    private IEnumerator PlayerMeleeAction()
    {
        // Delay the result of the melee until the melee animation is partway through
        yield return new WaitForSeconds(_meleeItemData.meleeDelay);

        // Get what the player is looking at in the environment
        var lookAt = Player.EnvironmentDetector.intance.GetLookAtGameObject();
        if (lookAt != null)
        {
            if (lookAt.GetComponent<IMeleeAbleItem>() != null)
            {
                lookAt.GetComponent<IMeleeAbleItem>().TakeDamage(1f);
            }
        }
     
    }

}
