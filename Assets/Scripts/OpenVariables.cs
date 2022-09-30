using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Internal;

public class OpenVariables : MonoBehaviour
{
    [SerializeField] public Transform[] Houses;
    [SerializeField] public Transform[] SecurityZones;
    [SerializeField] public Transform MeetingPlace;
    private void Awake()
    {
        Locator.Register<OpenVariables>(this);
    }
}
