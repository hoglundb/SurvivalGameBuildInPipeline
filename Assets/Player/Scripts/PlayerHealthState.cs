using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    //Class to hold the current health stats for the player. 
    [Serializable]
    public class PlayerHealthState
    {
        [SerializeField] [Range(0,1)] public float _blood;
        [SerializeField] [Range(0,1)] public float _carbohydrates;
        [SerializeField] [Range(0,1)] public float _protiens;
        [SerializeField] [Range(0,1)] public float _minerals;
        [SerializeField] [Range(0,1)] public float _vitamins;
        [SerializeField] [Range(0,1)] public float _warmth;
        [SerializeField] [Range(0,1)] public float _hydration;
    }
}

