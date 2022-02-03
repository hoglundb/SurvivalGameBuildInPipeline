using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/*This component is responsible for when the player is in "Base-Building" mode*/
namespace Player
{
    public class PlayerBaseBuildingController : MonoBehaviour
    {
        #region PrivateMemberVars

        //private GameObject _buildingBlockSelectUI;
        private PlayerControllerParent _playerControllerParentComponent;

        [Header("References for block placement")]
        [SerializeField] private LayerMask _baseBlockMask;
        [SerializeField] private Material _validPlacementMaterial;
        [SerializeField] private Material _invalidPlacementMaterial;

        [SerializeField] private GameObject _blockFaceHighlightPrefab;
        private GameObject _itemCurrentlyPlacing = null;

        //The suggested placement point. If > 10000000 we no that no valid placement point is found based on where player is looking
        private Vector3 _snapPoint;

        //The actual place the player is aiming as it intersects with objects in the scene. 
        private Vector3 _lookPoint; 

        //Components that reference the current selected block. Will be null if no block being placed or block type doesn't have that particular component. 
        private FoundationBlock _foundationBlockComponent;
        private BlockMaterialManager _blockMaterialComponent;
        private Geometery.FaceDefinitions _faceDefinitionsComponent;
        private SaveGame _saveGameComponent;
        private bool _canPlaceBlock = false;
        private PlacementMode _placementMode;

        private GameObject _blockFaceHighlight;
        private float _heightOffset = 0f;
        private GameObject _currentSnappedObject;
        #endregion



        #region MonoCallbacks
        private void Awake()
        {
            //_buildingBlockSelectUI = GameObject.Find("BuildingUIPanel");
            _playerControllerParentComponent = GetComponent<PlayerControllerParent>();
            _saveGameComponent = GetComponent<SaveGame>();

            //Create the block face highlight game object and disable until it is needed by the player. 
            _blockFaceHighlight = Instantiate(_blockFaceHighlightPrefab, Vector3.up * -1000, Quaternion.identity);

        }



        private void OnDisable()
        {          
            if (_itemCurrentlyPlacing != null)
            {
                 Destroy(_itemCurrentlyPlacing);
                _itemCurrentlyPlacing = null;
            }
        }

   
        // Update is called once per frame
        void Update()
        {
            //reset everything if player exits block placement. 
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.B))
            {
                Destroy(_itemCurrentlyPlacing);
                _itemCurrentlyPlacing = null;
                _blockFaceHighlight.transform.position = Vector3.down * 10000f;
            }

            if (!_itemCurrentlyPlacing) return;

            //When the player is placing a foundation piece. 
            if (_placementMode == PlacementMode.FOUNDATION)
            {
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
                _heightOffset += scrollAmount;

                //Allow player to rotate the base piece
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Vector3 currentEulers = _itemCurrentlyPlacing.transform.rotation.eulerAngles;
                    currentEulers.y += 15f;
                    _itemCurrentlyPlacing.transform.eulerAngles = currentEulers;
                }

