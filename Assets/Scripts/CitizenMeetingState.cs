using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenMeetingState : MonoBehaviour, ICitizenState
{
    [SerializeField]
    private Transform _meetingPlace;



    public void CitizenUpdate()
    {
        throw new System.NotImplementedException();
    }
    public void Enter()
    {
        Debug.Log("Meeting started");
    }

    public void Exit()
    {
        Debug.Log("Meeting done");
    }
}
