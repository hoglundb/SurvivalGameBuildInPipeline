using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildBlockUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _placableItemPrefabName;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private GameObject _itemButtonComponent;
    [SerializeField] private GameObject _backgroundComponent;
    private Image _backgroundImage;
    private Button _btnBuildItemUI;
    private Player.PlayerControllerParent _playerControllerParentComponent;
    private bool _isSelected = false;


    

    private void OnEnable()
    {
        if (_btnBuildItemUI == null)
        {
            _btnBuildItemUI = _itemButtonComponent.GetComponent<Button>();
            _btnBuildItemUI.onClick.AddListener(_OnBuildingBlockBtnClick);
     
        }
        if (_backgroundImage == null)
        {
            _backgroundImage = _backgroundComponent.GetComponent<Image>();
        }
        if (_playerControllerParentComponent == null)
        {
            _playerControllerParentComponent = Player.PlayerControllerParent.GetInstance();
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _backgroundImage.color = _selectedColor;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        _backgroundImage.color = _defaultColor;
    }


    //When player clicks this item, contact the player so the player can manage the placing of the prefab
    private void _OnBuildingBlockBtnClick()
    {
        _playerControllerParentComponent.playerBaseBuildingComponent.OnPlayerSelectItemToPlace(_placableItemPrefabName);
    }


    // Start is called before the first frame update
    void Start()
    {
        _playerControllerParentComponent = Player.PlayerControllerParent.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
