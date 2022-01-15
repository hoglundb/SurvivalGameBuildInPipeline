using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/*This component is responsible for when the player is in "Base-Building" mode*/
namespace Player
{
    public class PlayerBaseBuildingController : MonoBehaviour
    {
        private GameObject _buildingBlockSelectUI;
        private PlayerControllerParent _playerControllerParentComponent;
        [SerializeField] private LayerMask _baseBlockMask;
        [SerializeField] private Material _validPlacementMaterial;
        [SerializeField] private Material _invalidPlacementMaterial;
        private GameObject _itemCurrentlyPlacing = null;
        private Vector3 _lookPoint; 

        //Components that reference the current selected block. Will be null if no block being placed or block type doesn't have that particular component. 
        private FoundationBlock _foundationBlockComponent;
        private BlockMaterialManager _blockMaterialComponent;
        private Geometery.FaceDefinitions _faceDefinitionsComponent;
        private bool _canPlaceBlock = false;
        private PlacementMode _placementMode;
        [SerializeField] private GameObject _testMassPlacePrefab;
        private float _heightOffset = 0f;
        private GameObject _currentSnappedObject;
        private void Awake()
        {
            _buildingBlockSelectUI = GameObject.Find("BuildingUIPanel");
            _playerControllerParentComponent = GetComponent<PlayerControllerParent>(); 
            enabled = false;

            //for (int i = 0; i < 10000; i++)
            //{
            //    float x = Random.Range(-25, 25);
            //    float y = Random.Range(-0, 50);
            //    float z = Random.Range(-25, 25);
            //    var newTestObj = Instantiate(_testMassPlacePrefab);
            //    newTestObj.transform.position = transform.position + new Vector3(x, y, z);
            //    newTestObj.GetComponent<BoxCollider>().enabled = false;
             
            //}
        }


        private void OnEnable()
        {
            _buildingBlockSelectUI.SetActive(true);          
        }


        private void OnDisable()
        {
            if (_buildingBlockSelectUI != null)
            {
                _buildingBlockSelectUI.SetActive(false);
            }
          
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
                    _foundationBlockComponent.OnBlockPlace();
                    _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("FoundationBuildingBlock");
                    _InstaciateFoundationPiece(_itemCurrentlyPlacing);
                }
                _ManageFoundationBlockPlacement();
            }

            //When the player is placing a non-foundation piece
            else
            {
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
                if(scrollAmount != 0)  _faceDefinitionsComponent.RotateBlock(scrollAmount);

                _GetBlockPlacementPoint();
                _itemCurrentlyPlacing.transform.position = _lookPoint;

                if (Input.GetKeyDown(KeyCode.Mouse0) && _canPlaceBlock)
                {
                    _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("RegularBuildingBlock");
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
                    _itemCurrentlyPlacing = null;
                    _itemCurrentlyPlacing = newBlockToPlace;
                    _blockMaterialComponent = _itemCurrentlyPlacing.GetComponent<BlockMaterialManager>();
                    _faceDefinitionsComponent = _itemCurrentlyPlacing.GetComponent<Geometery.FaceDefinitions>();
                    _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
 
            }
        }


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
  

        private void _GetBlockPlacementPoint()
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
                        _itemCurrentlyPlacing.transform.rotation = hit.transform.GetComponent<Geometery.FaceDefinitions>().foundationReference.rotation;
                    }
                    else 
                    {
                        _itemCurrentlyPlacing.transform.rotation = hit.transform.rotation;
                    }
                    
                    _itemCurrentlyPlacing.transform.Rotate(_faceDefinitionsComponent.GetBlockRotationOffset());
                    Geometery.FaceDefinitions faceDef = hit.transform.gameObject.GetComponent<Geometery.FaceDefinitions>();
                    Transform faceToSnapTo = faceDef.GetNearestBlockFaceToPoint(hit.point);
                    _currentSnappedObject = faceToSnapTo.transform.parent.gameObject;
                      Vector3 offset = _faceDefinitionsComponent.GetSnapPositionOffset(faceToSnapTo);
                    _lookPoint = faceToSnapTo.position + offset;
                    _blockMaterialComponent.UpdatePlacementMaterial(_validPlacementMaterial);
                    _canPlaceBlock = true;
                    return;
           
                }
            }
        }


        public void OnPlayerSelectItemToPlace(GameObject placableItemPrefab)
        {
            if (placableItemPrefab.name.Contains("Foundation"))
            {
                _placementMode = PlacementMode.FOUNDATION;
            }
            else 
            {
                _placementMode = PlacementMode.BRICK;
            }
        
            _buildingBlockSelectUI.SetActive(false);
            _playerControllerParentComponent.playerMovementComponent.enabled = true;

            _InstaciateFoundationPiece(placableItemPrefab);
               
        }

        private void _InstaciateFoundationPiece(GameObject placableItemPrefab)
        {                        
            _itemCurrentlyPlacing = GameObject.Instantiate(placableItemPrefab);
            _foundationBlockComponent = _itemCurrentlyPlacing.GetComponent<FoundationBlock>();
            _blockMaterialComponent = _itemCurrentlyPlacing.GetComponent<BlockMaterialManager>();
            _faceDefinitionsComponent = _itemCurrentlyPlacing.GetComponent<Geometery.FaceDefinitions>();
            _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("Ignore Raycast");
            _blockMaterialComponent.UpdatePlacementMaterial(_validPlacementMaterial);
        }
    }
}


public enum PlacementMode
{ 
   FOUNDATION, 
   BRICK,
}
