using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace StepStateMachine
{
    public class DefultStep : IStep
    {
        public int Index { get; private set; }

        public DefultStep(int index)
        {
            this.Index = index;
        }
        public void OnStepActive()
        {
        }

        public bool OnLast()
        {
            return true;
        }
        public bool OnNext()
        {
            return true;
        }
        public bool IsComplete()
        {
            return true;
        }
    }
}