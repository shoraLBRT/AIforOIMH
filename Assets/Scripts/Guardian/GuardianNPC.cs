using Internal;
using UnityEngine;

public interface IGuardianState
{
    void GuardianUpdate();
    void Exit();
    void EnterAsync(GameObject concreteGuardian, GameObject[] guardianWayPoits);
}
public class GuardianNPC : MonoBehaviour
{
    private int _healthPoint;

    private GuardianController _stateManager;

    private void Start()
    {
        _stateManager = Locator.GetObject<GuardianController>();
        ChooseState();
    }
    private void ChooseState()
    {
        _stateManager.SetIdleState();
    }
}
