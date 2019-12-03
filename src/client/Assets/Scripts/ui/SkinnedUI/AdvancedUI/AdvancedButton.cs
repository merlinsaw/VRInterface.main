//-------------------------------------------------------
//	Advanced button that reacts to VRTK pointer and listens to skeleton gestures
//-------------------------------------------------------

#region using

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using SGEventSysthem; // Listens to the skeleton gesture events.
using VRTK; // Copatibility with interactibale objects.

#endregion
// TODO: @msaw - i need to somehow invoke the submission of a button or and a used event to the inspector window
namespace AdvancedUI
{
    [RequireComponent(typeof(VRTK_InteractableObject))]
    [RequireComponent(typeof(BoxCollider))]
    [AddComponentMenu("Custom UI/Advanced Button")]
    public class AdvancedButton : Selectable, ISubmitHandler, ISelectHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private static readonly _Logger log = new _Logger(typeof(AdvancedButton));

        #region Fields

        [Tooltip("The interacting object touching this button.")]
        public VRTK_InteractableObject linkedObject;

        [Tooltip("more debug information will be printed.")]
        [SerializeField] private bool debug = true;
        [Tooltip("the button will be automatically seltcted after a short duration.")]
        [SerializeField] private bool autoSelection = false;
        [Tooltip("the button will be automatically submitted after a short duration.")]
        [SerializeField] private bool autoSubmission = false;

        #endregion

        #region Properties

        [SerializeField] private InteractableEvents events;
        public InteractableEvents Events
        {
            get { return events; }
            set { events = value; }
        }

        #endregion

        #region Main Methods

        public void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, true);

            if (debug)
                log.InfoMS("Pressed");
        }


        public void Submit()
        {
            if (!IsActive() || !IsInteractable())
                return;

            //DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(FinishSubmit());


            if (debug)
                log.InfoMS("Submitted");
        }


        #endregion

        #region Inherited Methods


        public void OnSubmit(BaseEventData eventData)
        {
            //DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(FinishSubmit());


            if (debug)
                log.InfoMS("Submitted");
        }

#if using_collision_events

        public void OnSubmitDelayed(ColliderEventData eventData)
        {
            //DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(FinishSubmit());
            events.onSubmitDelayed.Invoke(eventData);

            if (debug)
                log.InfoMS("Deleayed Submitted");
        }

        public override void OnSelect(BaseEventData eventData) {
          base.OnSelect(eventData);
          events.onSelect.Invoke(eventData);

          if (debug)
        	log.InfoMS("Selected");
        }
        public override void OnDeselect(BaseEventData eventData) {
          base.OnDeselect(eventData);
          events.onDeselect.Invoke(eventData);

          if (debug)
        	log.InfoMS("Deselected");
        }

