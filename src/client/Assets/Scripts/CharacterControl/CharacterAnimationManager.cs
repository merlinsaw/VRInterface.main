using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(VRIK))]
public class CharacterAnimationManager : MonoBehaviour
{

    [Header("Animation Controller")]

    [Tooltip("If the Animation controller is not defined here, than it will use the Controller on that Gameobject.")]
    public Animator animator;
    [Tooltip("If this is enabled the animation will stop when releasing the key, otherwhise the animation will play to the end after one keyress.")]
    public bool holdKeyForAnimation;
    [SerializeField]
    private bool playingAnimation;

    private VRIK ik;

    private float ikWeight = 1.0f;

    void Awake()
    {
        animator = transform.root.GetComponentInChildren<Animator>();
        if (animator == null || !animator.isHuman)
        {
            Debug.LogWarning("CharacterController needs a Humanoid Animator to auto-detect biped references. Please assign references manually.");

        }

        ik = gameObject.transform.GetComponent<VRIK>();
    }

    void Update()
    {
        if (ik == null) return;

        // trigger animation for touching the arm mounted device that opens the UI.
        if (Input.GetKeyDown(KeyCode.T))
        {

            switch (playingAnimation)
            {
            case false:
                animator.SetBool("openTheUI", true);
                StopCoroutine("OnIkWeight");
                StartCoroutine("OnAnimationWeight");
                if (holdKeyForAnimation == false)
                    playingAnimation = true;
                break;
            case true:
                animator.SetBool("openTheUI", false);
                StopCoroutine("OnAnimationWeight");
                StartCoroutine("OnIkWeight");
                playingAnimation = false;
                break;
            default:
                break;
            }
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            if (holdKeyForAnimation == true)
            {
                animator.SetBool("openTheUI", false);
                StopCoroutine("OnAnimationWeight");
                StartCoroutine("OnIkWeight");
                playingAnimation = false;
            }
            

        }
        // trigger a generic button pressing animation.
        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.SetBool("buttonPushing", true);

        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            animator.SetBool("buttonPushing", false);
        }
    }
    // fade in the Ik weight for the arms to override animations with VR controller inputs.
    IEnumerator OnIkWeight()
    {
        while (ikWeight < 1.0f)
        {
            ikWeight += Time.deltaTime * 5;
            ik.solver.leftArm.positionWeight = ikWeight;
            ik.solver.leftArm.rotationWeight = ikWeight;
            ik.solver.rightArm.positionWeight = ikWeight;
            ik.solver.rightArm.rotationWeight = ikWeight;
            yield return null;
        }
    }
    // Fade out the Ik wheights for the arms to trigger testing gesture animations.
    IEnumerator OnAnimationWeight()
    {
        while (ikWeight > 0.0f)
        {
            ikWeight -= Time.deltaTime * 3;
            ik.solver.leftArm.positionWeight = ikWeight;
            ik.solver.leftArm.rotationWeight = ikWeight;
            ik.solver.rightArm.positionWeight = ikWeight;
            ik.solver.rightArm.rotationWeight = ikWeight;
            yield return null;
        }
    }
}
