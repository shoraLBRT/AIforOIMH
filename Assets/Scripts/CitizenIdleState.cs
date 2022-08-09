using System;
using System.Collections.Generic;
using UnityEngine;
using Internal;
using System.Threading.Tasks;

public class CitizenIdleState : ICitizenState
{
    [SerializeField] private Transform _citizenTransform;
    private GameObject[] _waypoints;

    private int _currentWP = 0;

    [SerializeField] private float _citizenSpeed = 10f;
    private float rotSpeed = 5f;

    private readonly Dictionary<_states, Action> IdleStates = new Dictionary<_states, Action>();
    public enum _states { stay, roaming, working };
    public _states _currentIdleState = _states.stay;

    private int randomInt = 0;

    #region Idle States Settings
    public async void Enter()
    {
        _citizenTransform = Locator.GetObject<CitizenOpenVariables>().CitizenTransform;
        _waypoints = Locator.GetObject<CitizenOpenVariables>().CitizenWaypoints;
        await InitIdleStates();
        SetRandomState();
        Debug.Log("Enter into Citizen idle state");
    }
    public void CitizenUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Idle State changed");
            SetRandomState();
        }
        UpdateCurrentState(_currentIdleState);
    }
    public void Exit()
    {
        _currentIdleState = 0;

        Debug.Log("Exit From Citizen Idle State");
    }
    public async Task InitIdleStates()
    {
        IdleStates.Add(_states.roaming, RoamingToWP);
        IdleStates.Add(_states.working, Working);
        IdleStates.Add(_states.stay, Stay);
        Debug.Log("Inited");
        await Task.Delay(100);
    }
    public void SetRandomState()
    {
        Debug.Log("Randoming");
        randomInt++;
        if (randomInt > 2)
            randomInt = 0;
        switch (randomInt)
        {
            case 1:
                SetIdleState(_states.roaming);
                Debug.Log("Roaming");
                break;
            case 2:
                SetIdleState(_states.working);
                Debug.Log("Idle working");
                break;
            default:
                SetIdleState(_states.stay);
                Debug.Log("Idle staying");
                break;
        }
    }
    public void SetIdleState(_states state)
    {
        _currentIdleState = _states.stay;
        _currentIdleState = state;
        Console.WriteLine("setted state" + state);
    }
    public void UpdateCurrentState(_states state)
    {
        IdleStates[state].Invoke();
    }
    #endregion

    #region States
    private void RoamingToWP()
    {
        var minRange = 3f;

        if (Vector3.Distance(_citizenTransform.position, _waypoints[_currentWP].transform.position) < minRange)
        {
            _currentWP++;
            Debug.Log("Idle wp changed");
        }

        if (_currentWP >= _waypoints.Length)
            _currentWP = 0;

        Quaternion lookatWP = Quaternion.LookRotation(_waypoints[_currentWP].transform.position - _citizenTransform.position);

        _citizenTransform.rotation = Quaternion.Slerp(_citizenTransform.rotation, lookatWP, rotSpeed * Time.deltaTime);

        _citizenTransform.Translate(0, 0, _citizenSpeed * Time.deltaTime);
    }

    private void Working()
    {
        _citizenTransform.Translate(0, 0, _citizenSpeed * Time.deltaTime);
    }

    private void Stay()
    {

    }

    #endregion
}
