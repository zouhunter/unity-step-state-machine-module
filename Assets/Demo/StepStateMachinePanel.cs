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
    private KeyCode currentKey;
    void Awake()
    {
        stepMachine =new StepMachine(m_Toggles);
        m_lastBtn.onClick.AddListener(()=>stepMachine.OnLast());
        m_nextBtn.onClick.AddListener(() => stepMachine.OnNext());
        stepMachine.onStepChanged = OnStepChanged;
        for (int i = 0; i < m_Toggles.Length; i++)
        {
            var id = i;
            var keyWord = ((char)(65 + id)).ToString();
            stepMachine.RegistStep(new InputStep(id, keyWord));
            m_Toggles[i].GetComponentInChildren<Text>().text = keyWord;
        }
        stepMachine.ReStartMachine();
    }

    private void OnStepChanged(int arg0)
    {
        m_lastBtn.gameObject.SetActive(arg0 != 0);
        m_nextBtn.gameObject.SetActive(arg0 != (m_Toggles.Length - 1));

        currentKey = (KeyCode)(97 + arg0);
    }

    private void Update()
    {
        if(Input.GetKeyDown(currentKey))
        {
            stepMachine.currentStep.CompleteState(0);
        }
    }

}