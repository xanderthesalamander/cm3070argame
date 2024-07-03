using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [Tooltip("Tutorial animator")]

    public int i = 0;

    public void ShowNextTutorial()
    {
        i = (i + 3 + 1) % 3;
        if (i == 0)
        {
            ShowIdle();
        }
        if (i == 1)
        {
            ShowPressButton();
        }
        if (i == 2)
        {
            ShowGrabObject();
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
