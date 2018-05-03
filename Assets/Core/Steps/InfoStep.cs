using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace StepStateMachine
{
    public abstract class InfoStep : IStep
    {
        public int Index { get; private set; }
        protected abstract string[] subSteps { get; }
        protected abstract int stepCompleteID { get; }//1,2

        public InfoStep(int index)
        {
            this.Index = index;
        }

        public abstract void OnStepActive();

        public virtual bool OnLast()
        {
            if(subSteps == null || subSteps.Length < stepCompleteID){
                return true;
            }
            if (stepCompleteID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool OnNext()
        {
            if (subSteps == null || subSteps.Length < stepCompleteID){
                return true;
            }
            if(stepCompleteID == subSteps.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsComplete()
        {
            if (subSteps == null || subSteps.Length < stepCompleteID)
            {
                return true;
            }
            return stepCompleteID == subSteps.Length;
        }

        public string GetCurrentStep()
        {
            if (subSteps == null || subSteps.Length < stepCompleteID)
            {
                return null;
            }
            if (stepCompleteID == subSteps.Length)
            {
                return null;
            }
            else
            {
                return subSteps[stepCompleteID];
            }
        }
    }
}