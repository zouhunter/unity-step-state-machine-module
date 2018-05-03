using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace StepStateMachine
{
    public class StepMachine
    {
        public UnityAction<int> onStepChanged { get; set; }
        private List<IStep> stepList = new List<IStep>();
        private Toggle[] toggles;
        private int _togIndex = 0;
        private string woring;
        private bool isSettingTogState;
        private StepMachine() { }
        public static StepMachine current { get; private set; }

        public static StepMachine CreateNewStepMachine(Toggle[] toggles)
        {
            current = new StepMachine(toggles);
            return current;
        }

        private StepMachine(Toggle[] toggles)
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

        public void ReStartMachine()
        {
            _togIndex = 0;
            SetToggleActiveState(0,true);
        }

        /// <summary>
        /// 上一步
        /// </summary>
        public bool OnLast()
        {
            var step = GetCurrentStep();
            if (_togIndex > 0)
            {
                if (step.OnLast())
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
        public bool OnNext()
        {
            var step = GetCurrentStep();
            if (_togIndex < toggles.Length - 1)
            {
                if (step.OnNext())
                {
                    SetToggleActiveState(++_togIndex,true);
                    return true;
                }

            }
            return false;
        }

        public void SetToggleActiveState(int id,bool trigger)
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

        public void RegistStep(IStep step)
        {
            if (step == null) return;

            var lastOne = stepList.Find(x => x.Index == step.Index);
            if (lastOne != null)
            {
                if (lastOne != step)
                {
                    stepList.Remove(lastOne);
                    stepList.Add(step);
                }
            }
            else
            {
                stepList.Add(step);
            }
        }

        public IStep GetCurrentStep()
        {
            var step = stepList.Find(x => x.Index == _togIndex);
            if (step == null)
            {
                step = new NormalStep(_togIndex);
                stepList.Add(step);
            }
            return step;
        }

        private void OnToggleActived(int id)
        {
            if (_togIndex < id)
            {
                for (int i = _togIndex; i < id; i++)
                {
                    if (!OnNext())
                    {
                        Debug.Log("!OnNext");
                        SetToggleActiveState(i,false);
                        break;
                    }
                }
                OnStepActive();
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
                OnStepActive();
            }

            if (onStepChanged != null)
            {
                onStepChanged.Invoke(_togIndex);
            }
        }

        private void OnStepActive()
        {
            var step = GetCurrentStep();
            step.OnStepActive();
        }
    }
}