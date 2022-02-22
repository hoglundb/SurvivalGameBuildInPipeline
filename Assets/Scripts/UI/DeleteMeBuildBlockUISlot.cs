using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildBlockUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _createBtn;
    private Button _btnBuildItemUI;


    private void Awake()
    {
        _createBtn.GetComponent<Button>().onClick.AddListener(_OnBuildingBlockBtnClick);
    }



    //When player clicks this item, contact the player so the player can manage the placing of the prefab
    private void _OnBuildingBlockBtnClick()
    {
       // Player.PlayerControllerParent.GetInstance().playerBaseBuildingComponent.OnPlayerSelectItemToPlace(_placableItemPrefabName);
    }


    //TODO: show item details in hover
    public void OnPointerEnter(PointerEventData eventData) { }


    public void OnPointerExit(PointerEventData eventData) { }


  
}
