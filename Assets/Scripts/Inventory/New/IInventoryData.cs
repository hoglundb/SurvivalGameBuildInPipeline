using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryData
{
    abstract bool TryAddInventoryItem(Inventory.AbstractInventoryItem objInventoryItemComponent);
}
