using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /****************************************************************************************
     This component attaches to the Player game object. It mannages all the other components on 
     the player and allows them to communicate with each other. 
   ****************************************************************************************/
    public class PlayerControllerParent : MonoBehaviour
    {

        #region PublicMemberVars

        //Track the mode the player is in. This will determine what child components on the player are enabled/disabled. 
        public PlayerModes playerMode = PlayerModes.DEFAULT;

        //Reference to the child component for player base building. Enable this component if in base building mode. 
        public PlayerBaseBuildingController playerBaseBuildingComponent;

        //Reference to the child component for player movement. Enable this component if allowing player to look around and move. 
        public PlayerMovement playerMovementComponent;

        //Reference to the child component that is responsible for detecting the envoronment where the player is looking 
        public EnvironmentDetector playerEnvironmentDetectorComponent;
       
        #endregion



        #region PrivateMemberVars

        //Reference to the animator component on the player avatar. The avatar is a child game object under the player. 
        private Animator _animator;

        //Singleton Reference so other components can easily 
        public static PlayerControllerParent _instance;

        //Reference the hand bones to have player hold equipable objects
        private Transform _leftHandBoneTransform;
        private Transform _rightHandBoneTransform;

        #endregion


        public Transform GetLeftHandBone()
        {
            return _leftHandBoneTransform;
        }


        public Transform GetRightHandBone()
        {
            return _rightHandBoneTransform;
        }



        #region MonoCallbacks

        //Initalize variables and component references
        private void Awake()
        {
            _instance = this;

            playerBaseBuildingComponent = GetComponent<PlayerBaseBuildingController>();
            playerMovementComponent = GetComponent<PlayerMovement>();
            playerEnvironmentDetectorComponent = GetComponent<EnvironmentDetector>();

            Cursor.lockState = CursorLockMode.Confined;
            _animator = GetComponentInChildren<Animator>();

            _rightHandBoneTransform = GameObject.Find("hand.R").transform;
            _leftHandBoneTransform = GameObject.Find("hand.L").transform;
            //Start off the player in idle animation
            SetAnimationTrigger("Idle");
        }


        private void Start()
        {

            //Load saved data
            SaveGame.GetInstance().LoadGameFromPlayerPrefs();
        }


        // Do stuff that needs done every frame
        void Update()
        {
            _PlayerInputResponseUpdate();
        }

        #endregion



        #region PlayerInputResponse

        //Takes keyboard input and enables/disables child components on the player accordinly. 
        private void _PlayerInputResponseUpdate()
        {
            //If player toggles base building mode, activate/deactivate the child components accordinly. 
            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    if (playerMode != PlayerModes.BUILDING)
            //    {
            //        playerMode = PlayerModes.BUILDING;
            //        playerBaseBuildingComponent.enabled = true;
            //        playerMovementComponent.enabled = false;
            //    }
            //}
            //else if (Input.GetKeyDown(KeyCode.Tab))
            //{
            //    if (playerMode == PlayerModes.BUILDING)
            //    {
            //        playerMode = PlayerModes.DEFAULT;
            //        playerBaseBuildingComponent.enabled = false;
            //        playerMovementComponent.enabled = true;
            //    }
            //}
        }



        //public void EnableBaseBuilding()
        //{
        //    playerBaseBuildingComponent.enabled = true;
        //}


        //public void DisableBaseBuidling()
        //{
        //    playerBaseBuildingComponent.enabled = false;
        //}



        #endregion



        #region AnimationFunctions

        //Child components call this to update the player animation
        public void SetAnimationTrigger(string animTrigger)
        {
            _animator.SetTrigger(animTrigger);
        }


        public void SetAnimationFloat(string paramName, float amount)
        {
            _animator.SetFloat(paramName, amount);
        }

        //Returns the name of the current animation on the player avatar
        public string GetCurrentPlayerAnimationName()
        {
            return "Not yet implemented";
        }


        //Returns the normalized time through the current animation clip on the player avatar. 
        public float GetPercentThroughCurrentAnimation()
        {
            return 0f;
        }

        #endregion



        #region StaticFunctions
        public static PlayerControllerParent GetInstance()
        {
            return _instance;
        }

        #endregion


        #region Interact With Other Components

        public void SetMovementEnablement(bool isEnabled)
        {
            playerMovementComponent.SetMovementEnablement(isEnabled);
        }

        #endregion
    }

}