using Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CitizenAgressiveState : ICitizenState
{
    private CitizenStateController _stateController;

    private Transform _targetForAgressive;

    private Transform _citizenTransform;
    private GameObject _citizenGO;

    private float _citizenSpeed = 10f;
    private float _rotSpeed = 5f;

    private bool _citizenIsSafe;

    private readonly Dictionary<_reactions, Action> ReactionsDict = new Dictionary<_reactions, Action>();
    public enum _reactions { stay, run, fight, safe };
    public _reactions _currentReaction;
    public async void EnterAsync(GameObject concreteCitizen, GameObject[] citizenWayPoits)
    {
        _stateController = Locator.GetObject<CitizenStateController>();
        _targetForAgressive = _stateController.TheWriter.transform;

        await InitIdleStates();

        ChoiceReaction();

        _citizenTransform = concreteCitizen.transform;
        _citizenGO = concreteCitizen;
        _citizenIsSafe = false;
    }

    public void CitizenUpdate()
    {
        UpdateCurrentReaction(_currentReaction);
    }

    public void Exit()
    {
        _citizenGO.GetComponent<Renderer>().material.color = Color.white;
        SetCurrentReaction(_reactions.stay);
        ReactionsDict.Clear();
    }

    private void ChoiceReaction()
    {
        int randomInt = UnityEngine.Random.Range(1, 3);
        switch (randomInt)
        {
            case 1:
                SetCurrentReaction(_reactions.run);
                break;
            case 2:
                SetCurrentReaction(_reactions.fight);
                break;
            default:
                SetCurrentReaction(_reactions.stay);
                break;
        }
    }
    public void SetCurrentReaction(_reactions state)
    {
        _currentReaction = _reactions.stay;
        _currentReaction = state;
    }
    public void UpdateCurrentReaction(_reactions state)
    {
        ReactionsDict[state].Invoke();
    }
    public async Task InitIdleStates()
    {
        ReactionsDict.Add(_reactions.stay, CitizenStaying);
        ReactionsDict.Add(_reactions.run, RunReaction);
        ReactionsDict.Add(_reactions.fight, FightReaction);
        ReactionsDict.Add(_reactions.safe, ExitFromAggressiveState);
        await Task.Delay(100);
    }

    private void ExitFromAggressiveState()
    {
        _stateController.SetIdleState();
    }

    #region Reactions

    private void RunReaction()
    {
        _citizenGO.GetComponent<Renderer>().material.color = Color.yellow;
        var safeRange = 300f;

        if (Vector3.Distance(_targetForAgressive.position, _citizenTransform.position) > safeRange)
        {
            _citizenIsSafe = true;
            ExitFromAggressiveState();
        }

        if (_citizenIsSafe == false)
        {
            Quaternion lookAtSafeDirection = Quaternion.LookRotation(-(_targetForAgressive.transform.position - _citizenTransform.position));

            _citizenTransform.rotation = Quaternion.Slerp(_citizenTransform.rotation, lookAtSafeDirection, _rotSpeed * Time.deltaTime);

            _citizenTransform.Translate(0, 0, _citizenSpeed * Time.deltaTime);
        }
    }

    private void FightReaction()
    {
        _citizenGO.GetComponent<Renderer>().material.color = Color.red;
        var safeRange = 200f;
        var fightRange = 3f;

        if (Vector3.Distance(_targetForAgressive.position, _citizenTransform.position) > safeRange)
        {
            _citizenIsSafe = true;
            ExitFromAggressiveState();
        }

        if (!_citizenIsSafe)
        {
            Quaternion lookAtTarget = Quaternion.LookRotation(_targetForAgressive.transform.position - _citizenTransform.position);

            _citizenTransform.rotation = Quaternion.Slerp(_citizenTransform.rotation, lookAtTarget, _rotSpeed * Time.deltaTime);

            _citizenTransform.Translate(0, 0, _citizenSpeed * Time.deltaTime);

            if (Vector3.Distance(_targetForAgressive.position, _citizenTransform.position) < fightRange)
            {
                CitizenAttacking();
            }
        }
    }

    #endregion  

    private void CitizenAttacking()
    {
        Debug.Log("citizen attacking");
    }
    private void CitizenStaying()
    {
        Debug.Log("citizen is aggressive, but staying");
    }
}
