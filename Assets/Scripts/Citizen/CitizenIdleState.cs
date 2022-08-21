using Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CitizenIdleState : ICitizenState
{
    private Transform _citizenTransform;
    private GameObject[] _waypoints;

    private GameObject[] _workAreas;
    private int _currentWA = 0;

    private float _workTime;

    private int _currentWP = 0;

    private float _citizenSpeed;
    private float _rotSpeed;

    private readonly Dictionary<_states, Action> IdleStates = new Dictionary<_states, Action>();
    public enum _states { stay, roaming, working };
    public _states _currentIdleState = _states.stay;

    private int randomInt = UnityEngine.Random.Range(0, 2);

    #region Idle States Settings
    public async void Enter(GameObject concreteCitizen, GameObject[] citizenWayPoints)
    {
        _citizenTransform = concreteCitizen.transform;
        GetGlobalVariables();
        _waypoints = citizenWayPoints;
        await InitIdleStates();
        SetRandomState();
        Debug.Log("Enter into Citizen idle state");
    }
    private void GetGlobalVariables()
    {
        _workAreas = Locator.GetObject<CitizenOpenVariables>().CitizenWorkAreas;
        _rotSpeed = Locator.GetObject<CitizenOpenVariables>().CitizenRotationSpeed;
        _citizenSpeed = Locator.GetObject<CitizenOpenVariables>().CitizenSpeed;
    }
    public void CitizenUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            SetCurrentIdleState(_states.roaming);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            SetCurrentIdleState(_states.stay);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SetCurrentIdleState(_states.working);
        UpdateCurrentState(_currentIdleState);
    }
    public void Exit()
    {
        _currentIdleState = 0;
        IdleStates.Clear();
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
        randomInt = UnityEngine.Random.Range(0, 2);
        if (randomInt > 2)
            randomInt = 0;
        switch (randomInt)
        {
            case 1:
                SetCurrentIdleState(_states.roaming);
                Debug.Log("Roaming");
                break;
            case 2:
                SetCurrentIdleState(_states.working);
                Debug.Log("Idle working");
                break;
            default:
                SetCurrentIdleState(_states.stay);
                Debug.Log("Idle staying");
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

        if (Vector3.Distance(_citizenTransform.position, _waypoints[_currentWP].transform.position) < minRange)
        {
            _currentWP++;
            Debug.Log("Idle wp changed");
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
        var minRange = 3f;
        _workTime = 1f;

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
            Debug.Log(_workTime);
        }

        if (_workTime <= 0f)
        {
            _currentWA++;
            if (_currentWA >= _workAreas.Length)
            {
                SetCurrentIdleState(_states.roaming);
                _currentWA = 0;
            }

        }
    }
    #endregion

    private void Stay()
    {

    }

    #endregion
}
