using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Inventory Item component for items where we don't need to distinguish between individual items of a particular type. 
 * The actual inventory (individual inventory items) doesn't hold the phyiscal objects. Instead it just holds a counter that is 
 * incremented with the object of a specific type is added to the inventory. 
 */
namespace Inventory
{
    public class GroupableInventoryItem : AbstractInventoryItem {  }

}
