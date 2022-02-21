using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTreeTakeDamage : MonoBehaviour
{
    [SerializeField] private GameObject _treeReference;
    private Tree.TreeController _treeControllerComponent;

    [Header("Check this to do damage")]
    [SerializeField] private bool _takeDamage;
    [SerializeField] private float _damageToGive = 10f;
    private bool _curCheckboxValue;

    // Start is called before the first frame update
    void Start()
    {
        _treeControllerComponent = _treeReference.GetComponent<Tree.TreeController>();
        _curCheckboxValue = false;
        _takeDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_curCheckboxValue != _takeDamage)
        {
            _curCheckboxValue = _takeDamage;
            _OnTreeTakeDamage();
        }
    }


    private void _OnTreeTakeDamage()
    {
        _treeControllerComponent.TakeDamage(_damageToGive);
    }
}
