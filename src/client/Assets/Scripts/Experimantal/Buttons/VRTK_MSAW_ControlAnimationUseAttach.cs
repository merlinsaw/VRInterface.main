// Control Animation Grab Attach|GrabAttachMechanics|50100
//namespace VRTK.GrabAttachMechanics
// references
// G:\msaw.main\UnityProject\SteamVR2_VRTK\Assets\VRTK\Examples\ExampleResources\SharedResources\Prefabs\Whirlygig\Scripts\InteractableWhirlyGig.cs
// G:\msaw.main\UnityProject\SteamVR2_VRTK\Assets\VRTK\Source\Scripts\Interactions\Interactables\GrabAttachMechanics\VRTK_ControlAnimationGrabAttach.cs

namespace Assets.Buttons
{
    using UnityEngine;
    using System.Collections;
    using VRTK;

    /// <summary>
    /// Event Payload
    /// </summary>
    /// <param name="interactingObject">The GameObject that is performing the interaction (e.g. a controller).</param>
    /// <param name="currentFrame">The current frame the animation is on.</param>
    public struct ControlAnimationUseAttachEventArgs
    {
        public GameObject interactingObject;
        public float currentFrame;
    }

    /// <summary>
    /// Event Payload
    /// </summary>
    /// <param name="sender">this object</param>
    /// <param name="e"><see cref="ControlAnimationUseAttachEventArgs"/></param>
    public delegate void ControlAnimationUseAttachEventHandler(object sender, ControlAnimationUseAttachEventArgs e);

    /// <summary>
    /// Scrubs through the given animation based on the distance from the grabbing object to the original grabbing point.
    /// </summary>
    /// <remarks>
    /// **Script Usage:**
    ///  * Place the `VRTK_ControlAnimationGrabAttach` script on either:
    ///    * The GameObject of the Interactable Object to detect interactions on.
    ///    * Any other scene GameObject and then link that GameObject to the Interactable Objects `Grab Attach Mechanic Script` parameter to denote use of the grab mechanic.
    ///    * Create and apply an animation via:
    ///      * `Animation Timeline` parameter takes a legacy `Animation` component to use as the timeline to scrub through. The animation must be marked as `legacy` via the inspector in debug mode.
    ///      * `Animator Timeline` parameter takes an Animator component to use as the timeline to scrub through.
    /// </remarks>
    [AddComponentMenu("Custom UI/Interactions/Interactables/Grab Attach Mechanics/VRTK_MSAW_ControlAnimationUseAttach")]
    public class VRTK_MSAW_ControlAnimationUseAttach : MonoBehaviour
    {
        public VRTK_InteractableObject linkedObject;

        [Header("Tracking Settings")]

        [Tooltip("The Player Skeleton root")]
        public Transform neck;

        [Header("Interaction Settings")]

        [Tooltip("The delay till the butten is usable.")]
        public float delayTillUsable;
        public float deleyTillUnusable;

        [Tooltip("If this is checked then when the Interact Grab grabs the Interactable Object, it will grab it with precision and pick it up at the particular point on the Interactable Object that the Interact Touch is touching.")]
        public bool precisionGrab;

        [Header("Animation Settings", order = 2)]

        [Tooltip("An Animation with the timeline to scrub through on grab. If this is set then the `Animator Timeline` will be ignored if it is also set.")]
        public Animation animationTimeline;
        [Tooltip("An Animator with the timeline to scrub through on grab.")]
        public Animator animatorTimeline;
        [Tooltip("The maximum amount of frames in the timeline.")]
        public float maxFrames = 1f;
        [Tooltip("An amount to multiply the distance by to determine the scrubbed frame to be on.")]
        public float distanceMultiplier = 1f;
        [Tooltip("If this is checked then the animation will rewind to the start on ungrab.")]
        public bool rewindOnRelease = false;
        [Tooltip("The speed in which the animation rewind will be multiplied by.")]
        public float rewindSpeedMultplier = 1f;


        protected Transform buttonTrackPoint;
        protected Transform initialAttachPoint;
        protected Transform controller;
        protected bool customTrackPoint;
        //protected Transform initialAttachPoint;
        protected VRTK_InteractableObject grabbedObjectScript;
        //protected Rigidbody controllerAttachPoint;

