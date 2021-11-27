using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform _rightHandBone;

    private Animator _anim;


    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _anim.SetTrigger("ChopAxe");
        }
    }


    public void EquipWeapon(GameObject weapon)
    { 
    
    }
}



public enum WeaponTypes
{ 
    AXE,
    BOW,

}