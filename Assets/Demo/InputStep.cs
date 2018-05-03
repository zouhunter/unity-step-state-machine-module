using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using StepStateMachine;

public class InputStep : InfoStep
{
    public static string[] keyWords = { "A", "B" };
    public static int completeID;

    public InputStep(int index):base(index)
    {

    }
    protected override string[] subSteps
    {
        get
        {
            return keyWords;
        }
    }
    protected override int stepCompleteID { get { return completeID; } }

    public override void OnStepActive()
    {
        Debug.Log("开始进行InputStep步骤,可以在此打开其他面板!");
        completeID = 0;
    }
    public override bool OnLast()
    {
        var canLast = base.OnLast();
        if(!canLast){
            completeID--;
            return false;
        }
        return true;
    }
    public override bool OnNext()
    {
        var conNext = base.OnNext();
        if(!conNext)
        {
            Debug.Log("请输入:" + GetCurrentStep());
            return false;
        }
        return true;
    }
}
