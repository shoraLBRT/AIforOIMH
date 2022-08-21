using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Internal;
using UnityEngine.UI;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _ManagerInterface;

    public delegate void MeetingTime();
    public event MeetingTime GoToMeeting;

    public delegate void TimeToSleep();
    public event TimeToSleep GoToTheBed;

    public delegate void CitizenIdleState();
    public event CitizenIdleState CitizenIdle;

    public delegate void CitizenIdleRoaming();
    public event CitizenIdleRoaming CitizenRoaming;

    public delegate void CitizenIdleWorking();
    public event CitizenIdleWorking CitizenWorking;

    //[SerializeField] private Text _citizenCurrentState;

    private void Awake()
    {
        Locator.Register<AIManager>(this);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            _ManagerInterface.SetActive(!_ManagerInterface.activeSelf);
    }
    public void DoTheMeeting()
    {
        GoToMeeting();
    }
    public void NightState()
    {
        GoToTheBed();
    }
    public void CitizenSetIdleState()
    {
        CitizenIdle();
    }
    public void CitizenSetIdleRoaming()
    {
        CitizenRoaming();
    }
    public void CitizenSetIdleWorking()
    {
        CitizenWorking();
    }
}
