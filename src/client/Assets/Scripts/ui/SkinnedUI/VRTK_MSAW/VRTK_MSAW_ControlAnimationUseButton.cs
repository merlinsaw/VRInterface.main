// Control Animation Grab Attach|GrabAttachMechanics|50100
// namespace VRTK.GrabAttachMechanics
// references
// G:\msaw.main\UnityProject\SteamVR2_VRTK\Assets\VRTK\Examples\ExampleResources\SharedResources\Prefabs\Whirlygig\Scripts\InteractableWhirlyGig.cs
// G:\msaw.main\UnityProject\SteamVR2_VRTK\Assets\VRTK\Source\Scripts\Interactions\Interactables\GrabAttachMechanics\VRTK_ControlAnimationGrabAttach.cs

using UnityEngine;
using System.Collections;
using VRTK;
using SGEventSysthem; // Listens to the skeleton gesture events

namespace skeleton.Events
{
    /// <summary>
    /// Event Payload
    /// </summary>
    /// <param name="interactingObject">The GameObject that is performing the interaction (e.g. a controller).</param>
    /// <param name="currentFrame">The current frame the animation is on.</param>
    public struct ControlAnimationUseButtonEventArgs
    {
        public GameObject interactingObject;
        public float currentFrame;
    }

    /// <summary>
    /// Event Payload
    /// </summary>
    /// <param name="sender">this object</param>
    /// <param name="e"><see cref="ControlAnimationUseButtonEventArgs"/></param>
    public delegate void ControlAnimationUseButtonEventHandler(object sender, ControlAnimationUseButtonEventArgs e);

    /// <summary>
    /// Scrubs through the given animation based on the progress of the push gesture.
    /// </summary>
    /// <remarks>
    /// **Script Usage:**
    ///  * Place the `VRTK_MSAW_ControlAnimationUseButton` script on either:
    ///    * The GameObject of the Interactable Object to detect interactions on.
    ///    * Any other scene GameObject and then link that GameObject to the Interactable Objects `Grab Attach Mechanic Script` parameter to denote use of the grab mechanic.
    ///    * Create and apply an animation via:
    ///      * `Animation Timeline` parameter takes a legacy `Animation` component to use as the timeline to scrub through. The animation must be marked as `legacy` via the inspector in debug mode.
    ///      * `Animator Timeline` parameter takes an Animator component to use as the timeline to scrub through.
    /// </remarks>
    [AddComponentMenu("Custom UI/Interactions/Interactables/Use Animate Mechanics/VRTK_MSAW_ControlAnimationUseButton")]
    [RequireComponent(typeof(VRTK_InteractableObject))]
    public class VRTK_MSAW_ControlAnimationUseButton : MonoBehaviour
    {
        private static readonly _Logger log = new _Logger(typeof(VRTK_MSAW_ControlAnimationUseButton));

        #region Fileds

        [Tooltip("The interacting object touching this button")]
        public VRTK_InteractableObject linkedObject;


        [Header("Interaction Settings")]

        [Tooltip("The delay till the butten is usable.")]
        public float delayTillUsable = 0.05f;
        [Tooltip("The delay till the butten is no more usable.")]
        public float delayTillUnusable = 0.05f;

        [Header("Animation Settings", order = 2)]

        [Tooltip("An Animation with the timeline to scrub through on using. If this is set then the `Animator Timeline` will be ignored if it is also set.")]
        public Animation animationTimeline;
        [Tooltip("An Animator with the timeline to scrub through on using.")]
        public Animator animatorTimeline;
        [Tooltip("The maximum amount of frames in the timeline.")]
        public float maxFrames = 1f;
        [Tooltip("An amount to multiply the distance by to determine the scrubbed frame to be on.")]
        public float distanceMultiplier = 1f;
        [Tooltip("If this is checked then the animation will rewind to the start on ungrab.")]
        public bool rewindOnRelease = false;
        [Tooltip("The speed in which the animation rewind will be multiplied by.")]
        public float rewindSpeedMultplier = 1f;


        protected Transform controller;
        protected VRTK_InteractableObject grabbedObjectScript;

        protected bool isUsed;

        protected Coroutine delayedUseAction;
        protected Coroutine delayedUnUseAction;

        [SerializeField]
        protected float animationSpeed = 0f;
        protected float frameOffset = 0f;
        protected float currentFrame = 0f;
        protected Coroutine resetTimelineRoutine;
        protected bool atEnd = false;
        protected string animationName = "";

        #endregion

