using Internal;
using System.Threading.Tasks;
using UnityEngine;

public class CitizenNightState : ICitizenState
{
    private Transform _house;
    private Transform _citizenTransform;

    private float _citizenSpeed;
    private float _rotSpeed;
    private float _minRange = 3f;

    private bool _citizenOnBed = false;
    public async void Enter(GameObject concreteCitizen, GameObject[] citizenWayPoits)
    {
        _citizenTransform = concreteCitizen.transform;
        Debug.Log("Meeting started");
        await GetGlobalVariables();
    }
    private async Task GetGlobalVariables()
    {
        _house = Locator.GetObject<CitizenOpenVariables>().House;
        _rotSpeed = Locator.GetObject<CitizenOpenVariables>().CitizenRotationSpeed;
        _citizenSpeed = Locator.GetObject<CitizenOpenVariables>().CitizenSpeed;
        await Task.Delay(50);
    }
    public void CitizenUpdate()
    {
        if (!_citizenOnBed)
            GoToSleep();
    }
    private void GoToSleep()
    {
        if (Vector3.Distance(_citizenTransform.position, _house.position) < _minRange)
        {
            _citizenOnBed = true;
            Debug.Log("Citizen Sleeping");
        }
        else _citizenOnBed = false;

        Quaternion lookatWP = Quaternion.LookRotation(_house.position - _citizenTransform.position);

        _citizenTransform.rotation = Quaternion.Slerp(_citizenTransform.rotation, lookatWP, _rotSpeed * Time.deltaTime);

        _citizenTransform.Translate(0, 0, _citizenSpeed * Time.deltaTime);
    }

    public void Exit()
    {
        _citizenOnBed = false;
    }
}
