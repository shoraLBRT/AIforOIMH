using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGameTime : MonoBehaviour
{
    [SerializeField] private Text _timeText;
    private int _timeVelocity = 15;
    TimeSpan ts = new TimeSpan(2, 2, 2);

    private void Update()
    {
        _timeText.text = ts.ToString(@"hh\:mm\:ss");
        ts = ts.Add(TimeSpan.FromSeconds(_timeVelocity * Time.deltaTime));
    }
}
