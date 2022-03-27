using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Component is attached to the tool tip for splitting an inventory item
/// </summary>
public class SplitInventoryItemsTooltip : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    /// <summary>
    /// The current container being referenced that the player can split items in, using the UI tooltip. 
    /// </summary>
    public InventoryItemContainer currentContainer;

    /// <summary>
    /// References the child game object that contains the btn for spliting one
    /// </summary>
    [SerializeField] private GameObject _btnGameObjSplit1;

    /// <summary>
    /// References the child game object that conatins the btn for splitting half
    /// </summary>
    [SerializeField] private GameObject _btnGameObjSplitHalf;

    /// <summary>
    /// References the child button component for splitting one
    /// </summary>
    private Button _btnSplitOne;

    /// <summary>
    /// References the child button component for splitting half
    /// </summary>
    private Button _btnSplitHalf;

    /// <summary>
    /// Static reference to this class so we can treat it like a singleton
    /// </summary>
    public static SplitInventoryItemsTooltip instance;

    /// <summary>
    /// True if the tooltip is in use. False otherwise. Allows component using the tooltip to halt other player actions if the tooltip is being current used. 
    /// </summary>
    private bool _isInUse = false;

    private bool _isHoveringOverTooltip = false;


    /// <summary>
    /// Initalize variables and set up btn event handlers for the tooltip. 
    /// </summary>
    private void Awake()
    {
        instance = this;

        _btnSplitHalf = _btnGameObjSplitHalf.GetComponent<Button>();
        _btnSplitOne = _btnGameObjSplit1.GetComponent<Button>();

        _btnSplitHalf.onClick.AddListener(OnSplitHalfBtnClick);
        _btnSplitOne.onClick.AddListener(OnSplitOneBtnClick);
        // default the position to being off the screen
        transform.position = Vector3.zero;
    }


    /// <summary>
    /// Called when player clicks the button splitting a stack of items in an inventory slot container. Calls back to the InvenoryController to
    /// perform the actual splitting.
    /// </summary>
    public void OnSplitHalfBtnClick()
    {
        _Split(SplitMode.HALF);
    }


    /// <summary>
    /// Called when player clicks the button to split a stack of items in an inventory slot container. Calls back to the InvenoryController to
    /// perform the actual splitting.
    /// </summary>
    public void OnSplitOneBtnClick()
    {
        _Split(SplitMode.ONE);
    }


    /// <summary>
    /// This function contains the functionallity that is common to both the 'OnSplitHalfBtnClick' and 'OnSplitOneBtnClick' callbacks.
    /// </summary>
    private void _Split(SplitMode splitMode)
    {
        if (currentContainer == null) return;
        _isInUse = false;
        transform.position = Vector3.zero;
        InventoryController.instance.SplitStackableItem(currentContainer, splitMode);
    }


    /// <summary>
    /// Set the _isHoveringOverToolTip to false so that the OnPointerEnter function can set it again if cursor is over the UITooltip. 
    /// We handle it this way to avoid having this functionallity interfering with the button click callbacks. 
    /// </summary>
    /// <param name="eventData">The system event data for the pointer</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        _isHoveringOverTooltip = false;
    }


    /// <summary>
    /// Allows us to check when the pointer entered the tool tip, so that clicking off the tool tip closes it.
    /// Sets _isHoveringOverToolTip true if pointer enters a game object with the 'UITooltip' tag.
    /// We handle it this way to avoid having this functionallity interfering with the button click callbacks. 
    /// </summary>
    /// <param name="eventData">The system event data for the pointer</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.tag == "UITooltip")
        {
            _isHoveringOverTooltip = true;
        };
    }


    /// <summary>
    /// Allows the InventoryController component to check if cursor is over the tooltip so that clicking outside will close it. 
    /// We handle it this way to avoid having this functionallity interfering with the button click callbacks. 
    /// </summary>
    /// <returns>Returns the value of the _isHoveringOverTookip member</returns>
    public bool IsCursorOverTooltip()
    {
        return _isHoveringOverTooltip;
    }


    /// <summary>
    /// Hides the tooltip by moving it's transform. Sets the _isInUse member to false so we know the tooltip cannot be interacted with. 
    /// </summary>
    public void Hide()
    {
        _isInUse = false;
        transform.position = Vector3.zero;
    }


    /// <summary>
    /// Sets the tool tip to be flagged as in use. Also repositions the UI element to be visible according to the 'targetPosition' specified. 
    /// </summary>
    /// <param name="targetPosition">A vector 3 containing the screen coordinates for positioning the UI tooltip</param>
    public void Show(Vector3 targetPosition)
    {
        _isInUse = true;
        transform.position = targetPosition;
    }


    /// <summary>
    /// Checks if the tooltip is in use based on the '_isInUse' flag.
    /// </summary>
    /// <returns>Returns the current value of the '_isInUse' boolean flag.</returns>
    public bool IsInUse()
    {
        return _isInUse;
    }
}


public enum SplitMode
{ 
   ONE, 
   HALF,
   NONE, 
   ALL,
}