using Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GuardianIdleState : IGuardianState
{
    private AIManager _aiManager;

    private Transform _guardianTransform;
    private GameObject[] _waypoints;

    private Transform[] _securityAreas;
    private int _currentWA = 0;

    private float _holdTime = 2f;

    private int _currentWP = 0;

    private float _guardianSpeed = 8f;
    private float _rotSpeed = 5f;

    private readonly Dictionary<_states, Action> IdleStates = new Dictionary<_states, Action>();
    public enum _states { stay, roaming, holding };
    public _states _currentIdleState = _states.stay;

    private int randomInt = UnityEngine.Random.Range(0, 2);

    #region Idle States Settings
    public async void EnterAsync(GameObject concreteGuardian, GameObject[] guardianWayPoints)
    {
        _guardianTransform = concreteGuardian.transform;
        GetGlobalVariables();

        _waypoints = guardianWayPoints;

        await InitIdleStates();
        SetRandomState();
        _aiManager = Locator.GetObject<AIManager>();
        _aiManager.GuardianRoaming += delegate()
        {
            SetCurrentIdleState(_states.roaming);
        };
        _aiManager.GuardianHolding += delegate ()
        {
            SetCurrentIdleState(_states.holding);
        };

    }
    private void GetGlobalVariables()
    {
        _securityAreas = Locator.GetObject<OpenVariables>().SecurityZones;

    }
    public void GuardianUpdate()
    {
        UpdateCurrentState(_currentIdleState);
    }
    public void Exit()
    {
        _currentIdleState = 0;
        IdleStates.Clear();
    }
    public async Task InitIdleStates()
    {   
        IdleStates.Add(_states.roaming, RoamingToWP);
        IdleStates.Add(_states.holding, Holding);
        IdleStates.Add(_states.stay, Stay);
        Debug.Log("Inited");
        await Task.Delay(100);
    }
    public void SetRandomState()
    {
        randomInt = UnityEngine.Random.Range(1, 3);
        if (randomInt > 2)
            randomInt = 0;
        switch (randomInt)
        {
            case 1:
                SetCurrentIdleState(_states.roaming);
                break;
            case 2:
                SetCurrentIdleState(_states.holding);
                break;
            default:
                SetCurrentIdleState(_states.stay);
                break;
        }
    }
    public void SetCurrentIdleState(_states state)
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
        var minRange = 5f;

        if (Vector3.Distance(_guardianTransform.position, _waypoints[_currentWP].transform.position) < minRange)
        {
            _currentWP++;
        }

        if (_currentWP >= _waypoints.Length)
            _currentWP = 0;

        Quaternion lookatWP = Quaternion.LookRotation(_waypoints[_currentWP].transform.position - _guardianTransform.position);

        _guardianTransform.rotation = Quaternion.Slerp(_guardianTransform.rotation, lookatWP, _rotSpeed * Time.deltaTime);

        _guardianTransform.Translate(0, 0, _guardianSpeed * Time.deltaTime);
    }
    private void Holding()
    {
        var minRange = 8f;

        if (Vector3.Distance(_guardianTransform.position, _securityAreas[_currentWA].transform.position) < minRange)
        {
            QuestTimer();
        }

        Quaternion lookatWP = Quaternion.LookRotation(_securityAreas[_currentWA].transform.position - _guardianTransform.position);
        _guardianTransform.rotation = Quaternion.Slerp(_guardianTransform.rotation, lookatWP, _rotSpeed * Time.deltaTime);

        _guardianTransform.Translate(0, 0, _guardianSpeed * Time.deltaTime);
    }
    private void QuestTimer()
    {
        if (_holdTime > 0)
        {
            _holdTime -= Time.deltaTime * 100f;
            _guardianSpeed = 1f;
        }

        if (_holdTime <= 0f)
        {
            _guardianSpeed = 5f;
            _currentWA = UnityEngine.Random.Range(0, _securityAreas.Length);
            _holdTime = 5f;
            if (_currentWA >= _securityAreas.Length)
            {
                _currentWA = 0;
            }

        }
    }

    private void Stay()
    {

    }

    #endregion
}
