using Internal;
using System.Threading.Tasks;
using UnityEngine;

public class CitizenMeetingState : ICitizenState
{
    private Transform _meetingPlace;
    private Transform _citizenTransform;

    private float _citizenSpeed;
    private float _rotSpeed;
    private float _minRange = 3f;

    private bool _citizenOnMeeting = false;

    public async void Enter(GameObject concreteCitizen, GameObject[] citizenWayPoits)
    {
        _citizenTransform = concreteCitizen.transform;
        Debug.Log("Meeting started");
        await GetGlobalVariables();
    }
    private async Task GetGlobalVariables()
    {
        //_citizenTransform = Locator.GetObject<CitizenOpenVariables>().CitizenTransform;

        _meetingPlace = Locator.GetObject<CitizenOpenVariables>().MeetingPlace;
        _rotSpeed = Locator.GetObject<CitizenOpenVariables>().CitizenRotationSpeed;
        _citizenSpeed = Locator.GetObject<CitizenOpenVariables>().CitizenSpeed;
        await Task.Delay(50);
    }
    public void CitizenUpdate()
    {
        if (!_citizenOnMeeting)
            GoingToTheMeeting();
        else DoTheMeeting();
    }
    public void Exit()
    {
        _citizenOnMeeting = false;
    }
    private void DoTheMeeting()
    {
        if (Vector3.Distance(_citizenTransform.position, _meetingPlace.position) > _minRange)
        {
            _citizenOnMeeting = false;
        }
    }
    private void GoingToTheMeeting()
    {
        if (Vector3.Distance(_citizenTransform.position, _meetingPlace.position) < _minRange)
        {
            _citizenOnMeeting = true;
            Debug.Log("Citizen on meeting");
        }
        else _citizenOnMeeting = false;

        Quaternion lookatWP = Quaternion.LookRotation(_meetingPlace.position - _citizenTransform.position);

        _citizenTransform.rotation = Quaternion.Slerp(_citizenTransform.rotation, lookatWP, _rotSpeed * Time.deltaTime);

        _citizenTransform.Translate(0, 0, _citizenSpeed * Time.deltaTime);
    }
}
