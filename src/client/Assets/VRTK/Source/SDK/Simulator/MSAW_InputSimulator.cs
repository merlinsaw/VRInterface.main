////-------------------------------------------------------
////	attemting to adept the 
////  InputSimulator
////  to be skelton and event driven
////  G:\msaw.main\UnityProject\SteamVR2_VRTK\Assets\VRTK\Source\SDK\Simulator\SDK_InputSimulator.cs
////-------------------------------------------------------

//#region using

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using UnityEngine;
//using UnityEngine.UI;
//using Valve.VR;

//#endregion
///// <summary>
/////  skeleton driven hands and interacting with objects without the need of a hmd or VR controls.
///// </summary>
//namespace VRTK

//{
//    public class MSAW_InputSimulator : MonoBehaviour
//    {


//        #region Public fields
//        [Header("Genaral Settings")]

//        [Tooltip("Show control information in the upper left corner of the screen.")]
//        public bool showControlHints = true;
//        [Tooltip("Display an acis helper to show which axis the hands will be moved through")]
//        public bool showHandAxisHelpers = true;

//        [Header("Mouse Curser Lock Settings")]

//        [Tooltip("Lock the mouse cursor to the game window.")]
//        public bool lockMouseToViw = true;

//        [Header("Manual Adjustment Settings")]

//        [Tooltip("The Colour of the GameObject representing the left hand.")]
//        public Color leftHandColor = Color.red;
//        [Tooltip("The Colour of the GameObject representing the right hand.")]
//        public Color rightHandColor = Color.green;

//        [Header("Operation Key Binding Settings")]

//        [Tooltip("Key used to enable mouse input if a button press is required.")]
//        public KeyCode mouseMovementKey = KeyCode.Mouse1;
//        [Tooltip("Key used to toggle control hints on/off.")]
//        public KeyCode toggleControlHints = KeyCode.F1;
//        [Tooltip("Key used to toggle control hints on/off.")]
//        public KeyCode toggleMouseLock = KeyCode.F4;
//        [Tooltip("Key used to switch between left and righ hand.")]
//        public KeyCode changeHands = KeyCode.Tab;
//        [Tooltip("Key used to switch hands On/Off.")]
//        public KeyCode handsOnOff = KeyCode.LeftAlt;
//        [Tooltip("Key used to switch between positional and rotational movement.")]
//        public KeyCode rotationPosition = KeyCode.LeftShift;
//        [Tooltip("Key used to switch between X/Y and X/Z axis.")]
//        public KeyCode changeAxis = KeyCode.LeftControl;
//        [Tooltip("Key used to distance pickup with left hand.")]
//        public KeyCode distancePickupLeft = KeyCode.Mouse0;
//        [Tooltip("Key used to distance pickup with right hand.")]
//        public KeyCode distancePickupRight = KeyCode.Mouse1;
//        [Tooltip("Key used to enable distance pickup.")]
//        public KeyCode distancePickupModifier = KeyCode.LeftControl;

//        [Header("Controller Bone Bindings")]

//        [Tooltip("Neck")]
//        public Transform neckBone;
//        [Tooltip("Left Hand Bone")]
//        public Transform leftHandBone;
//        [Tooltip("Right Hand Bone")]
//        public Transform rightHandBone;

//        [Header("Controller Key Binding Settings")]

//        [Tooltip("Key used to simulate trigger button.")]
//        public KeyCode triggerAlias = KeyCode.Mouse1;
//        [Tooltip("Key used to simulate grip button.")]
//        public KeyCode gripAlias = KeyCode.Mouse0;
//        #endregion

//        #region protected fields

//        protected GameObject hintCanvas;
//        protected Text hintText;

//        protected Transform rightHand;
//        protected Transform leftHand;

//        protected Vector3 rightHandOldPos;
//        protected Vector3 leftHandOldPos;

//        protected Transform neck;

//        protected SDK_ControllerSim rightController;
//        protected SDK_ControllerSim leftController;

//        protected static GameObject cachedCameraRig;
//        protected static bool destroyed = false;

//        protected GameObject crossHairPanel;
//        protected Transform leftHandHorizontalAxisGuide;
//        protected Transform leftHandVerticalAxisGuide;
//        protected Transform rightHandHorizontalAxisGuide;
//        protected Transform rightHandVerticalAxisGuide;

//        #endregion

