using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StepStateMachinePanel : MonoBehaviour {

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

    public event UnityAction OnDelete;
    public static StepStateMachinePanel Instence;
    [SerializeField] private Toggle[] m_Toggles;
    [SerializeField] private StepInfo[] _steps;//所有需要完成的步骤
    [SerializeField] private Button m_lastBtn;
    [SerializeField] private Button m_nextBtn;
    [SerializeField] private Button m_returnBtn;

    private UnityAction lastBtnEvent;
    private UnityAction nextBtnEvent;
    private int _togIndex = 0;
    private Toggle m_lastCloseTog;
    //[HideInInspector]
    //public BeamSystem beamSystem;
    //[HideInInspector]
    //public SteelSystem steelSystem;
    [HideInInspector]
    public Camera systemCamera;
    void Awake(){
        Instence = this;

        m_lastBtn.onClick.AddListener(OnLastButtonClicked);
        m_nextBtn.onClick.AddListener(OnNextButtonClicked);
        m_returnBtn.onClick.AddListener(OnBackButtonClicked);
        for (int i = 0; i < m_Toggles.Length; i++)
        {
            int index = i;
            m_Toggles[index].onValueChanged.AddListener((x)=> { if (x) SetTogActive(index); });
        }

        //Facade.Instance.SendNotification<ExpState>(AppFixed.AppCommand.EXPSTATECHANGED, ExpState.Choise);
    }

    private void Start()
    {
        SetTogActive(0);
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
            m_lastBtn.gameObject.SetActive(_togIndex != 0);
            m_nextBtn.gameObject.SetActive(_togIndex != (m_Toggles.Length - 1));
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
        else if(_togIndex > 0)
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
        if (info!= null)
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
