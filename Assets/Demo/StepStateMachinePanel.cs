using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using StepStateMachine;

public class StepStateMachinePanel : MonoBehaviour {

    public event UnityAction OnDelete;
    public static StepStateMachinePanel Instence;

    [SerializeField] private Button m_lastBtn;
    [SerializeField] private Button m_nextBtn;
    [SerializeField] private Button m_returnBtn;

  
  
    //[HideInInspector]
    //public BeamSystem beamSystem;
    //[HideInInspector]
    //public SteelSystem steelSystem;
    [HideInInspector]
    public Camera systemCamera;
    private StepController stepCtrl;
    [SerializeField] private Toggle[] m_Toggles;

    void Awake(){
        Instence = this;

        m_lastBtn.onClick.AddListener(stepCtrl.OnLastButtonClicked);
        m_nextBtn.onClick.AddListener(stepCtrl.OnNextButtonClicked);
        m_returnBtn.onClick.AddListener(OnBackButtonClicked);
        for (int i = 0; i < m_Toggles.Length; i++)
        {
            int index = i;
            m_Toggles[index].onValueChanged.AddListener((x) => { if (x) stepCtrl. SetTogActive(index); });
        }
        //Facade.Instance.SendNotification<ExpState>(AppFixed.AppCommand.EXPSTATECHANGED, ExpState.Choise);
    }

    private void Start()
    {
        stepCtrl.SetTogActive(0);
    }

    public void HideBackGround(bool isOn)
    {
        Image image = GetComponent<Image>();
        image.enabled = !isOn;
    }
    private void OnBackButtonClicked()
    {
        //Facade.Instance.SendNotification(AppFixed.ExperimentEvents.OPENCAMERA, false);
        //Facade.Instance.SendNotification(AppFixed.ExperimentEvents.OPENMENU);
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        OnDelete.Invoke();
    }
}