#endif
        public void OnPointerClick(PointerEventData eventData)
        {
            events.onPointerClick.Invoke(eventData);

            if (debug)
                log.InfoMS("Clicked");
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            events.onPointerDown.Invoke(eventData);

            if (debug)
                log.InfoMS("Pointer down");
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            events.onPointerUp.Invoke(eventData);

            if (debug)
                log.InfoMS("Pointer up");
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            events.onPointerEnter.Invoke(eventData);


            if (debug)
                log.InfoMS("Pointer enter");
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            events.onPointerExit.Invoke(eventData);

            if (debug)
                log.InfoMS("Pointer exit");
        }

        #endregion

        #region Interactible Object Methods



        #endregion

        #region  Collision Methods (Legacy)
        /*
        public void OnTriggerExits(ColliderEventData eventData)
        {

            //StopCoroutine(DeleayedSelection());
            this.StopAllCoroutines();
            events.onTriggerExits.Invoke(eventData);

            if (debug)
                log.InfoMS("Trigger exit");
            //  @msaw - the normal state should be used when the button is not in use instead of exit trigger to avoid jtter
            DoStateTransition(SelectionState.Normal, true);
        }

        public void OnTriggerEnters(ColliderEventData eventData)
        {
            //DoStateTransition(currentSelectionState, false);
            //DoStateTransition(SelectionState.Highlighted, false);
            StartCoroutine(DeleayedSelection(eventData));
            events.onTriggerEnters.Invoke(eventData);


            if (debug)
                log.InfoMS("Trigger enter");
            log.InfoMS(eventData.Controller.name + " touched " + eventData.Collider.name);
        }
         */
        #endregion

        #region Skeleton Gesture Methods

        void OnSGPushStarted(SGPushStartedEvent gesture)
        {

            log.InfoMS($"<Color=blue>{gameObject.name} button recived push started</Color>");

            DoStateTransition(SelectionState.Pressed, true);
        }

        void OnSGPushEnded(SGPushEndedEvent gesture)
        {
            log.InfoMS($"<Color=blue>{gameObject.name} button recived Push ended</Color>");

            DoStateTransition(SelectionState.Selected, true);
        }

        #endregion

        #region Coroutines

        private IEnumerator FinishSubmit()
        {
            float
                fadeTime = colors.fadeDuration,
                elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(SelectionState.Selected, false);

        }



        private IEnumerator DeleayedSelection(ColliderEventData eventData)
        {
            float
              fadeTime = colors.fadeDuration,
              elapsedTime = 0f;


            DoStateTransition(SelectionState.Highlighted, false);
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }



            if (autoSelection)
                DoStateTransition(SelectionState.Pressed, true);
            // if autoSubmission is true the button will submit automatically, 
            // this can be useful for gaze based interaction where hovering will already submit the button.
            if (autoSubmission)
            {

#if using_collision_events
                OnSubmitDelayed(eventData);
#endif
            }
        }

        #endregion

        #region Interactable Object Used

        protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
        {
            log.InfoMS("<Color=red>\n---  Used  ---</Color>");
            DoStateTransition(SelectionState.Highlighted, true);
        }

        protected virtual void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
        {
            log.InfoMS("<Color=blue>\n---  Unused  ---</Color>");
            DoStateTransition(SelectionState.Normal, true);
        }

        #endregion

        #region Override Methods



        protected override void OnEnable()
        {
            linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

            if (linkedObject != null)
            {
                linkedObject.InteractableObjectUsed += InteractableObjectUsed;
                linkedObject.InteractableObjectUnused += InteractableObjectUnused;
            }
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            if (linkedObject != null)
            {
                linkedObject.InteractableObjectUsed -= InteractableObjectUsed;
                linkedObject.InteractableObjectUnused -= InteractableObjectUnused;
            }

            SGPushStartedEvent.UnregisterListener(OnSGPushStarted);
            SGPushEndedEvent.UnregisterListener(OnSGPushEnded);
            base.InstantClearState();
            base.OnDisable();
        }
        /// <summary>
        /// Additional methods can be called here during button state changes. e.g. play a sound.
        /// The listeners for the Skeleton Gestures are managed here.
        /// </summary>
        /// <param name="state">the standard button selection states.</param>
        /// <param name="instant">will use fade tiem or transition instantly.</param>
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            //log.InfoMS($"<Color=cyan>state transition </Color> <b>{state}</b> <Color=cyan>of </Color><Color=green>{gameObject.name}</Color>");

            switch (state)
            {
            case SelectionState.Normal:
                SGPushStartedEvent.UnregisterListener(OnSGPushStarted);
                SGPushEndedEvent.UnregisterListener(OnSGPushEnded);
                break;
            case SelectionState.Highlighted:
                SGPushStartedEvent.RegisterListener(OnSGPushStarted);
                break;
            case SelectionState.Pressed:
                SGPushStartedEvent.UnregisterListener(OnSGPushStarted);
                SGPushEndedEvent.RegisterListener(OnSGPushEnded);
                break;
            case SelectionState.Selected:
                break;
            case SelectionState.Disabled:
                SGPushStartedEvent.UnregisterListener(OnSGPushStarted);
                SGPushEndedEvent.UnregisterListener(OnSGPushEnded);
                break;
            default:
                SGPushStartedEvent.UnregisterListener(OnSGPushStarted);
                SGPushEndedEvent.UnregisterListener(OnSGPushEnded);
                break;
            }

            base.DoStateTransition(state, instant);
        }

        #endregion
    }
}

