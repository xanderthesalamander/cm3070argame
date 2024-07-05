using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // TODO: This needs cleanup - rushing in for playtest (tech debt)
    [SerializeField] Animator animator;
    [Tooltip("Tutorial animator")]
    public string tutorialAnimation = "animatorIdle";
    [Tooltip("The animation to be shown")]

    public int i = 0;
    private int n = 3;

    public void ShowPrevTutorial()
    {
        i = (i + n - 1) % n;
        ShowTutorial();
    }

    public void ShowNextTutorial()
    {
        i = (i + n + 1) % n;
        ShowTutorial();
    }

    private void ShowTutorial()
    {
        // if (i == 0)
        // {
        //     ShowIdle();
        // }
        if (i == 0)
        {
            ShowPressButton();
        }
        if (i == 1)
        {
            ShowGrabObject();
        }
        if (i == 2)
        {
            ShowUseObject();
        }
    }

    void OnEnable()
    {
        if (tutorialAnimation == "animatorIdle")
        {
            animator.SetTrigger("animatorIdleTrigger");
        }
        else if (tutorialAnimation == "animatorPressButton")
        {
            animator.SetTrigger("animatorPressButtonTrigger");
        }
        else if (tutorialAnimation == "animatorGrabObject")
        {
            animator.SetTrigger("animatorGrabObjectTrigger");
        }
        else if (tutorialAnimation == "animatorUseObject")
        {
            animator.SetTrigger("animatorUseObjectTrigger");
        }
        else
        {
            Debug.LogError("TutorialManager - Invalid animation selected");
        }
    }

    public void ShowIdle()
    {
        animator.SetTrigger("animatorIdleTrigger");
    }

    public void ShowGrabObject()
    {
        animator.SetTrigger("animatorGrabObjectTrigger");
    }

    public void ShowUseObject()
    {
        animator.SetTrigger("animatorUseObjectTrigger");
    }

    public void ShowPressButton()
    {
        animator.SetTrigger("animatorPressButtonTrigger");
    }
}
