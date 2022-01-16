using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalResourceManager : MonoBehaviour
{

    [SerializeField] private GameObject _spellPrefab;
    [SerializeField] private GameObject _spell2Prefab;
    private GameObject _spell;
    private GameObject _spell2;
    private static GlobalResourceManager _instance;

    private void Awake()
    {
        _instance = this;
        _AllocateResources();
    }

    public static GlobalResourceManager GetInstance()
    {
        return _instance;
    }


    private void _AllocateResources()
    {
        _spell = Instantiate(_spellPrefab);
        _spell2 = Instantiate(_spell2Prefab);
    }

    public GameObject GetSpell(int type)
    {
        if (type == 1)
        {
            return _spell;
        }
        return _spell2;
    }
}
