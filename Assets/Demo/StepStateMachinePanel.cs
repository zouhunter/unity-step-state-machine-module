using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using StepStateMachine;
using System;

public class StepStateMachinePanel : MonoBehaviour
{
    [SerializeField] private Button m_lastBtn;
    [SerializeField] private Button m_nextBtn;
    [SerializeField] private Toggle[] m_Toggles;
    private StepMachine stepMachine;

    void Awake()
    {
        stepMachine = StepMachine.CreateNewStepMachine(m_Toggles);
        m_lastBtn.onClick.AddListener(()=>stepMachine.OnLast());
        m_nextBtn.onClick.AddListener(() => stepMachine.OnNext());
        stepMachine.onStepChanged = OnStepChanged;
        stepMachine.RegistStep(new InputStep(0));
        stepMachine.ReStartMachine();
    }

    private void OnStepChanged(int arg0)
    {
        m_lastBtn.gameObject.SetActive(arg0 != 0);
        m_nextBtn.gameObject.SetActive(arg0 != (m_Toggles.Length - 1));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            InputStep.completeID = 1;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            InputStep.completeID = 2;
        }
    }

}