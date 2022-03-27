using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Component to be present on all prefab items that can take damage. Tracks the health of the item and has the methods for 
/// recieving damage.
/// </summary>
public class DamagableItem : MonoBehaviour
{
    /// <summary>
    /// The normalized health of this game object. Will vary from 0 to 1 depending on damage recieved. 
    /// </summary>
    private float _health = 1f;


    /// <summary>
    /// Decrements the _health variable by the specified amount.
    /// </summary>
    /// <param name="normaizedDamageAmount">a float representing how much damage to take away</param>
    public void TakeDamage(float normaizedDamageAmount)
    {
        _health -= normaizedDamageAmount;
        if (_health < 0f) _health = 0f;
    }


    /// <summary>
    /// Returns the value of the _health variable. Call this function to determine how much health is left
    /// on a DamagableItem game object. 
    /// </summary>
    /// <returns>Returns the value of _health as a float.</returns>
    public float GetHealth()
    {
        return _health;
    }


    /// <summary>
    /// Resets the normalized health to 1. Call this if restoring health to or reusing this game object. 
    /// </summary>
    public void ResetHealth()
    {
        _health = 1f;
    }
}