        /// <summary>
        /// Emitted when the Animation Frame is at the start.
        /// </summary>
        public event ControlAnimationUseButtonEventHandler AnimationFrameAtStart;
        /// <summary>
        /// Emitted when the Animation Frame is at the end.
        /// </summary>
        public event ControlAnimationUseButtonEventHandler AnimationFrameAtEnd;
        /// <summary>
        /// Emitted when the Animation Frame has changed.
        /// </summary>
        public event ControlAnimationUseButtonEventHandler AnimationFrameChanged;

        //-----------------------------------------------------------------
        //	Animation Frame Events
        //-----------------------------------------------------------------
        public virtual void OnAnimationFrameChanged(ControlAnimationUseButtonEventArgs e)
        {
            AnimationFrameChanged?.Invoke(this, e);
        }

        public virtual void OnAnimationFrameAtStart(ControlAnimationUseButtonEventArgs e)
        {
            AnimationFrameAtStart?.Invoke(this, e);
        }

        public virtual void OnAnimationFrameAtEnd(ControlAnimationUseButtonEventArgs e)
        {
            AnimationFrameAtEnd?.Invoke(this, e);
        }
        //-----------------------------------------------------------------
        //	                            TOUCHED
        //-----------------------------------------------------------------
        protected virtual void InteractableObjectTouched(object sender, InteractableObjectEventArgs e)
        {
            CancelDelayedUnUseAction();
            if (isUsed == false)
                delayedUseAction = StartCoroutine(DelayedUseAction(sender, e, delayTillUsable));

            //log.InfoMS($"<Color=yellow>\n{gameObject.name}, Touched ******************************************* </Color> \n {e.interactingObject}");
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
            //CheckUse(e.interactingObject);
            linkedObject.OnInteractableObjectUsed(e);

            // TODO: @msaw - figure out if we need the animation reset or if it's stalling the animation
            //RewindAnimation(); 


            log.InfoMS($"+Delayed Use Action+ {delayTillUsable}");
        }
        #endregion

        //-----------------------------------------------------------------
        //	                                USED
        //-----------------------------------------------------------------
        protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
        {
            isUsed = true;

            SGPushStartedEvent.RegisterListener(OnSGPushStarted);
            SGPushChangedEvent.RegisterListener(OnSGPushChanged);
            SGPushEndedEvent.RegisterListener(OnSGPushEnded);

            controller = e.interactingObject.transform;

            //log.InfoMS("Controller: " + controller.name);
            //log.InfoMS("Button: " + gameObject.name);//(sender as GameObject).name);
            //log.InfoMS("<Color=red>\n+++Used+++</Color>");

            

            atEnd = false;
        }

        //-----------------------------------------------------------------
        //	                           PUSH GESTURE
        //-----------------------------------------------------------------

        #region Push Gesture Methods

        private SGPushStartedEvent sGPushStartedEvent = new SGPushStartedEvent();
        void OnSGPushStarted(SGPushStartedEvent gesture)
        {
            sGPushStartedEvent = gesture;
            gesture.Listener = gameObject;
            gesture.Description += gameObject.name;
            log.InfoMS($"<Color=magenta>\n{gesture.Description}</Color>");
            
            SetFrame(0);
            animatorTimeline.Play("Pressed", 0, 0);
            SetTimelineSpeed(0);
        }

        void OnSGPushChanged(SGPushChangedEvent gesture)
        {
            // |<-        +---+  O 
            gesture.Listener = gameObject;
            gesture.Description += gameObject.name;
            log.InfoMS($"<Color=yellow>\n{gesture.Description}</Color> Frame: {gesture.PushDistance}");
            SetFrame(gesture.PushDistance);
        }

        void OnSGPushEnded(SGPushEndedEvent gesture)
        {
            gesture.Listener = gameObject;
            gesture.Description += gameObject.name;
            log.InfoMS($"<Color=red>\n{gesture.Description}</Color>");
            
            animatorTimeline.Play("Selected", 0, 0);
            SetTimelineSpeed(1);
        }

        //TODO: @msaw - when the gesture is canceled the animation could be rewinded
        void OnSGPushCanceled()
        {
            sGPushStartedEvent.ResetEvent();
            sGPushStartedEvent.Description = 
                (sGPushStartedEvent.Listener == null ? gameObject.name : sGPushStartedEvent.Listener.name) 
                + " Cancelled the Push from " + 
                (sGPushStartedEvent.Sender == null ? "" : sGPushStartedEvent.Sender.name);

            log.InfoMS($"<Color=red>\n{sGPushStartedEvent.Description}</Color>");
        }
        #endregion

