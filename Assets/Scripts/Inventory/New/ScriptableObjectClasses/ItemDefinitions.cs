using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Inventory/Data/SOItemDefinitions")]
public class ItemDefinitions : MonoBehaviour
{
    [Header("Material items in inventory (groupable)")]
    public List<SOMaterial> materials;

    [Header("Ammo items in inventory (groupable")]
    public List<SOAmmo> ammo;

    [Header("Weapons/tools in inventory (non-groupable)")]
    public List<GameObject> weaponsAndTools;

    [Header("Food in inventory (non-groupable")]
    public List<GameObject> food;


    private static ItemDefinitions _instance;


    public static ItemDefinitions GetInstance()
    {
        return _instance;
    }



    private void Awake()
    {
        _instance = this;
    }



    public void AddMaterial(SOMaterial materialToAdd)
    {
        Debug.LogError(materialToAdd.ItemName);
        Debug.LogError("adding");
        foreach (var m in materials)
        {
            Debug.LogError(m.ItemName);
            if (m.ItemName == materialToAdd.ItemName)
            {
                m.quantity++;
            }
        }
    }



    //Called to remove the specified quantity of the specified type from the player's inventory. 
    public void RemoveMaterial(SOMaterial materialToRemove, int quantityToRemove = 1)
    {
        foreach (var m in materials)
        {
            if (m.ItemName == materialToRemove.ItemName)
            {
                for (int i = 0; i < quantityToRemove; i++)
                {
                    m.quantity--;
                }
            }
        }
    }



    public void AddAmmo(SOAmmo ammoToAdd)
    {
        foreach (var a in ammo)
        {
            if (a.ItemName == ammoToAdd.ItemName)
            {
                a.quantity++;
            }
        }
    }



    public void AddFood(GameObject foodItemGameObj)
    {
        food.Add(foodItemGameObj);
    }



    public int GetItemQuantity(SOGeneralInventoryItem item)
    {
        if (item is SOMaterial)
        {
            return _FindMaterialItem((SOMaterial)item).quantity;
        }

        if (item is SOAmmo)
        {
            return _FindAmmoItem((SOAmmo)item).quantity;
        }

        return -1;
    }


    private SOMaterial _FindMaterialItem(SOMaterial item)
    {
        foreach (var m in materials)
        {
            if (m.ItemName == item.ItemName) return item;
        }
        return null;
    }


    private SOAmmo _FindAmmoItem(SOAmmo am)
    {
        foreach (var a in ammo)
        {
            if (a.ItemName == am.ItemName) return am;
        }
        return null;
    }
}




