using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace StepStateMachine
{
    public class StepMachine
    {
        public UnityAction<string> onError { get; set; }
        public UnityAction<int> onStepChanged { get; set; }
        protected List<IStep> stepList = new List<IStep>();
        protected Toggle[] toggles;
        protected int _togIndex = 0;
        protected string woring;
        protected bool isSettingTogState;
        public int noModifyIndex;
        public IStep currentStep
        {
            get
            {
                return stepList[_togIndex];
            }
        }
        public StepMachine() { }
     
        public StepMachine(Toggle[] toggles)
        {
            RegistToggleArray(toggles);
        }

        public void RegistToggleArray(Toggle[] toggles)
        {
            this.toggles = toggles;
            for (int i = 0; i < toggles.Length; i++)
            {
                int index = i;
                toggles[index].onValueChanged.AddListener((x) =>
                {
                    if (x && !isSettingTogState)
                        OnToggleActived(index);
                });
            }
        }

        public virtual void ReStartMachine()
        {
            _togIndex = 0;
            SetToggleActiveState(0,true);
        }
        /// <summary>
        /// 上一步
        /// </summary>
        public virtual bool OnLast()
        {
            var step = GetCurrentStep();

            var error = "last step not registed:" + (_togIndex - 1);
            if (IsStepEmpty(step, error)) return false;

            if (_togIndex > 0)
            {
                if (step.CanLast())
                {
                    SetToggleActiveState(--_togIndex,true);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 下一步
        /// </summary>
        public virtual bool OnNext()
        {
            var step = GetCurrentStep();

            var error = "next step not registed:" + (_togIndex + 1);
            if (IsStepEmpty(step, error)) return false;

            if (_togIndex < toggles.Length - 1)
            {
                if (step.CanNext())
                {
                    SetToggleActiveState(++_togIndex,true);
                    return true;
                }

            }
            return false;
        }

        public virtual void SetToggleActiveState(int id,bool trigger)
        {
            if (id >= 0 && id < toggles.Length)
            {
                _togIndex = id;

                if (!trigger){
                    isSettingTogState = true;
                }

                toggles[id].isOn = true;
                isSettingTogState = false;
            }
        }

        public virtual void RegistSteps(IEnumerable<IStep> steps)
        {
            foreach (var item in steps)
            {
                RegistStep(item);
            }
        }

        public virtual void RegistStep(IStep step)
        {
            if (step == null) return;
            var lastOne = stepList.Find(x => x.Index == step.Index);
            if (lastOne != null)
            {
                if (lastOne != step)
                {
                    stepList.Remove(lastOne);
                    lastOne.OnUnRegisted();
                    stepList.Add(step);
                    step.OnRegisted(this);
                }
            }
            else
            {
                step.OnRegisted(this);
                stepList.Add(step);
            }
        }

        public virtual IStep GetCurrentStep()
        {
            var step = stepList.Find(x => x.Index == _togIndex);
            if (step == null)
            {
                return null;
            }
            return step;
        }

        protected virtual void OnToggleActived(int id)
        {
            if (_togIndex < id)
            {
                for (int i = _togIndex; i < id; i++)
                {
                    if (!OnNext())
                    {
                        SetToggleActiveState(i,false);
                        break;
                    }
                }
            }
            else if (_togIndex > id)
            {
                for (int i = _togIndex ; i > id; i--)
                {
                    if (!OnLast())
                    {
                        SetToggleActiveState(i,false);
                        break;
                    }
                }
            }

            OnStepActive();
        }

        protected virtual void OnStepActive()
        {
            var step = GetCurrentStep();
            var error = "current step not registed:" + (_togIndex);
            if (IsStepEmpty(step, error)) return;

            step.OnStepActive();
            step.onStateChanged = OnStateChanged;

            if (onStepChanged != null)
            {
                onStepChanged.Invoke(_togIndex);
            }
        }

        protected virtual void OnStateChanged()
        {
            if (noModifyIndex > _togIndex + 1)
            {
                for (int i = _togIndex + 1; i < noModifyIndex; i++)
                {
                    var step = stepList[i];
                    step.OnReset();
                }
            }

            noModifyIndex = _togIndex + 1;
        }

        protected bool IsStepEmpty(IStep step,string error)
        {
            if (step == null)
            {
                if (onError != null)
                {
                    onError.Invoke(error);
                }
                else
                {
                    Debug.LogError(error);
                }
                return true;
            }
            return false;
        }
    }
}