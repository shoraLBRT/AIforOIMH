using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractNPC : MonoBehaviour
{
    private int Health;

    protected abstract void SetIdleState();
    protected abstract void SetMeetingState();
    protected abstract void SetAgressiveState();
    protected abstract void SetNightState();
}