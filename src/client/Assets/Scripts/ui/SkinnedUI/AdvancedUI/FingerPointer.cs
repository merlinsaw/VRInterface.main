//-------------------------------------------------------
//	A collision object for the fingertip to trigger touch interactions.
//-------------------------------------------------------

#region using

using UnityEngine;
using UnityEngine.Events;

#endregion

namespace AdvancedUI
{


    public class FingerPointer : MonoBehaviour
    {



        //PanelMainMenu MainMenue = new PanelMainMenu();
        //Button button;
        AdvancedButton button;
        //BaseEventData baseEventData;
        //PointerEventData pointerEventData;
        ColliderEventData colliderEventData;



        public UnityEvent TouchButton;
        public UnityEvent GrabItem;
        public UnityEvent ReleaseItem;

        public bool uiIsOpen = false;
        public GameObject MainMenu;


        public void _popo() {

            Debug.Log("popo ++++++++++++++++++++++++++++++++++++");
        }


        private void Awake()
        {
            //button = GetComponent<Button>();
        }






        private void OnTriggerEnter(Collider other)
        {



            //Debug.Log(other.gameObject.name);
            if (other.gameObject.GetComponent<AdvancedButton>() != null)
            {
                Debug.Log("is advanced button");

                try
                {
                    button = other.GetComponent<AdvancedButton>();
                    TouchButton.Invoke();
                    //button.OnSelect(eventDataB);
                    //button.OnPointerEnter(eventDataP);
                    colliderEventData = new ColliderEventData(this.gameObject, button.gameObject);
                    // TODO: @msaw - if we dont need the trigger enter on the button and finger we can work around this
                    //button.OnTriggerEnters(colliderEventData);


                }
                catch (System.Exception ex)
                {

                    Debug.Log(ex);
                }

            }

            if (other.gameObject.name == "OpenUITrigger")
            {
                // open the 3d UI

                if (MainMenu != null)
                    OpenMainMenu();

                //MainMenue.Open();

                Debug.Log("OpenUITrigger collision detected");
            }


        }


        private void OpenMainMenu()
        {
            if (!uiIsOpen)
            {
                uiIsOpen = true;
                MainMenu.SetActive(true);
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<AdvancedButton>() != null)
            {
                button = other.GetComponent<AdvancedButton>();
                // TODO: @msaw - if we dont need the trigger enter on the button and finger we can work around this
                //button.OnTriggerExits(colliderEventData);
            }
        }

    }
}