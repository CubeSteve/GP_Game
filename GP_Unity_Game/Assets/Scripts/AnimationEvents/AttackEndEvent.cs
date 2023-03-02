using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEndEvent : MonoBehaviour
{
    public void AttackEnd(string s)
    {
        this.GetComponent<Animator>().SetBool("isAttacking", false);
    }
}
