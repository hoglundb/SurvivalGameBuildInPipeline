using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component is attached to the tool tip for splitting an inventory item
/// </summary>
public class SplitInventoryItemsTooltip : MonoBehaviour
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
    public bool isInUse = false;


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

        Debug.LogError("dkfjslkdfj");
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
        isInUse = false;
        transform.position = Vector3.zero;
        InventoryController.instance.SplitStackableItem(currentContainer, splitMode);
    }
}


public enum SplitMode
{ 
   ONE, 
   HALF,
   NONE, 
   ALL,
}