//        /// <summary>
//        /// The FindInScene method is used to find the `[VRSimulator_CameraRig]` GameObject within the current scene.
//        /// </summary>
//        /// <returns>Returns the found `[VRSimulator_CameraRig]` GameObject if it is found. If it is not found then it prints a debug log error.</returns>
//        public static GameObject FindInScene()
//        {
//            if (cachedCameraRig == null && !destroyed)
//            {
//                cachedCameraRig = VRTK_SharedMethods.FindEvenInactiveGameObject<SDK_InputSimulator>(null, true);
//                if (!cachedCameraRig)
//                {
//                    VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_SCENE, "[VRSimulator_CameraRig]", "SDK_InputSimulator", ". check that the `VRTK/Prefabs/CameraRigs/[VRSimulator_CameraRig]` prefab been added to the scene."));
//                }
//            }
//            return cachedCameraRig;
//        }

//        protected virtual void Awake()
//        {
//            VRTK_SDKManager.AttemptAddBehaviourToToggleOnLoadedSetupChange(this);
//        }

//        protected virtual void OnEnable()
//        {
//            hintCanvas = transform.Find("Canvas/Control Hints").gameObject;
//            crossHairPanel = transform.Find("Canvas/CrosshairPanel").gameObject;
//            hintText = hintCanvas.GetComponentInChildren<Text>();
//            hintCanvas.SetActive(showControlHints);
//            rightHand = transform.Find("RightHand");
//            rightHand.gameObject.SetActive(true);
//            leftHand = transform.Find("LeftHand");
//            leftHand.gameObject.SetActive(true);
//            leftHandHorizontalAxisGuide = leftHand.Find("Guides/HorizontalPlane");
//            leftHandVerticalAxisGuide = leftHand.Find("Guides/VerticalPlane");
//            rightHandHorizontalAxisGuide = rightHand.Find("Guides/HorizontalPlane");
//            rightHandVerticalAxisGuide = rightHand.Find("Guides/VerticalPlane");
//            //currentHand = rightHand;
//            //oldPos = Input.mousePosition;
//            rightHandOldPos = rightHandBone.position;
//            leftHandOldPos = leftHandBone.position;
//            neck = transform.Find("Neck");

//            SetHandColor(leftHand, leftHandColor);
//            SetHandColor(rightHand, rightHandColor);
//            rightController = rightHand.GetComponent<SDK_ControllerSim>();
//            leftController = leftHand.GetComponent<SDK_ControllerSim>();

//            //rightController.selected = true;
//            //leftController.selected = false;
//            //destroyed = false;

//            SDK_SimController controllerSDK = VRTK_SDK_Bridge.GetControllerSDK() as SDK_SimController;
//            if (controllerSDK != null)
//            {
//                Dictionary<string, KeyCode> keyMappings = new Dictionary<string, KeyCode>()
//                {
//                    // can it be that i need to send something else here?
//                    {"Trigger", triggerAlias },
//                    {"Grip", gripAlias },
//                };
//                controllerSDK.SetKeyMappings(keyMappings);
//            }
//            rightHand.gameObject.SetActive(true);
//            leftHand.gameObject.SetActive(true);
//            crossHairPanel.SetActive(false);
//        }

//        protected virtual void OnDestroy()
//        {
//            VRTK_SDKManager.AttemptRemoveBehaviourToToggleOnLoadedSetupChange(this);
//            destroyed = true;
//        }



//        protected virtual void Update()
//        {

//            if (Input.GetKeyDown(toggleControlHints))
//            {
//                showControlHints = !showControlHints;
//                hintCanvas.SetActive(showControlHints);
//            }


//            // actually both hands are moved simultanious
//            rightController.selected = true;
//            leftController.selected = false;

//            // get the bone position and rotation

//            // movehands to position and rotation

//            // display axis modles





//            if (Input.GetKeyDown(toggleMouseLock))
//            {
//                lockMouseToView = !lockMouseToView;
//            }


//            if (mouseMovementInput == MouseInputMode.RequiresButtonPress)
//            {
//                if (lockMouseToView)
//                {
//                    Cursor.lockState = Input.GetKey(mouseMovementKey) ? CursorLockMode.Locked : CursorLockMode.None;
//                }
//                else if (Input.GetKeyDown(mouseMovementKey))
//                {
//                    oldPos = Input.mousePosition;
//                }
//            }
//            else
//            {
//                Cursor.lockState = lockMouseToView ? CursorLockMode.Locked : CursorLockMode.None;
//            }