        protected bool tracked;
        protected bool autoUse;
        protected bool kinematic;

        protected Coroutine delayedUseAction;
        protected Coroutine delayedUnUseAction;

        /// <summary>
        /// Emitted when the Animation Frame is at the start.
        /// </summary>
        public event ControlAnimationUseAttachEventHandler AnimationFrameAtStart;
        /// <summary>
        /// Emitted when the Animation Frame is at the end.
        /// </summary>
        public event ControlAnimationUseAttachEventHandler AnimationFrameAtEnd;
        /// <summary>
        /// Emitted when the Animation Frame has changed.
        /// </summary>
        public event ControlAnimationUseAttachEventHandler AnimationFrameChanged;

        protected float animationSpeed = 0f;
        protected float frameOffset = 0f;
        protected float currentFrame = 0f;
        protected Coroutine resetTimelineRoutine;
        protected bool atEnd = false;
        protected string animationName = "";

       

        //-----------------------------------------------------------------
        //	Animation Frame Events
        //-----------------------------------------------------------------
        public virtual void OnAnimationFrameChanged(ControlAnimationUseAttachEventArgs e)
        {
            if (AnimationFrameChanged != null)
            {
                AnimationFrameChanged(this, e);
            }
        }

        public virtual void OnAnimationFrameAtStart(ControlAnimationUseAttachEventArgs e)
        {
            if (AnimationFrameAtStart != null)
            {
                AnimationFrameAtStart(this, e);
            }
        }

        public virtual void OnAnimationFrameAtEnd(ControlAnimationUseAttachEventArgs e)
        {
            if (AnimationFrameAtEnd != null)
            {
                AnimationFrameAtEnd(this, e);
            }
        }
        //-----------------------------------------------------------------
        //	                            TOUCHED
        //-----------------------------------------------------------------
        protected virtual void InteractableObjectTouched(object sender, InteractableObjectEventArgs e)
        {
            CancelDelayedUnUseAction();
            delayedUseAction = StartCoroutine(DelayedUseAction(sender, e, delayTillUsable));
            //autoGrab.grabOnTouchWhen = VRTK_ObjectTouchAutoInteract.AutoInteractions.NoButtonHeld;
            // grab the object

            //linkedObject.InteractableObjectTouched -= InteractableObjectTouched;
            

            Debug.Log($"<Color=yellow> {sender}, {e} </Color>");
        }
        #region Delayed Using

        protected virtual void CancelDelayedUseAction()
        {
            if (delayedUseAction != null)
            {
                StopCoroutine(delayedUseAction);
            }
        }

        protected virtual IEnumerator DelayedUseAction(object sender, InteractableObjectEventArgs e, float deleay)
        {

            float
                fadeTime = deleay,
                elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            // grab the object
            
            InteractableObjectUsed(sender, e);
            //linkedObject.InteractableObjectUsed += InteractableObjectUsed;
            RewindAnimation();
            //linkedObject.InteractableObjectUnused += InteractableObjectUnused;
            //autoGrab.useOnTouchWhen = VRTK_ObjectTouchAutoInteract.AutoInteractions.NoButtonHeld;
            Debug.Log($"+Delayed Use Action+ {delayTillUsable}");
        }
        #endregion

        //-----------------------------------------------------------------
        //	                            NOT TOUCHED
        //-----------------------------------------------------------------
        protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
        {
            CancelDelayedUseAction();
            delayedUnUseAction = StartCoroutine(DelayedUnUseAction(sender, e, deleyTillUnusable));

            Debug.Log($"<Color=white> {sender}, {e} </Color>");
            autoUse = true;
            //autoGrab.grabOnTouchWhen = VRTK_ObjectTouchAutoInteract.AutoInteractions.Never;

            //InteractableObjectUnused(sender, e);
            //this.StopAllCoroutines();


            //linkedObject.InteractableObjectUsed -= InteractableObjectUsed;
            //linkedObject.InteractableObjectUnused -= InteractableObjectUnused;
        }

        #region Delayed Un Using

        protected virtual void CancelDelayedUnUseAction()
        {
            if (delayedUnUseAction != null)
            {
                StopCoroutine(delayedUnUseAction);
            }
        }

