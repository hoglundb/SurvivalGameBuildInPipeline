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
        [SerializeField] private LayerMask _baseBlockMask;
        [SerializeField] private Material _validPlacementMaterial;
        [SerializeField] private Material _invalidPlacementMaterial;
        [SerializeField] private GameObject _baseBuildingEffectPrefab;
        private GameObject _baseBuildingEffect;
        private GameObject _itemCurrentlyPlacing = null;

        private Vector3 _lookPoint; 

        //Components that reference the current selected block. Will be null if no block being placed or block type doesn't have that particular component. 
        private FoundationBlock _foundationBlockComponent;
        private BlockMaterialManager _blockMaterialComponent;
        private Geometery.FaceDefinitions _faceDefinitionsComponent;
        private SaveGame _saveGameComponent;
        private bool _canPlaceBlock = false;
        private PlacementMode _placementMode;

        [SerializeField] private GameObject _blockFaceHighlightPrefab;
        private GameObject _blockFaceHighlight;
        private float _heightOffset = 0f;
        private GameObject _currentSnappedObject;

        [SerializeField] private Vector3 _handSpellOffset;
        #endregion



        #region MonoCallbacks
        private void Awake()
        {
            //_buildingBlockSelectUI = GameObject.Find("BuildingUIPanel");
            _playerControllerParentComponent = GetComponent<PlayerControllerParent>();
            _saveGameComponent = GetComponent<SaveGame>();
            enabled = false;

            //Create the block face highlight game object and disable until it is needed by the player. 
            _blockFaceHighlight = Instantiate(_blockFaceHighlightPrefab, Vector3.up * -1000, Quaternion.identity);

            //Create the base building effect and disable until it is needed by the player. 
            _baseBuildingEffect = Instantiate(_baseBuildingEffectPrefab);
        }


        private void OnEnable()
        {
            ////Enable the building UI
            //_buildingBlockSelectUI.SetActive(true);

            ////Start the player animation for using magic to place building blocks
            //_playerControllerParentComponent.SetAnimationTrigger("LiftBuildingBlock");

            _baseBuildingEffect.SetActive(true);
           
        }


        private void OnDisable()
        {
            //if (_buildingBlockSelectUI != null)
            //{
            //    _buildingBlockSelectUI.SetActive(false);
            //}
          
            if (_itemCurrentlyPlacing != null)
            {
                GameObject.Destroy(_itemCurrentlyPlacing);
                _itemCurrentlyPlacing = null;
            }
        }

   
        // Update is called once per frame
        void Update()
        {
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
                    _PlayBaseBuildingEffect(_foundationBlockComponent.transform.position);
                    _foundationBlockComponent.OnBlockPlace();
                    _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("FoundationBuildingBlock");
                    _itemCurrentlyPlacing.GetComponent<BoxCollider>().isTrigger = false;

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

                //Snap the block to it's sugguested position. 
                _itemCurrentlyPlacing.transform.position = _lookPoint;

                if (Input.GetKeyDown(KeyCode.Mouse0) && _canPlaceBlock)
                {
                    _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("RegularBuildingBlock");
                    SaveGame.GetInstance().AddBlock(_itemCurrentlyPlacing);
                    _PlayBaseBuildingEffect(_faceDefinitionsComponent.transform.position);
                    _blockMaterialComponent.ResetMaterial();
                    if (_currentSnappedObject.name.Contains("Foundation"))
                    {
                        _faceDefinitionsComponent.foundationReference = _currentSnappedObject.transform;
                    }
                    else
                    {
                        _faceDefinitionsComponent.foundationReference = _currentSnappedObject.GetComponent<Geometery.FaceDefinitions>().foundationReference;
                    }

                    GameObject newBlockToPlace = Instantiate(_itemCurrentlyPlacing);
                    newBlockToPlace.name = _itemCurrentlyPlacing.name;
                    newBlockToPlace.GetComponent<Geometery.FaceDefinitions>().SetRotationIndex(_faceDefinitionsComponent.GetBlockRotationIndex());
                    _itemCurrentlyPlacing = null;
                    _itemCurrentlyPlacing = newBlockToPlace;
                    _blockMaterialComponent = _itemCurrentlyPlacing.GetComponent<BlockMaterialManager>();
                    _faceDefinitionsComponent = _itemCurrentlyPlacing.GetComponent<Geometery.FaceDefinitions>();
                    _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("Ignore Raycast");
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
                _itemCurrentlyPlacing.transform.position = _lookPoint + Vector3.up * _heightOffset;
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
            Vector3 fromExistingFoundationToPlacingFoundation = _lookPoint - nearestFoundation.transform.position;
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
            Collider[] colliders = Physics.OverlapSphere(_lookPoint, 2.0f, _baseBlockMask);
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
                    _lookPoint = hit.point;                 
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
                    _lookPoint = hit.point + Vector3.up * .1f;
                    return;
                }

                //If we hit a foundation piece or another block, get the nearest face. This face will be used as the snap point suggestion. 
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("RegularBuildingBlock") || hit.transform.gameObject.layer == LayerMask.NameToLayer("FoundationBuildingBlock"))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("RegularBuildingBlock"))
                    {
                        var hitGameObjectRef = hit.transform.GetComponent<Geometery.FaceDefinitions>();

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
                    Transform faceToSnapTo = faceDef.GetNearestBlockFaceToPoint(hit.point);
                    if (faceToSnapTo != null)
                    {
                        _blockFaceHighlight.transform.position = faceToSnapTo.position + faceToSnapTo.up.normalized * .004f;
                        _blockFaceHighlight.transform.rotation = faceToSnapTo.transform.rotation;
                    }
                   
                    _currentSnappedObject = faceToSnapTo.transform.parent.gameObject;
                      Vector3 offset = _faceDefinitionsComponent.GetSnapPositionOffset(faceToSnapTo);
                    _lookPoint = faceToSnapTo.position + offset;
                    _blockMaterialComponent.UpdatePlacementMaterial(_validPlacementMaterial);
                    _canPlaceBlock = true;
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
        
           // _buildingBlockSelectUI.SetActive(false);
            _playerControllerParentComponent.playerMovementComponent.enabled = true;

            _InstaciateFoundationPiece(placableItemPrefab);
               
        }


        private void _InstaciateFoundationPiece(GameObject placableItemPrefab)
        {
            _itemCurrentlyPlacing = GameObject.Instantiate(placableItemPrefab);
            _itemCurrentlyPlacing.name = placableItemPrefab.name;
            _foundationBlockComponent = _itemCurrentlyPlacing.GetComponent<FoundationBlock>();
            _blockMaterialComponent = _itemCurrentlyPlacing.GetComponent<BlockMaterialManager>();
            _faceDefinitionsComponent = _itemCurrentlyPlacing.GetComponent<Geometery.FaceDefinitions>();
            _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("Ignore Raycast");
            _blockMaterialComponent.UpdatePlacementMaterial(_validPlacementMaterial);
        }

        private void _PlayBaseBuildingEffect(Vector3 targetPos)
        {
            //_playerControllerParentComponent.SetAnimationTrigger("PlaceBuildingBlock");
            //Vector3 handPos = _playerControllerParentComponent.rightHandBoneTransform.position;
            //_baseBuildingEffect.SetActive(true);
            //_baseBuildingEffect.GetComponent<Magic.EffectBaseBuilding>().ShootSpellEffect(handPos - transform.forward * .3f, targetPos);

        }

        #endregion
    }
}

