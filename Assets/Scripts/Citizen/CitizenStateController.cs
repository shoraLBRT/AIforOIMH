using Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CitizenStateController : MonoBehaviour, INPCStates
{
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
        if (Input.GetKeyDown(KeyCode.A))
            SetMeetingState();
    }
    private void InitStates()
    {
        _statesGroup = new Dictionary<Type, ICitizenState>();
        _statesGroup[typeof(CitizenIdleState)] = new CitizenIdleState();
        _statesGroup[typeof(CitizenMeetingState)] = new CitizenMeetingState();
    }
    private void SetState(ICitizenState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = newState;
        _currentState.Enter(_concreteCitizen, _citizenWaypoints);
    }
    private ICitizenState GetState<T>() where T : ICitizenState
    {
        var type = typeof(T);
        return _statesGroup[type];
    }

    public void SetIdleState()
    {
        Debug.Log("Citizen Idle State");
        ICitizenState state = GetState<CitizenIdleState>();
        SetState(state);
    }

    public void SetMeetingState()
    {
        Debug.Log("Citizen Meeting State");
        ICitizenState state = GetState<CitizenMeetingState>();
        SetState(state);
    }
    public void SetAgressiveState()
    {
        throw new NotImplementedException();
    }

    public void SetNightState()
    {
        throw new NotImplementedException();
    }
}
