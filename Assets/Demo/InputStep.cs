using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using StepStateMachine;

public class InputStep : SubListStep
{
    public InputStep(int index,params string[] steps):base(index)
    {
        this.SubSteps = steps;
    }
    public override bool CanLast()
    {
        var canLast = base.CanLast();
        if(!canLast){
            StepCompleteID--;
            return false;
        }
        return true;
    }
    public override bool CanNext()
    {
        var conNext = base.CanNext();
        if(!conNext)
        {
            Debug.Log("请输入:" + CurrentInnerStep);
            return false;
        }
        return true;
    }
}
