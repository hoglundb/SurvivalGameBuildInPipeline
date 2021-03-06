using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geometery
{
    public class FaceDefinitions : MonoBehaviour
    {
        [Header("Define the block faces")] 
        [SerializeField] private bool _regenerateFacesOnLoad = false;
        [SerializeField] private bool _drawDebugLines = false;
        [SerializeField] private bool _onStartCreateFaceTransforms = false;
        [SerializeField] private GameObject _facePrefab;
        [Header("Gets populated on Awake")]
        [SerializeField] private List<Transform> _faceTransforms;
        [Header("Set this manually")]
        [SerializeField] private List<Vector3> _possibleRotations;

        //Track the orgonal layer on this object. We need to be able to set it to ignore raycasts and back. Object with collider may be child object of this. 
        private LayerMask _objLayerName;

        [SerializeField] private Collider _collider;

        private int _currentPlacementIndex = 0;

        internal Transform foundationReference;

        private int _currentRotationIndex = 0;

        private void Awake()
        {
            //Cache the layer that this object orionally had
            _objLayerName = gameObject.layer;

            if (_onStartCreateFaceTransforms)
            {
                _faceTransforms = new List<Transform>();
                _CreateFaceTransforms();
            }

            _faceTransforms = new List<Transform>();
            //Harvest all the child objects that are face transforms
            foreach (Transform child in transform)
            {
                if (child.gameObject.name.ToLower().Contains("facetransform"))
                {
                    _faceTransforms.Add(child);
                }
            }

            if (_collider == null)
            { 
             _collider = GetComponent<Collider>();
            }
           
            if (_collider == null)
            {
                _collider = GetComponentInChildren<Collider>();
            }
            if (_collider == null)
            {
                Debug.LogError("Error: No collider on " + name);
            }
        }



        public void SetColliderTrigger(bool isTrigger)
        {
            _collider.isTrigger = isTrigger;
        }
  

        public void IngnoreRaycasts()
        { 
           _collider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }


        public void RestoreLayerMaskToOrigonal()
        {
            _collider.gameObject.layer = _objLayerName;
        }



        //Cycles through the possible rotations for the block
        public void RotateBlock(float dir)
        {
            if (dir > 0)
            {
                _currentRotationIndex++;
            }
            else 
            {
                _currentRotationIndex--;
            }

            if (_currentRotationIndex < 0) _currentRotationIndex = _possibleRotations.Count - 1;

            else if (_currentRotationIndex > _possibleRotations.Count - 1) _currentRotationIndex = 0;            
        }


        //Returns the block rotation Euler angles based on the current rotation index. 
        public Vector3 GetBlockRotationOffset()
        {
            return _possibleRotations[_currentRotationIndex];
        }


        //Returns the rotation index of the block. If this block is set and another block is instanciated, this rotation index is copied to the new block. 
        public int GetBlockRotationIndex()
        {
            return _currentRotationIndex;
        }


        //Called when a block is instanciated right after placing another block. The rotation index of this block is copied and set from the previously placed block. 
        public void SetRotationIndex(int newRotationIndex)
        {
            if (newRotationIndex > 0 && newRotationIndex < _possibleRotations.Count)
            {
                _currentRotationIndex = newRotationIndex;
            }
        }


        public void OnPlaceBlock(Transform foundationRef)
        {
            foundationReference = foundationRef;
        }


        // private void Update()
        //{
        //    if (_drawDebugLines)
        //    {
        //        foreach (var f in _faceTransforms)
        //        {
        //            Debug.LogError(f.up);
        //            Debug.DrawRay(f.position, f.up * 10f, Color.red);
        //        }
        //    }
        //}


        //Another block calls this to determine the nearest face to it
        public Transform GetNearestBlockFaceToPoint(Vector3 point)
        {         
            float curDistance = 100000f;
            Transform nearestFace = _faceTransforms[0];
            foreach (var f in _faceTransforms)
            {
                float distToFace = Helpers.GetDistanceSquared(point, f.position);
                if (distToFace < curDistance)
                {
                    curDistance = distToFace;
                    nearestFace = f;
                }
            }
            return nearestFace;
        }


        //Returns the position this block needs to be in to allign with the face of the other block
        public Vector3 GetSnapPositionOffset(Transform faceOfPlacedBlockWeWillSnapTo)
        {
            List<Transform> parallelFaces = _FindFacesWithMatchingNormals(faceOfPlacedBlockWeWillSnapTo);
            if (parallelFaces.Count > 0)
            {
                int index = _currentPlacementIndex % parallelFaces.Count;
                return -(parallelFaces[index].position - parallelFaces[index].parent.position);
            }

            return Vector3.one * 100000000f;
        }


        private List<Transform> _FindFacesWithMatchingNormals(Transform faceTransformToMatch)
        {
            List<Transform> matchingFaces = new List<Transform>();

            foreach (var f in _faceTransforms)
            { 
                //rotation must match on at least two axies
               if(f.up == -faceTransformToMatch.up)// && (f.right == faceTransformToMatch.right || f.right == -faceTransformToMatch.right))                 
               {
                    matchingFaces.Add(f);
               }
            }
            return matchingFaces;
        }



        public void NextPlacementOption()
        {
            _currentPlacementIndex++;
        }


        private void _CreateFaceTransforms()
        {
            if (gameObject.name.Contains("FoundationBuildingBlock"))
            {
                float length = 1.6f;
                float squareSize = .2f;
                float height = .4f;

                float numFacesAlongLength = (length / squareSize) * 2f;
                float startX = (length - squareSize / 2f);
                float startY = startX;

                float curX = startX;
                float curY = startY;
                for (int i = 0; i < numFacesAlongLength; i++)
                {
                    for (int j = 0; j < numFacesAlongLength; j++)
                    {
                        GameObject newFaceGameObj = Instantiate(_facePrefab);
                        newFaceGameObj.transform.parent = transform;
                        newFaceGameObj.transform.localPosition = new Vector3(curX, height, curY);
                        _faceTransforms.Add(newFaceGameObj.transform);
                        curY -= squareSize;
                    }
                    curY = startY;
                    curX -= squareSize;
                }
            }
        }

    }
}