        protected virtual IEnumerator DelayedUnUseAction(object sender, InteractableObjectEventArgs e, float deleay)
        {

            float
                fadeTime = deleay,
                elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            // grab the object
            InteractableObjectUnused(sender, e);
            linkedObject.InteractableObjectUsed -= InteractableObjectUsed;
            RewindAnimation();
            //linkedObject.InteractableObjectUnused += InteractableObjectUnused;
            //autoGrab.useOnTouchWhen = VRTK_ObjectTouchAutoInteract.AutoInteractions.NoButtonHeld;
            Debug.Log($"-Delayed Un Use Action- {deleyTillUnusable}");
        }
        #endregion
        //-----------------------------------------------------------------
        //	                                USED
        //-----------------------------------------------------------------
        protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
        {
            if (tracked == false)
            {


                tracked = true;
                controller = e.interactingObject.transform;

                Debug.Log("Controller: " + controller.name);
                Debug.Log("Button: " + gameObject.name);//(sender as GameObject).name);
                Debug.Log("<Color=red>+++Used+++</Color>");

                SetTrackPointPosition(gameObject.transform, buttonTrackPoint);
                SetTrackPointPosition(controller, initialAttachPoint);

                atEnd = false;
            }
        }

        //-----------------------------------------------------------------
        //                                NOT USED
        //-----------------------------------------------------------------
        protected virtual void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
        {
            if (tracked == true)
            {
                tracked = false;
                Debug.Log("<Color=blue>---Unused---</Color>");
                frameOffset = currentFrame;
                if (rewindOnRelease)
                {
                    RewindAnimation();
                }

                //grabbedObject = null;
                //grabbedObjectScript = null;
                //buttonTrackPoint = null;
                //grabbedSnapHandle = null;
                //initialAttachPoint = null;
                //controllerAttachPoint = null;
            }
        }

        /// <summary>
        /// The CreateTrackPoint method sets up the point of grab to track on the grabbed object.
        /// </summary>
        /// <param name="controllerPoint">The point on the controller where the grab was initiated.</param>
        /// <param name="currentGrabbedObject">The GameObject that is currently being grabbed.</param>
        /// <param name="currentGrabbingObject">The GameObject that is currently doing the grabbing.</param>
        /// <param name="customTrackPoint">A reference to whether the created track point is an auto generated custom object.</param>
        /// <returns>The Transform of the created track point.</returns>
        public Transform CreateTrackPoint(string name ,ref bool customTrackPoint, Transform p = null)
        {
            Transform returnTrackpoint = null;
            customTrackPoint = true;
            returnTrackpoint = new GameObject(VRTK_SharedMethods.GenerateVRTKObjectName(true, name, "ControlAnimation", "AttachPoint")).transform;
            returnTrackpoint.SetParent(p);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.SetParent(returnTrackpoint);
            sphere.transform.GetComponent<Collider>().enabled = false;
            sphere.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
            //returnTrackpoint.position = (precisionGrab ? controllerPoint.position : currentGrabbedObject.transform.position);
            return returnTrackpoint;
        }
        /// <summary>
        /// The SetTrackPointPosition sets the track point to the controller position.
        /// </summary>
        /// <param name="controllerPoint">The point on the controller where the grab was initiated.</param>
        /// <param name="trackPoint">The point that will be used for distance mesurement of the controller.</param>
        public void SetTrackPointPosition(Transform controllerPoint, Transform trackPoint)
        {
            if (trackPoint != null)
            {
                trackPoint.position = controllerPoint.position;
            }


        }

        /// <summary>
        /// The ProcessUpdate method is run in every Update method on the Interactable Object.
        /// </summary>
        public void LateUpdate()
        {
            if (buttonTrackPoint != null && tracked && linkedObject.IsTouched()==true)
            {
                {
                    //+              *---+ 
                    
                    float grabDistance =
                        Vector3.Distance(initialAttachPoint.position, buttonTrackPoint.position)
                        - Vector3.Distance(buttonTrackPoint.position, controller.position);
                    SetFrame(grabDistance + frameOffset);
                }
            }
        }