        //-----------------------------------------------------------------
        //	                            NOT TOUCHED
        //-----------------------------------------------------------------
        protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
        {
            CancelDelayedUseAction();
            if (isUsed == true)
                delayedUnUseAction = StartCoroutine(DelayedUnUseAction(sender, e, delayTillUnusable));

            //log.InfoMS($"<Color=yellow>\n{gameObject.name}, Released ##################################### </Color>");
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
            // release the object
            InteractableObjectUnused(sender, e);
            linkedObject.OnInteractableObjectUnused(e);

            // TODO: @msaw - figure out if we need the animation reset or if it's stalling the animation
            //RewindAnimation();

            Debug.Log($"-Delayed Un Use Action- {delayTillUnusable}");
        }
        #endregion

        //-----------------------------------------------------------------
        //                                NOT USED
        //-----------------------------------------------------------------
        protected virtual void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
        {
            if (isUsed && sGPushStartedEvent.HasFired)
                OnSGPushCanceled();

            isUsed = false;

            SGPushStartedEvent.UnregisterListener(OnSGPushStarted);
            SGPushChangedEvent.UnregisterListener(OnSGPushChanged);
            SGPushEndedEvent.UnregisterListener(OnSGPushEnded);

            //log.InfoMS("<Color=blue>\n---Unused---</Color>");
            frameOffset = currentFrame;
            if (rewindOnRelease)
            {
                RewindAnimation();
            }
            SetTimelineSpeed(1);
            
        }


        //-----------------------------------------------------------------
        protected virtual void OnEnable()
        {
            linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

            if (linkedObject != null)
            {
                linkedObject.InteractableObjectTouched += InteractableObjectTouched;
                linkedObject.InteractableObjectUntouched += InteractableObjectUntouched;
            }
        }

        protected virtual void OnDisable()
        {
            if (linkedObject != null)
            {
                linkedObject.InteractableObjectTouched -= InteractableObjectTouched;
                linkedObject.InteractableObjectUntouched -= InteractableObjectUntouched;
            }
            CancelResetTimeline();
        }

        //-----------------------------------------------------------------
        //                           ANIMATION CONTROL
        //-----------------------------------------------------------------

        #region Animation Control

        protected void Initialise()
        {
            InitTimeline();
        }

        /// <summary>
        /// The SetFrame method scrubs to the specific frame of the Animator timeline.
        /// </summary>
        /// <param name="frame">The frame to scrub to.</param>
        public virtual void SetFrame(float frame)
        {
            float setFrame = frame;//* distanceMultiplier;
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

        protected virtual void InitTimeline()
        {
            animatorTimeline = (animatorTimeline != null ? animatorTimeline : GetComponent<Animator>());
            //animationTimeline = (animationTimeline != null ? animationTimeline : GetComponent<Animation>());
            //if (animationTimeline != null)
            //{
            //    if (!animationTimeline.clip.legacy)
            //    {
            //        VRTK_Logger.Error("The `VRTK_MSAW_ControlAnimationUseButton` script is using an `Animation Timeline` that has not been set to `Legacy Animation`. Only legacy animations are supported.");
            //    }
            
            //    foreach (AnimationState currentClip in animationTimeline)
            //    {
            //        animationName = currentClip.name;
            //        break;
            //    }
            //}
            SetTimelineSpeed(animationSpeed);
        }

        protected virtual void SetTimelineSpeed(float speed)
        {
            //if (animationTimeline != null)
            //{
            //    animationTimeline[animationName].speed = speed;
            //}
            //else 
            if (animatorTimeline != null)
            {
                animatorTimeline.speed = speed;
            }
        }

        protected virtual void SetTimelinePosition(float framePosition)
        {
            //if (animationTimeline != null)
            //{
            //    animationTimeline[animationName].time = framePosition;
            //    animationTimeline.Play(animationName);
            //}
            //else 
            if (animatorTimeline != null)
            {
                animatorTimeline.Play(0, 0, framePosition);
            }
        }

        protected void ResumeAnimation()
        {
            if (animatorTimeline != null)
            {
                //animatorTimeline.Play(0);
                animatorTimeline.StartPlayback();
                
                
            }
        }

        protected void HaltAnimation()
        {
            if (animationTimeline != null)
            {
                animatorTimeline.StopPlayback();
                //animationTimeline.playAutomatically = false;

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
            SetTimelineSpeed(1);
        }

        #endregion

        protected virtual ControlAnimationUseButtonEventArgs SetEventPayload(float frame)
        {
            ControlAnimationUseButtonEventArgs e;
            e.interactingObject = (grabbedObjectScript != null ? grabbedObjectScript.GetGrabbingObject() : null);
            e.currentFrame = frame;
            return e;
        }
    }
}