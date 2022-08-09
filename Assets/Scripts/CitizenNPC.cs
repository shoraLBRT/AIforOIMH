using Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICitizenState
{
    void CitizenUpdate();
    void Exit();
    void Enter();
}
public class CitizenNPC : AbstractNPC
{
    private CitizenOpenVariables _citizenVariables;
    private int _healthPoint;

    private Rigidbody _rb;
    private Animator _animator;

    private Dictionary<Type, ICitizenState> _statesGroup;
    private ICitizenState _currentState;
    private enum AnimationState { idle, walk, work };
    private AnimationState currentAnimationState;

    private void Start()
    {
        _citizenVariables = Locator.GetObject<CitizenOpenVariables>();
        _animator = _citizenVariables.CitizenAnimator;
        _rb = _citizenVariables.CitizenRB;

        Debug.Log("Starting Citizen");
        InitStates();
        ChooseState();

    }
    private void Update()
    {
        _currentState.CitizenUpdate();

        AnimationControlling();
    }
    private void ChooseState()
    {
        Debug.Log("choosing State");
        SetIdleState();
    }
    #region States Settings
    private void InitStates()
    {
        _statesGroup = new Dictionary<Type, ICitizenState>();
        _statesGroup[typeof(CitizenIdleState)] = new CitizenIdleState();
    }
    protected void SetState(ICitizenState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = newState;
        _currentState.Enter();
    }
    protected override void SetIdleState()
    {
        Debug.Log("Citizen Idle State");
        ICitizenState state = GetState<CitizenIdleState>();
        SetState(state);
    }
    protected override void SetMeetingState()
    {
        throw new NotImplementedException();
    }

    protected override void SetAgressiveState()
    {
        throw new NotImplementedException();
    }

    protected override void SetNightState()
    {
        throw new NotImplementedException();
    }
    private ICitizenState GetState<T>() where T : ICitizenState
    {
        var type = typeof(T);
        return _statesGroup[type];
    }
    #endregion
    #region Animation of citizen
    private void AnimationControlling()
    {
        //if (_rb.velocity.y > 2 || _rb.velocity.z > 2)
        //    SetAnimationState(AnimationState.walk);
        //else SetAnimationState(AnimationState.idle);
    }
    private void SetAnimationState(AnimationState state)
    {
        //currentAnimationState = state;
        //_animator.SetInteger("state", (int)currentAnimationState);
    }

    #endregion
}
