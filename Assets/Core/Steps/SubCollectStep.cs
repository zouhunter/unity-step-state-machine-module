using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace StepStateMachine
{
    /// <summary>
    /// 子步骤中需要完成所有的状态才能进行下一步
    /// </summary>
    public abstract class SubCollectStep : IStep
    {
        public int Index { get; private set; }
        public UnityAction onReset { get; set; }
        public UnityAction onStateChanged { get; set; }

        public StepMachine Machine { get; private set; }

        protected List<string> completed = new List<string>();
        protected virtual string[] SubSteps { get; set; }

        public SubCollectStep(int index)
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
            return completed.Count == 0;
        }

        public virtual bool CanNext()
        {
            return completed.Count == SubSteps.Length;
        }


        public virtual void CompleteState(int stepID)
        {
            if (stepID >= 0 && SubSteps.Length > stepID)
            {
                var keyword = SubSteps[stepID];
                if (!completed.Contains(keyword))
                {
                    completed.Add(keyword);
                }
            }
        }

        public virtual void OnReset()
        {
            completed.Clear();
            if (onReset != null)
            {
                onReset.Invoke();
            }
        }

        public virtual void OnUnRegisted() { }
    }
}