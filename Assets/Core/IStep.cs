using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace StepStateMachine
{
    public interface IStep
    {
        int Index { get; }
        int Current { get; }
        UnityAction onReset { get; set; }
        UnityAction onStateChanged { get; set; }
        string Name { get; set; }

        void OnRegisted(StepMachine stepMachine);
        void OnUnRegisted();

        void OnStepActive();
        void OnReset();

        void CompleteState(int state);
        bool CanLast();
        bool CanNext();
    }
}