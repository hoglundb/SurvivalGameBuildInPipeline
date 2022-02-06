using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This interface allows us to quickly check if an object can be affected by melee actions. Any meleeable game object contains a component that invokes this interface. 
public interface IMeleeAbleItem
{
    public void TakeDamage(float damageAmount);
}
