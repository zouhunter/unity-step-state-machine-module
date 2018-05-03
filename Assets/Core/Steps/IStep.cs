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
        void OnStepActive();
        bool OnLast();
        bool OnNext();
        bool IsComplete();
    }
}