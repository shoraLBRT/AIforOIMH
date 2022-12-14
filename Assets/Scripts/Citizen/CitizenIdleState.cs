using Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CitizenIdleState : ICitizenState
{
    private AIManager _aiManager;

    private Transform _citizenTransform;
    private GameObject[] _waypoints;

    private GameObject[] _workAreas;
    private int _currentWA = 0;

    private float _workTime = 2f;

    private int _currentWP = 0;

    private float _citizenSpeed;
    private float _rotSpeed;

    private readonly Dictionary<_states, Action> IdleStates = new Dictionary<_states, Action>();
    public enum _states { stay, roaming, working };
    public _states _currentIdleState = _states.stay;

    private int randomInt = UnityEngine.Random.Range(0, 2);

    #region Idle States Settings
    public async void EnterAsync(GameObject concreteCitizen, GameObject[] citizenWayPoints)
    {
        _citizenTransform = concreteCitizen.transform;
        GetGlobalVariables();

        _waypoints = citizenWayPoints;

        await InitIdleStates();
        SetRandomState();
        _aiManager = Locator.GetObject<AIManager>();
        _aiManager.CitizenRoaming += delegate()
        {
            SetCurrentIdleState(_states.roaming);
        };
        _aiManager.CitizenWorking += delegate ()
        {
            SetCurrentIdleState(_states.working);
        };

    }
    private void GetGlobalVariables()
    {
        _workAreas = Locator.GetObject<CitizenOpenVariables>().CitizenWorkAreas;
        _rotSpeed = Locator.GetObject<CitizenOpenVariables>().CitizenRotationSpeed;
        _citizenSpeed = Locator.GetObject<CitizenOpenVariables>().CitizenSpeed;
    }
    public void CitizenUpdate()
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
        IdleStates.Add(_states.working, Working);
        IdleStates.Add(_states.stay, Stay);
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
                SetCurrentIdleState(_states.working);
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

        if (Vector3.Distance(_citizenTransform.position, _waypoints[_currentWP].transform.position) < minRange)
        {
            _currentWP++;
        }

        if (_currentWP >= _waypoints.Length)
            _currentWP = 0;

        Quaternion lookatWP = Quaternion.LookRotation(_waypoints[_currentWP].transform.position - _citizenTransform.position);

        _citizenTransform.rotation = Quaternion.Slerp(_citizenTransform.rotation, lookatWP, _rotSpeed * Time.deltaTime);

        _citizenTransform.Translate(0, 0, _citizenSpeed * Time.deltaTime);
    }
    #region CitizenWorking
    private void Working()
    {
        var minRange = 8f;


        if (Vector3.Distance(_citizenTransform.position, _workAreas[_currentWA].transform.position) < minRange)
        {
            QuestTimer();
        }

        Quaternion lookatWP = Quaternion.LookRotation(_workAreas[_currentWA].transform.position - _citizenTransform.position);
        _citizenTransform.rotation = Quaternion.Slerp(_citizenTransform.rotation, lookatWP, _rotSpeed * Time.deltaTime);

        _citizenTransform.Translate(0, 0, _citizenSpeed * Time.deltaTime);
    }
    private void QuestTimer()
    {
        if (_workTime > 0)
        {
            _workTime -= Time.deltaTime * 100f;
        }

        if (_workTime <= 0f)
        {
            _currentWA = UnityEngine.Random.Range(0, _workAreas.Length);
            _workTime = 2f;
            if (_currentWA >= _workAreas.Length)
            {
                _currentWA = 0;
            }

        }
    }
    #endregion

    private void Talking()
    {

    }

    private void Stay()
    {

    }

    #endregion
}
