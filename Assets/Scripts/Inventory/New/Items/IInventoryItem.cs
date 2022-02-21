using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Inventory
{
    public interface IInventoryItem 
    {
        public SOGeneralInventoryItem GetItemInfo();
    }
}

