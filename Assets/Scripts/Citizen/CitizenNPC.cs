using Internal;
using UnityEngine;

public interface ICitizenState
{
    void CitizenUpdate();
    void Exit();
    void Enter(GameObject concreteCitizen, GameObject[] citizenWayPoits);
}
public class CitizenNPC : MonoBehaviour
{
    private int _healthPoint;

    private CitizenStateController _stateManager;

    private void Start()
    {
        _stateManager = Locator.GetObject<CitizenStateController>();
        Debug.Log("Starting Citizen");
        ChooseState();
    }
    private void ChooseState()
    {
        Debug.Log("choosing random(or not) State");
        _stateManager.SetIdleState();
    }
}