                _GetFoundationPlacementPoint();
                if (Input.GetKeyDown(KeyCode.Mouse0) && !_foundationBlockComponent.IsCollidingThisFrame())
                {
                    _foundationBlockComponent.OnBlockPlace();
                    _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("FoundationBuildingBlock");
                    _itemCurrentlyPlacing.GetComponent<Geometery.FaceDefinitions>().SetColliderTrigger(false);  
                    
                    //Add the item the player is placing to the dictionary of placed items so it can be saved by the player. 
                    SaveGame.GetInstance().AddBlock(_itemCurrentlyPlacing);

                    _InstaciateFoundationPiece(_itemCurrentlyPlacing);
                }
                _ManageFoundationBlockPlacement();
            }

            //When the player is placing a non-foundation piece
            else
            {
                //If the player scrolls this sets the rotation offset of the block that is currently being placed. Pass this information to the block. 
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
                if(scrollAmount != 0)  _faceDefinitionsComponent.RotateBlock(scrollAmount);

                //If player right clicks, update the suggested face for the currently placed block. The face only updates within the given rotation. 
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    _faceDefinitionsComponent.NextPlacementOption();
                }

                //Set the snap point based on the nearby geometery, where the player is aiming ,and the rotation/position offset the player has selected. 
                _ComputeSnapPoint();

                //Snap the block to it's sugguested position, but only if one is available
                if (_canPlaceBlock && _snapPoint.magnitude < 1000000f)
                {
                    Debug.LogError(_snapPoint.magnitude);
                    _itemCurrentlyPlacing.transform.position = _snapPoint;
                }

                //No valid snap suggestions, let the player just keep dragging the piece
                else 
                {
                    _itemCurrentlyPlacing.transform.position = _lookPoint;
                }

                if (Input.GetKeyDown(KeyCode.Mouse0) && _canPlaceBlock)
                {
                    //Restore the game object to use it's origonally defined layer mask 
                    _itemCurrentlyPlacing.GetComponent<Geometery.FaceDefinitions>().RestoreLayerMaskToOrigonal();
                    SaveGame.GetInstance().AddBlock(_itemCurrentlyPlacing);
                    _blockMaterialComponent.ResetMaterial();
                    if (_currentSnappedObject.name.Contains("Foundation"))
                    {
                        _faceDefinitionsComponent.foundationReference = _currentSnappedObject.transform;
                    }
                    else
                    {
                        _faceDefinitionsComponent.foundationReference = _currentSnappedObject.GetComponent<Geometery.FaceDefinitions>().foundationReference;
                    }
                    _itemCurrentlyPlacing.GetComponent<Geometery.FaceDefinitions>().SetColliderTrigger(false);
                    GameObject newBlockToPlace = Instantiate(_itemCurrentlyPlacing);
                    newBlockToPlace.name = _itemCurrentlyPlacing.name;
                    newBlockToPlace.GetComponent<Geometery.FaceDefinitions>().SetRotationIndex(_faceDefinitionsComponent.GetBlockRotationIndex());
                    _itemCurrentlyPlacing = null;
                    _itemCurrentlyPlacing = newBlockToPlace;
                    _blockMaterialComponent = _itemCurrentlyPlacing.GetComponent<BlockMaterialManager>();
                    _faceDefinitionsComponent = _itemCurrentlyPlacing.GetComponent<Geometery.FaceDefinitions>();
                    _itemCurrentlyPlacing.GetComponent<Geometery.FaceDefinitions>().IngnoreRaycasts();
                }
 
            }
        }

        #endregion



        #region General
  
        private void _ManageFoundationBlockPlacement()
        {
            GameObject nearestFoundation = _GetNearestBaseBlock(_itemCurrentlyPlacing);
            if (nearestFoundation == null)
            {
                _itemCurrentlyPlacing.transform.position = _snapPoint + Vector3.up * _heightOffset;
            }
            else
            {
                Vector3 SuggestedPosition = _GetFoundationSuggestedPositionFromLookPoint(nearestFoundation);
                _itemCurrentlyPlacing.transform.rotation = nearestFoundation.transform.rotation;
                _heightOffset = 0f;
                 _itemCurrentlyPlacing.transform.position = SuggestedPosition;
            }

            //update the mesh material based on if the block can legally be placed here. The BlockCollistionChecker script on the block actually performs the check.            
            bool isColliding = _foundationBlockComponent.IsCollidingThisFrame();
            if (isColliding)
            {
                _blockMaterialComponent.UpdatePlacementMaterial(_invalidPlacementMaterial);
            }
            else 
            {
                _blockMaterialComponent.UpdatePlacementMaterial(_validPlacementMaterial);
            }
        }


        private Vector3 _GetFoundationSuggestedPositionFromLookPoint(GameObject nearestFoundation)
        {
            float size = 3.2f;
            Vector3 fromExistingFoundationToPlacingFoundation = _snapPoint - nearestFoundation.transform.position;
            float option1 = Mathf.Abs(Vector3.Angle(_itemCurrentlyPlacing.transform.forward, fromExistingFoundationToPlacingFoundation));
            float option2 = Mathf.Abs(Vector3.Angle(-_itemCurrentlyPlacing.transform.forward, fromExistingFoundationToPlacingFoundation));
            float option3 = Mathf.Abs(Vector3.Angle(_itemCurrentlyPlacing.transform.right, fromExistingFoundationToPlacingFoundation));
            float option4 = Mathf.Abs(Vector3.Angle(-_itemCurrentlyPlacing.transform.right, fromExistingFoundationToPlacingFoundation));
            List<float> optionsList = new List<float>{ option1, option2, option3, option4 };
            float smallest = optionsList.Min();
            if (smallest == option1)
            {
                return  nearestFoundation.transform.position + nearestFoundation.transform.forward.normalized * size;
            }
            if (smallest == option2)
            {
                return nearestFoundation.transform.position - nearestFoundation.transform.forward.normalized * size;
            }
            if (smallest == option3)
            {
                return nearestFoundation.transform.position + nearestFoundation.transform.right.normalized * size;
            }
            if (smallest == option4)
            {
                return nearestFoundation.transform.position - nearestFoundation.transform.right.normalized * size;
            }
            return Vector3.zero;
        }


        /*Takes a base block game object and checks if a nearby base block is present in the scene. 
         Otherwise returns the nearest one. */
        private GameObject _GetNearestBaseBlock(GameObject thisBaseBlock)
        {
            Collider[] colliders = Physics.OverlapSphere(_snapPoint, 2.0f, _baseBlockMask);
            foreach (var c in colliders)
            {
                return c.gameObject;
            }
            return null;
        }


        /*Called each frame that the player is in the process of placing an item*/
        private void _GetFoundationPlacementPoint()
        {
            //Raycast from the camera to the terrain     
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 30f))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") || hit.transform.gameObject.layer == LayerMask.NameToLayer("FoundationBuildingBlock"))
                {
                    _snapPoint = hit.point;                 
                    return;
                }
            }
        }
  

        private void _ComputeSnapPoint()
        {
            RaycastHit hit;
            _canPlaceBlock = false;
            _blockMaterialComponent.UpdatePlacementMaterial(_invalidPlacementMaterial);
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 30f))
            {
                //If we hit the ground, just let the player keep dragging the block
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    _snapPoint = hit.point + Vector3.up * .4f;
                    _lookPoint = hit.point + Vector3.up * .4f;
                    return;
                }

                //If we hit a foundation piece or another block, get the nearest face. This face will be used as the snap point suggestion. 
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("RegularBuildingBlock") || hit.transform.gameObject.layer == LayerMask.NameToLayer("FoundationBuildingBlock"))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("RegularBuildingBlock"))
                    {
                        _lookPoint = hit.point + Vector3.up * .1f;

                        //Get the face definitions component that is on every placeable piece. For odly shaped pieces this component is on a child game object. 
                        var hitGameObjectRef = hit.transform.GetComponent<Geometery.FaceDefinitions>();                      
                        if (hitGameObjectRef == null) hitGameObjectRef = hit.transform.parent.GetComponent<Geometery.FaceDefinitions>();
                        if (hitGameObjectRef == null) Debug.LogError("No FaceDefinintions component on " + hit.transform.gameObject.name);


                        //No foundation reference cansometimes happen for blocks that were spawned from the previous save
                        if (hitGameObjectRef.foundationReference == null || hitGameObjectRef.foundationReference.rotation == null)
                        {
                            hitGameObjectRef.foundationReference = hit.transform;
                        }
                            _itemCurrentlyPlacing.transform.rotation = hitGameObjectRef.foundationReference.rotation;
        
                    }
                    else 
                    {
                        _itemCurrentlyPlacing.transform.rotation = hit.transform.rotation;
                    }
                    
                    _itemCurrentlyPlacing.transform.Rotate(_faceDefinitionsComponent.GetBlockRotationOffset());
                    Geometery.FaceDefinitions faceDef = hit.transform.gameObject.GetComponent<Geometery.FaceDefinitions>();
                    if(faceDef == null) faceDef = hit.transform.parent.GetComponent<Geometery.FaceDefinitions>();
                    Transform faceToSnapTo = faceDef.GetNearestBlockFaceToPoint(hit.point);
                    if (faceToSnapTo != null)
                    {
                        _blockFaceHighlight.transform.position = faceToSnapTo.position + faceToSnapTo.up.normalized * .004f;
                        _blockFaceHighlight.transform.rotation = faceToSnapTo.transform.rotation;
                    }
                   
                    _currentSnappedObject = faceToSnapTo.transform.parent.gameObject;
                      Vector3 offset = _faceDefinitionsComponent.GetSnapPositionOffset(faceToSnapTo);

                    //If no valid snap suggestions, don't snap the piece into place and make sure it is highlighed red
                    if (offset.magnitude > 1000000f)
                    {
                        _canPlaceBlock = false;
                        _blockMaterialComponent.UpdatePlacementMaterial(_invalidPlacementMaterial);
                    }
                    else 
                    {
                        _snapPoint = faceToSnapTo.position + offset;
                        _blockMaterialComponent.UpdatePlacementMaterial(_validPlacementMaterial);
                        _canPlaceBlock = true;
                    }
          
                    return;           
                }
            }
            return;
        }


        public void OnPlayerSelectItemToPlace(string placableItemPrefabName)
        {
            GameObject placableItemPrefab = BaseBuildingItems.GetInstance().GetBaseBuildingPrefabByName(placableItemPrefabName);
            if (placableItemPrefab.name.Contains("Foundation"))
            {
                _placementMode = PlacementMode.FOUNDATION;
            }
            else 
            {
                _placementMode = PlacementMode.BRICK;
            }
        
            _playerControllerParentComponent.playerMovementComponent.enabled = true;

            _InstaciateFoundationPiece(placableItemPrefab);
               
        }


        public void CreateBlockForPlacement(GameObject placeableItemPrefab)
        {
            if (placeableItemPrefab.name.Contains("Foundation"))
            {
                _placementMode = PlacementMode.FOUNDATION;
            }
            else
            {
                _placementMode = PlacementMode.BRICK;
            }
            _playerControllerParentComponent.playerMovementComponent.enabled = true;
            _InstaciateFoundationPiece(placeableItemPrefab);
        }


        private void _InstaciateFoundationPiece(GameObject placableItemPrefab)
        {
            _itemCurrentlyPlacing = Instantiate(placableItemPrefab);
            _itemCurrentlyPlacing.GetComponent<Geometery.FaceDefinitions>().SetColliderTrigger(true);
            _itemCurrentlyPlacing.name = placableItemPrefab.name;
            _foundationBlockComponent = _itemCurrentlyPlacing.GetComponent<FoundationBlock>();
            _blockMaterialComponent = _itemCurrentlyPlacing.GetComponent<BlockMaterialManager>();
            _faceDefinitionsComponent = _itemCurrentlyPlacing.GetComponent<Geometery.FaceDefinitions>();
            _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("Ignore Raycast");
            _blockMaterialComponent.UpdatePlacementMaterial(_validPlacementMaterial);
        }


        #endregion
    }
}

