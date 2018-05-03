using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace StepStateMachine
{
    public class NormalStep: IStep
    {
        public int Index { get; private set; }

        public NormalStep(int index)
        {
            this.Index = index;
        }
        public void OnStepActive()
        {
            Debug.Log("StepActive:" + Index);
        }
        public virtual bool OnLast()
        {
            return true;
        }
        public virtual bool OnNext()
        {
            return true;
        }
        public virtual bool IsComplete()
        {
            return true;
        }
    }
}