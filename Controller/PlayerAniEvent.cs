using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniEvent : MonoBehaviour
{
    public void JumpBooster()
    {
        PlayerController._instance.JumpBooster();
    }

    public void BarrierEnd()
    {
        PlayerController._instance.BarrierSkillEnd();
    }
}
