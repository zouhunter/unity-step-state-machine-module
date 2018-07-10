using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace StepStateMachine
{
    /// <summary>
    /// 子步骤中需要按顺序，完成一系列事件，才能进行下一步
    /// </summary>
    public abstract class SubListStep : IStep
    {
        private string name;
        public int Index { get; protected set; }
        public UnityAction onReset { get; set; }
        public UnityAction onStateChanged { get; set; }

        public StepMachine Mechine{ get; protected set; }

        protected int completedID { get;  set; }//1,2
        protected virtual string[] SubSteps { get; set; }
        public string CurrentInnerStep
        {
            get
            {
                if (SubSteps == null || SubSteps.Length < completedID)
                {
                    return null;
                }
                if (completedID == SubSteps.Length)
                {
                    return null;
                }
                else
                {
                    return SubSteps[completedID];
                }
            }

        }

        public int Current
        {
            get
            {
                return completedID;
            }
        }
        public virtual string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
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
            this.Mechine = stepMachine;
        }

        public virtual void OnStepActive() { }

        public virtual bool CanLast()
        {
            if(SubSteps == null || SubSteps.Length < completedID)
            {
                return true;
            }
            if (completedID == 0)
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
            if (SubSteps == null || SubSteps.Length < completedID)
            {
                return true;
            }
            if(completedID == SubSteps.Length)
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
                completedID = stepID + 1;

                if(onStateChanged != null)
                {
                    onStateChanged.Invoke();
                }
            }

        }

        public virtual void OnReset()
        {
            completedID = 0;

            if (onReset != null)
            {
                onReset.Invoke();
            }

           
        }

        public virtual void OnUnRegisted() { }
    }
}