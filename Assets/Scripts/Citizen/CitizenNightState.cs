using Internal;
using System.Threading.Tasks;
using UnityEngine;

public class CitizenNightState : ICitizenState
{
    private Transform[] _houses;
    private Transform _citizenTransform;

    private float _citizenSpeed;
    private float _rotSpeed;
    private float _minRange = 3f;

    private int _randomHouse;
    private bool _citizenOnBed = false;
    public async void EnterAsync(GameObject concreteCitizen, GameObject[] citizenWayPoits)
    {
        _citizenTransform = concreteCitizen.transform;
        Debug.Log("Meeting started");
        await GetGlobalVariables();
        _randomHouse = Random.Range(0, 3);
    }
    private async Task GetGlobalVariables()
    {
        _houses = Locator.GetObject<OpenVariables>().Houses;
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
        if (Vector3.Distance(_citizenTransform.position, _houses[_randomHouse].position) < _minRange)
        {
            _citizenOnBed = true;
            Debug.Log("Citizen Sleeping");
        }
        else _citizenOnBed = false;

        Quaternion lookatWP = Quaternion.LookRotation(_houses[_randomHouse].position - _citizenTransform.position);

        _citizenTransform.rotation = Quaternion.Slerp(_citizenTransform.rotation, lookatWP, _rotSpeed * Time.deltaTime);

        _citizenTransform.Translate(0, 0, _citizenSpeed * Time.deltaTime);
    }

    public void Exit()
    {
        _citizenOnBed = false;
    }
}
