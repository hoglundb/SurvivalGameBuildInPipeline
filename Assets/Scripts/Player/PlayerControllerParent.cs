using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerControllerParent : MonoBehaviour
    {

        #region PublicMemberVars

        //Track the mode the player is in. This will determine what child components on the player are enabled/disabled. 
        public PlayerModes playerMode = PlayerModes.DEFAULT;

        //Reference to the child component for player base building. Enable this component if in base building mode. 
        public PlayerBaseBuildingController playerBaseBuildingComponent;

        //Reference to the child component for player movement. Enable this component if allowing player to look around and move. 
        public PlayerMovement playerMovementComponent;

        internal Transform rightHandBoneTransform;
       
        #endregion



        #region PrivateMemberVars

        //Reference to the animator component on the player avatar. The avatar is a child game object under the player. 
        private Animator _animator;

        //Singleton Reference so other components can easily 
        public static PlayerControllerParent _instance;

        #endregion



        #region MonoCallbacks

        //Initalize variables and component references
        private void Awake()
        {
            _instance = this;

            playerBaseBuildingComponent = GetComponent<PlayerBaseBuildingController>();
            playerMovementComponent = GetComponent<PlayerMovement>();
          
            Cursor.lockState = CursorLockMode.Confined;
            _animator = GetComponentInChildren<Animator>();

            rightHandBoneTransform = GameObject.Find("RightHand").transform;
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
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (playerMode != PlayerModes.BUILDING)
                {
                    playerMode = PlayerModes.BUILDING;
                    playerBaseBuildingComponent.enabled = true;
                    playerMovementComponent.enabled = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (playerMode == PlayerModes.BUILDING)
                {
                    playerMode = PlayerModes.DEFAULT;
                    playerBaseBuildingComponent.enabled = false;
                    playerMovementComponent.enabled = true;
                }
            }
        }

        #endregion



        #region AnimationFunctions

        //Child components call this to update the player animation
        public void SetAnimationTrigger(string animTrigger)
        {
            _animator.SetTrigger(animTrigger);
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
    }

}