using Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GuardianAgressiveState : IGuardianState
{
    private GuardianController _stateController;

    private Transform _targetForAgressive;

    private Transform _guardianTransform;
    private GameObject _guardianGO;

    private float _guardianSpeed = 15f;
    private float _rotSpeed = 8f;

    private bool _guardianIsSafe;

    private readonly Dictionary<_guardReactions, Action> ReactionsDict = new Dictionary<_guardReactions, Action>();
    public enum _guardReactions { stay, fight, getHelp };
    public _guardReactions _currentReaction;
    public async void EnterAsync(GameObject concreteGuardian, GameObject[] guardianWayPoits)
    {
        await InitIdleStates();
        _stateController = Locator.GetObject<GuardianController>();
        _targetForAgressive = _stateController.TheWriter.transform;

        _guardianTransform = concreteGuardian.transform;
        _guardianGO = concreteGuardian;
        _guardianIsSafe = false;

        ChoiceReaction();
    }

    public void GuardianUpdate()
    {
        UpdateCurrentReaction(_currentReaction);
    }

    public void Exit()
    {
        _guardianGO.GetComponent<Renderer>().material.color = Color.white;
        //SetReaction(_guardReactions.stay);
        _currentReaction = _guardReactions.stay;
        ReactionsDict.Clear();
    }

    private void ChoiceReaction()
    {
        int randomInt = UnityEngine.Random.Range(1, 3);
        switch (randomInt)
        {
            case 1:
                _currentReaction = _guardReactions.getHelp;
                break;
            case 2:
                _currentReaction = _guardReactions.fight;
                break;
            default:
                _currentReaction = _guardReactions.stay;
                break;
        }
    }
    //public void SetReaction(_guardReactions state)
    //{
    //    //_currentReaction = _guardReactions.stay;
    //    _currentReaction = state;
    //}
    public void UpdateCurrentReaction(_guardReactions state)
    {
        ReactionsDict[state].Invoke();
    }
    public async Task InitIdleStates()
    {
        ReactionsDict.Add(_guardReactions.stay, GuardianStaying);
        ReactionsDict.Add(_guardReactions.getHelp, GettingHelp);
        ReactionsDict.Add(_guardReactions.fight, FightReaction);
        await Task.Delay(300);
    }

    private void ExitFromAggressiveState()
    {
        _stateController.SetIdleState();
    }

    #region Reactions

    private void GettingHelp()
    {
        _guardianGO.GetComponent<Renderer>().material.color = Color.green;
        Debug.Log("guardian getting help");
    }

    private void FightReaction()
    {
        _guardianGO.GetComponent<Renderer>().material.color = Color.red;
        var safeRange = 1000f;
        var fightRange = 3f;

        if (Vector3.Distance(_targetForAgressive.position, _guardianTransform.position) > safeRange)
        {
            _guardianIsSafe = true;
            ExitFromAggressiveState();
        }

        if (!_guardianIsSafe)
        {
            Quaternion lookAtTarget = Quaternion.LookRotation(_targetForAgressive.transform.position - _guardianTransform.position);

            _guardianTransform.rotation = Quaternion.Slerp(_guardianTransform.rotation, lookAtTarget, _rotSpeed * Time.deltaTime);

            _guardianTransform.Translate(0, 0, _guardianSpeed * Time.deltaTime);

            if (Vector3.Distance(_targetForAgressive.position, _guardianTransform.position) < fightRange)
            {
                GuardianAttacking();
            }
        }
    }

    #endregion  

    private void GuardianAttacking()
    {
        Debug.Log("Guardian attacking");
    }
    private void GuardianStaying()
    {
        Debug.Log("Guardian is aggressive, but staying");
    }
}
