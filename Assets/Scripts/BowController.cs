using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class for managing the state of the a bow when the player has it equiped. 
public class BowController : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float _drawAmount; //Determines how far back the bow is drawn;
    [SerializeField] private Transform _arrowLaunchPos;
    [SerializeField] private Vector3 _arrowStartPos;  //Arrow position when bow is not drawn back at all;
    [SerializeField] private Vector3 _arrowEndPos; //Arrow position when bow is fully drawn.
    [SerializeField] private Vector3 _arrowLocalRot;
    [SerializeField] private GameObject _arrowPrefab; //Todo: have this item populate from the player's inventory.

    [Header("Bow Stats")]
    [SerializeField] public float _bowPower;
    [SerializeField] public float _drawSpeed;


    private GameObject _loadedArrow; //References the arrow game object that is loaded and ready to shoot. 
    private ArrowController _arrowController;
    private Camera _mainCameraReference;


    private void Awake()
    {
        _mainCameraReference = Camera.main;
    }

    //Player calls this to load an arrow onto the bow. TODO: have the arrow game object origonate from the player inventory. 
    public void LoadArrow(GameObject arrowGameObj = null)
    {
        _loadedArrow = arrowGameObj;
        if (_loadedArrow == null)
        {
            _loadedArrow = Instantiate(_arrowPrefab);
        }

        //Disable the arrow's rigidbody until we actually shoot it.
        _arrowController = _loadedArrow.GetComponent<ArrowController>();
        _arrowController.DisablePhysics();

        //Initalize the arrow to align with the bow. 
        _loadedArrow.transform.parent = transform;
        _loadedArrow.transform.localPosition = _arrowStartPos;
        _loadedArrow.transform.localRotation = Quaternion.Euler(_arrowLocalRot);

        _loadedArrow.SetActive(false);
        StartCoroutine(ActivateLoadedArrow());
    }


    private IEnumerator ActivateLoadedArrow()
    {
        yield return new WaitForSeconds(.25f);
        _loadedArrow.SetActive(true);
    }


    //Player calls this to check if an arrow has been loaded onto the bow
    public bool HasLoadedArrow()
    {
        return (_loadedArrow != null) ? true : false;
    }


    //Player calls this to set the draw amount for the bow. 
    public void SetDrawAmount(float drawAmount)
    {
        _drawAmount = drawAmount; 
    }


    //Player calls this when firing the arrow. Arrow velocity is dependant on the bow and the draw amount. 
    public void LooseArrow()
    {
        if (_loadedArrow == null) return;

        //Tell the arrow to active it's rigid body component. 
        _arrowController.EnablePhysics();

        //Give the arrow it's velocity and start position via the ArrowController component on the arrow game object. 
        _arrowController.LaunchArrow(_mainCameraReference.transform.forward.normalized * _bowPower * _drawAmount , _arrowLaunchPos.position);

        //Dereference the arrow since player/bow is no longer controlling it. 
        _loadedArrow = null;
        _arrowController = null;
        _drawAmount = 0f;
    }


    public void Update()
    {
        //paremetrically blend between arrow start and end pos based on the draw amount parameter. 
        if (_loadedArrow != null) 
        {
            _loadedArrow.transform.localPosition = ((1 -  _drawAmount) * _arrowStartPos + _drawAmount * _arrowEndPos) * .05f;           
        }
    }

}

