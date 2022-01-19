using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuildingItems : MonoBehaviour
{
    [SerializeField] private List<GameObject> _prefabs;


    private static BaseBuildingItems _instance;

    private void Awake()
    {
        _instance = this;
    }


    public static BaseBuildingItems GetInstance()
    {
        return _instance;
    }


    public GameObject GetBaseBuildingPrefabByName(string prefabName)
    {
        foreach (var p in _prefabs)
        {
            if (p.name == prefabName) return p;
        }

        Debug.LogError("Could not find prefab named " + prefabName);
        return null;
    }

}


[System.Serializable]
public class BuildingBlockItem
{
   [SerializeField] string name;
   [SerializeField] GameObject prefab;
}
