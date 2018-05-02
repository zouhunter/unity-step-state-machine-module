using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using StepStateMachine;

namespace StepStateMachine
{
    public class StepController 
    {
        private UnityAction lastBtnEvent;
        private UnityAction nextBtnEvent;
        private int _togIndex = 0;
        private Toggle m_lastCloseTog;
        [SerializeField] private Toggle[] m_Toggles;
        [SerializeField] private StepInfo[] _steps;//所有需要完成的步骤

        public StepController()
        {
           
        }
        public void SetLastEvent(UnityAction lastBtnEvent)
        {
            this.lastBtnEvent = lastBtnEvent;
        }

        public void SetNextEvent(UnityAction nextBtnEvent)
        {
            this.nextBtnEvent = nextBtnEvent;
        }
        public void SetTogActive(int id)
        {
            if (id >= 0 && id < m_Toggles.Length)
            {
                _togIndex = id;
                m_Toggles[id].isOn = true;
                //m_lastBtn.gameObject.SetActive(_togIndex != 0);
                //m_nextBtn.gameObject.SetActive(_togIndex != (m_Toggles.Length - 1));
            }

            if (id != 3 && id != 8 && id != 9)
            {
                //Facade.Instance.SendNotification(AppFixed.ExperimentEvents.OPENCAMERA, false);
            }
        }


        public void OnLastButtonClicked()
        {
            if (lastBtnEvent != null)
            {
                lastBtnEvent.Invoke();
            }
            else if (_togIndex > 0)
            {
                SetTogActive(--_togIndex);
            }
        }

        public void OnNextButtonClicked()
        {
            if (nextBtnEvent != null)
            {
                nextBtnEvent.Invoke();
            }
            else if (_togIndex < m_Toggles.Length - 1)
            {
                SetTogActive(++_togIndex);
            }
        }

        /// <summary>
        /// return isforward
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public void SetCurrentStep(int stepID)
        {
            StepInfo info = System.Array.Find(_steps, (x) => x.TogID == _togIndex);
            if (info != null)
            {
                info.StepID = stepID;
            }
        }

        /// <summary>
        /// 获取关闭某个按扭需要进行的步骤
        /// </summary>
        /// <param name="tog"></param>
        /// <returns></returns>
        public void SetLastCloseTog(Toggle tog)
        {
            m_lastCloseTog = tog;
        }

        public bool NeedWait(out string waitStep)
        {
            if (m_lastCloseTog == null) { waitStep = null; return false; }
            int togIndex = System.Array.IndexOf(m_Toggles, m_lastCloseTog);
            if (_togIndex > togIndex)
            {
                for (int i = togIndex; i < _togIndex; i++)
                {
                    StepInfo info = System.Array.Find(_steps, (x) => x.TogID == i);
                    if (info != null && info.NextStepName != null)
                    {
                        waitStep = info.NextStepName;
                        m_lastCloseTog.isOn = true;
                        return true;
                    }
                }
                waitStep = null;
                return false;
            }
            else
            {
                waitStep = null;
                return false;
            }
        }
    }
}
