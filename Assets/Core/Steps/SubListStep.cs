using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace StepStateMachine
{
    /// <summary>
    /// 子步骤中需要按顺序，完成一系列事件，才能进行下一步
    /// </summary>
    public abstract class SubListStep : IStep
    {
        public int Index { get; protected set; }
        public UnityAction onReset { get; set; }
        public UnityAction onStateChanged { get; set; }

        public StepMachine Machine{ get; protected set; }

        protected int StepCompleteID { get;  set; }//1,2
        protected virtual string[] SubSteps { get; set; }
        public string CurrentInnerStep
        {
            get
            {
                if (SubSteps == null || SubSteps.Length < StepCompleteID)
                {
                    return null;
                }
                if (StepCompleteID == SubSteps.Length)
                {
                    return null;
                }
                else
                {
                    return SubSteps[StepCompleteID];
                }
            }

        }

        public SubListStep(int index)
        {
            this.Index = index;
        }

        public void SetSubSteps(params string[] steps)
        {
            this.SubSteps = steps;
        }

        public virtual void OnRegisted(StepMachine stepMachine)
        {
            this.Machine = stepMachine;
        }

        public virtual void OnStepActive() { }

        public virtual bool CanLast()
        {
            if(SubSteps == null || SubSteps.Length < StepCompleteID)
            {
                return true;
            }
            if (StepCompleteID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool CanNext()
        {
            if (SubSteps == null || SubSteps.Length < StepCompleteID)
            {
                return true;
            }
            if(StepCompleteID == SubSteps.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public virtual void CompleteState(int stepID)
        {
            if (stepID >= 0 && SubSteps.Length > stepID)
            {
                StepCompleteID = stepID + 1;
            }
        }

        public virtual void OnReset()
        {
            StepCompleteID = 0;
            if (onReset != null)
            {
                onReset.Invoke();
            }
        }

        public virtual void OnUnRegisted() { }
    }
}