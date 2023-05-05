using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEndEvent : MonoBehaviour
{
    public void AttackEnd()
    {
        this.GetComponent<Animator>().SetBool("isAttacking", false);
    }

    public void PressEnd()
    {
        this.GetComponent<Animator>().SetBool("isPressing", false);
    }
}
