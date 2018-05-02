using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StepStateMachine
{
    [System.Serializable]
    public class StepInfo
    {
        [SerializeField]
        private int _togID;
        private int _stepID = 0;
        [SerializeField]
        private string[] stepNames;
        public int StepID
        {
            set
            {
                if (value >= 0 && value < stepNames.Length)
                {
                    _stepID = value;
                }
            }
        }
        public string NextStepName
        {
            get
            {
                if (stepNames != null && _stepID + 1 < stepNames.Length)
                {
                    return stepNames[_stepID + 1];
                }
                return null;
            }
        }
        public int TogID { get { return _togID; } }
    }
}