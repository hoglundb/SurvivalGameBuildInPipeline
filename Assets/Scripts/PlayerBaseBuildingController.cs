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
        private int numBlocks = 0;
        private GameObject _itemCurrentlyPlacing = null;
        private Vector3 _lookPoint;
        private FoundationBlock _foundationBlockComponent;
        private bool _canPlaceCurrentBlock = false;

        private void Awake()
        {
            _buildingBlockSelectUI = GameObject.Find("BuildingUIPanel");
            _playerControllerParentComponent = GetComponent<PlayerControllerParent>(); 
            enabled = false;
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
            if (_itemCurrentlyPlacing != null)
            {
                _ManagePlayerItemPlacement();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _foundationBlockComponent.OnBlockPlace();
                    _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("FoundationBuildingBlock");
                    _InstaciateFoundationPiece(_itemCurrentlyPlacing);
                }

                _ManageBaseBlockPlacement();
            }
        }


        private void _ManageBaseBlockPlacement()
        {
            GameObject nearestFoundation = _GetNearestBaseBlock(_itemCurrentlyPlacing);
            if (nearestFoundation == null)
            {
                _itemCurrentlyPlacing.transform.position = _lookPoint;
            }
            else
            {
                Vector3 SuggestedPosition = _GetFoundationSuggestedPositionFromLookPoint(nearestFoundation);
                 _itemCurrentlyPlacing.transform.position = SuggestedPosition;
            }

            //update the mesh material based on if the block can legally be placed here. The BlockCollistionChecker script on the block actually performs the check.
            _foundationBlockComponent.UpdatePlacementMaterial(!_foundationBlockComponent.IsCollidingThisFrame());
        }


        private Vector3 _GetFoundationSuggestedPositionFromLookPoint(GameObject nearestFoundation)
        {
            float size = 4;
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
        private void _ManagePlayerItemPlacement()
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


        private void LateUpdate()
        {
            //Reset this so we know if block is colliding next 
            _canPlaceCurrentBlock = true;
        }


        public void OnPlayerSelectItemToPlace(GameObject placableItemPrefab)
        {
            _buildingBlockSelectUI.SetActive(false);
            _playerControllerParentComponent.playerMovementComponent.enabled = true;

            _InstaciateFoundationPiece(placableItemPrefab);         
        }

        private void _InstaciateFoundationPiece(GameObject placableItemPrefab)
        {            
            _itemCurrentlyPlacing = GameObject.Instantiate(placableItemPrefab);
            _foundationBlockComponent = _itemCurrentlyPlacing.GetComponent<FoundationBlock>();
            _itemCurrentlyPlacing.layer = LayerMask.NameToLayer("Ignore Raycast");;
            _foundationBlockComponent.UpdatePlacementMaterial(true);
        }
    }

}

