using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    [CreateAssetMenu(fileName = "SOBrickFaces", menuName = "ScriptableObjects/SOBrickFaces")]
    public class SOBrickFaces : ScriptableObject
    {
        [SerializeField]
        public List<Geometery.BlockFace> faces;
    }


