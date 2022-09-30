using Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GuardianController : MonoBehaviour, INPCStates
{
    public GameObject TheWriter;

    private AIManager _aiManager;
    private Dictionary<Type, IGuardianState> _statesGroup;
    private IGuardianState _currentState;

    private GameObject _concreteGuardian;

    [SerializeField] private GameObject[] _GuardianWaypoints;


    private void Awake()
    {
        Locator.Register<GuardianController>(this);
        InitStates();
        _concreteGuardian = gameObject;
    }
    private void Start()
    {
        SetIdleState();
        _aiManager = Locator.GetObject<AIManager>();
        //_aiManager.GoToMeeting += SetMeetingState;
        //_aiManager.GoToTheBed += SetNightState;
        _aiManager.GuardianIdle += SetIdleState;
        _aiManager.GuardianAgr += SetAgressiveState;
    }
    private void Update()
    {
        TestController();

        if (_currentState != null)
            _currentState.GuardianUpdate();
    }
    private void TestController()
    {
        if (Input.GetKeyDown(KeyCode.W))
            SetIdleState();

    }
    private void InitStates()
    {
        _statesGroup = new Dictionary<Type, IGuardianState>();
        _statesGroup[typeof(GuardianIdleState)] = new GuardianIdleState();
        //_statesGroup[typeof(CitizenMeetingState)] = new GuardianMeetingState();
        //_statesGroup[typeof(CitizenNightState)] = new GuardianNightState();
        _statesGroup[typeof(GuardianAgressiveState)] = new GuardianAgressiveState();
    }
    private void SetState(IGuardianState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = newState;
        _currentState.EnterAsync(_concreteGuardian, _GuardianWaypoints);
    }
    private IGuardianState GetState<T>() where T : IGuardianState
    {
        var type = typeof(T);
        return _statesGroup[type];
    }

    public void SetIdleState()
    {
        IGuardianState state = GetState<GuardianIdleState>();
        SetState(state);
    }

    public void SetMeetingState()
    {
        //ICitizenState state = GetState<GuardianMeetingState>();
        //SetState(state);
    }
    public void SetAgressiveState()
    {
        IGuardianState state = GetState<GuardianAgressiveState>();
        SetState(state);
    }

    public void SetNightState()
    {
        //IGuardianState state = GetState<GuardianMeetingState>();
        //SetState(state);
    }
}
