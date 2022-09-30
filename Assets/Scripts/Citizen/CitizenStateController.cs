using Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CitizenStateController : MonoBehaviour, INPCStates
{
    public GameObject TheWriter;

    private AIManager _aiManager;
    private Dictionary<Type, ICitizenState> _statesGroup;
    private ICitizenState _currentState;

    private GameObject _concreteCitizen;

    [SerializeField] private GameObject[] _citizenWaypoints;


    private void Awake()
    {
        Locator.Register<CitizenStateController>(this);
        InitStates();
        _concreteCitizen = gameObject;
    }
    private void Start()
    {
        SetIdleState();
        _aiManager = Locator.GetObject<AIManager>();
        _aiManager.GoToMeeting += SetMeetingState;
        _aiManager.GoToTheBed += SetNightState;
        _aiManager.CitizenIdle += SetIdleState;
        _aiManager.CitizenAgr += SetAgressiveState;
    }
    private void Update()
    {
        TestController();

        if (_currentState != null)
            _currentState.CitizenUpdate();
    }
    private void TestController()
    {
        if (Input.GetKeyDown(KeyCode.W))
            SetIdleState();

    }
    private void InitStates()
    {
        _statesGroup = new Dictionary<Type, ICitizenState>();
        _statesGroup[typeof(CitizenIdleState)] = new CitizenIdleState();
        _statesGroup[typeof(CitizenMeetingState)] = new CitizenMeetingState();
        _statesGroup[typeof(CitizenNightState)] = new CitizenNightState();
        _statesGroup[typeof(CitizenAgressiveState)] = new CitizenAgressiveState();
    }
    private void SetState(ICitizenState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = newState;
        _currentState.EnterAsync(_concreteCitizen, _citizenWaypoints);
    }
    private ICitizenState GetState<T>() where T : ICitizenState
    {
        var type = typeof(T);
        return _statesGroup[type];
    }

    public void SetIdleState()
    {
        ICitizenState state = GetState<CitizenIdleState>();
        SetState(state);
    }

    public void SetMeetingState()
    {
        ICitizenState state = GetState<CitizenMeetingState>();
        SetState(state);
    }
    public void SetAgressiveState()
    {
        ICitizenState state = GetState<CitizenAgressiveState>();
        SetState(state);
    }

    public void SetNightState()
    {
        ICitizenState state = GetState<CitizenNightState>();
        SetState(state);
    }
}