        /// <summary>
        /// The SetFrame method scrubs to the specific frame of the Animator timeline.
        /// </summary>
        /// <param name="frame">The frame to scrub to.</param>
        public virtual void SetFrame(float frame)
        {
            float setFrame = frame * distanceMultiplier;
            SetTimelineSpeed(animationSpeed);
            if (setFrame < maxFrames)
            {
                SetTimelinePosition(setFrame);
                if (setFrame == 0)
                {
                    OnAnimationFrameAtStart(SetEventPayload(setFrame));
                }
                OnAnimationFrameChanged(SetEventPayload(setFrame));
                currentFrame = frame;
                atEnd = false;
            }
            else if (!atEnd)
            {
                OnAnimationFrameAtEnd(SetEventPayload(setFrame));
                atEnd = true;
            }
        }

        /// <summary>
        /// The RewindAnimation method will force the animation to rewind to the start frame.
        /// </summary>
        public virtual void RewindAnimation()
        {
            CancelResetTimeline();
            resetTimelineRoutine = StartCoroutine(ResetTimeline(currentFrame));
        }

        protected virtual void OnEnable()
        {
            linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

            if (linkedObject != null)
            {
                //linkedObject.InteractableObjectUsed += InteractableObjectUsed;
                //linkedObject.InteractableObjectUnused += InteractableObjectUnused;

                linkedObject.InteractableObjectTouched += InteractableObjectTouched;
                linkedObject.InteractableObjectUntouched += InteractableObjectUntouched;
            }
            // TODO: @UI_interaction: the UI sould create the UI Trackpoint to be used all over again
            buttonTrackPoint = CreateTrackPoint("Button", ref customTrackPoint, neck);
            initialAttachPoint = CreateTrackPoint("initialAttachPoint", ref customTrackPoint, neck);
        }

        protected virtual void OnDisable()
        {

            if (linkedObject != null)
            {
                //linkedObject.InteractableObjectUsed -= InteractableObjectUsed;
                //linkedObject.InteractableObjectUnused -= InteractableObjectUnused;

                linkedObject.InteractableObjectTouched -= InteractableObjectTouched;
                linkedObject.InteractableObjectUntouched -= InteractableObjectUntouched;
            }

            CancelResetTimeline();
        }



        #region Animation Control
        //--------------------------------------------------------
        protected void Initialise()
        {
            tracked = false;
            kinematic = true;
            InitTimeline();
        }


        protected virtual void InitTimeline()
        {
            animatorTimeline = (animatorTimeline != null ? animatorTimeline : GetComponent<Animator>());
            animationTimeline = (animationTimeline != null ? animationTimeline : GetComponent<Animation>());
            if (animationTimeline != null)
            {
                if (!animationTimeline.clip.legacy)
                {
                    VRTK_Logger.Error("The `VRTK_ControlAnimationGrabAttach` script is using an `Animation Timeline` that has not been set to `Legacy Animation`. Only legacy animations are supported.");
                }

                foreach (AnimationState currentClip in animationTimeline)
                {
                    animationName = currentClip.name;
                    break;
                }
            }
            SetTimelineSpeed(animationSpeed);
        }

        protected virtual void SetTimelineSpeed(float speed)
        {
            if (animationTimeline != null)
            {
                animationTimeline[animationName].speed = speed;
            }
            else if (animatorTimeline != null)
            {
                animatorTimeline.speed = speed;
            }
        }

        protected virtual void SetTimelinePosition(float framePosition)
        {
            if (animationTimeline != null)
            {
                animationTimeline[animationName].time = framePosition;
                animationTimeline.Play(animationName);
            }
            else if (animatorTimeline != null)
            {
                animatorTimeline.Play(0, 0, framePosition);
            }
        }

        protected virtual void CancelResetTimeline()
        {
            if (resetTimelineRoutine != null)
            {
                StopCoroutine(resetTimelineRoutine);
            }
        }

        protected virtual IEnumerator ResetTimeline(float frame)
        {
            while (frame > 0f)
            {
                SetFrame(frame);
                frame -= Time.fixedDeltaTime * rewindSpeedMultplier;
                frameOffset = currentFrame;
                yield return null;
            }
            SetFrame(0f);
        }

        #endregion

        protected virtual ControlAnimationUseAttachEventArgs SetEventPayload(float frame)
        {
            ControlAnimationUseAttachEventArgs e;
            e.interactingObject = (grabbedObjectScript != null ? grabbedObjectScript.GetGrabbingObject() : null);
            e.currentFrame = frame;
            return e;
        }
    }
}