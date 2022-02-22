using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaseBuilding
{
    public class BaseBuildingUISlot : MonoBehaviour
    {
        [Header("The scriptable obj representing this piece")]
        [SerializeField] private SOBaseBuildingPiece _soBuildingPiece;

        [Header("The 'Create' btn reference")]
        [SerializeField] private GameObject _createBtn;

        private void Awake()
        {
            _createBtn.GetComponent<Button>().onClick.AddListener(_OnCreateBtnClick);

        }


        private void _OnCreateBtnClick()
        {
            //Pass the scriptable obj contining the item data to the player and request that the player allow the player to place it. 
            Player.PlayerControllerParent.GetInstance().playerBaseBuildingComponent.OnPlayerSelectItemToPlace(_soBuildingPiece);
        }
    }
}