//            if (Input.GetKeyDown(handsOnOff))
//            {
//                if (isHand)
//                {
//                    SetMove();
//                    ToggleGuidePlanes(false, false);
//                }
//                else
//                {
//                    SetHand();
//                }
//            }

//            if (Input.GetKeyDown(changeHands))
//            {
//                if (currentHand.name == "LeftHand")
//                {
//                    currentHand = rightHand;
//                    rightController.selected = true;
//                    leftController.selected = false;
//                }
//                else
//                {
//                    currentHand = leftHand;
//                    rightController.selected = false;
//                    leftController.selected = true;
//                }
//            }

//            if (isHand)
//            {
//                UpdateHands();
//            }
//            else
//            {
//                UpdateRotation();
//                if (Input.GetKeyDown(distancePickupRight) && Input.GetKey(distancePickupModifier))
//                {
//                    TryPickup(true);
//                }
//                else if (Input.GetKeyDown(distancePickupLeft) && Input.GetKey(distancePickupModifier))
//                {
//                    TryPickup(false);
//                }

//                if (Input.GetKeyDown(distancePickupModifier))
//                {
//                    crossHairPanel.SetActive(true);
//                }
//                else if (Input.GetKeyUp(distancePickupModifier))
//                {
//                    crossHairPanel.SetActive(false);
//                }
//            }

//            UpdatePosition();

//            if (showControlHints)
//            {
//                UpdateHints();
//            }
//        }






//        #region visual aids

//        protected virtual void SetHandColor(Transform hand, Color givenColor)
//        {
//            Transform foundHand = hand.Find("Hand");
//            if (foundHand != null && givenColor != Color.clear)
//            {
//                Renderer[] renderers = foundHand.GetComponentsInChildren<Renderer>(true);
//                for (int i = 0; i < renderers.Length; i++)
//                {
//                    renderers[i].material.color = givenColor;
//                }
//            }
//        }

//        #endregion

//        #region hints

//        protected virtual void UpdateHints()
//        {
//            string hints = "";
//            Func<KeyCode, string> key = (k) => "<b>" + k.ToString() + "</b>";

//            string mouseInputRequires = "";
//            mouseInputRequires = " (" + key(mouseMovementKey) + ")";


//            // WASD Movement
//            //string movementKeys = moveForward.ToString() + moveLeft.ToString() + moveBackward.ToString() + moveRight.ToString();
//            hints += "Toggle Control Hints: " + key(toggleControlHints) + "\n\n";
//            hints += "Toggle Mouse Lock: " + key(toggleMouseLock) + "\n";
//            //hints += "Move Player/Playspace: <b>" + movementKeys + "</b>\n";
//            //hints += "Sprint Modifier: (" + key(sprint) + ")\n\n";

//            if (isHand)
//            {
//                // Controllers
//                if (Input.GetKey(rotationPosition))
//                {
//                    hints += "Mouse: <b>Controller Rotation" + mouseInputRequires + "</b>\n";
//                }
//                else
//                {
//                    hints += "Mouse: <b>Controller Position" + mouseInputRequires + "</b>\n";
//                }
//                hints += "Modes: HMD (" + key(handsOnOff) + "), Rotation (" + key(rotationPosition) + ")\n";

//                hints += "Controller Hand: " + currentHand.name.Replace("Hand", "") + " (" + key(changeHands) + ")\n";

//                string axis = Input.GetKey(changeAxis) ? "X/Y" : "X/Z";
//                hints += "Axis: " + axis + " (" + key(changeAxis) + ")\n";

//                // Controller Buttons
//                string pressMode = "Press";

//                hints += "Trigger " + pressMode + ": " + key(triggerAlias) + "\n";
//                hints += "Grip " + pressMode + ": " + key(gripAlias) + "\n";

//            }
//            else
//            {
//                // HMD Input
//                hints += "Mouse: <b>HMD Rotation" + mouseInputRequires + "</b>\n";
//                hints += "Modes: Controller (" + key(handsOnOff) + ")\n";
//                hints += "Distance Pickup Modifier: (" + key(distancePickupModifier) + ")\n";
//                hints += "Distance Pickup Left Hand: (" + key(distancePickupLeft) + ")\n";
//                hints += "Distance Pickup Right Hand: (" + key(distancePickupRight) + ")\n";
//            }

//            hintText.text = hints.TrimEnd();


//        }

//        #endregion
//    }
//}
