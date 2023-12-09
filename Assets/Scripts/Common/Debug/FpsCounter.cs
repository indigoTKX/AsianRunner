using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _fpsCounterLabel;
    [SerializeField]private float _refreshTime = 0.5f;
    
    private int _frameCounter = 0;
    private float _timeCounter = 0.0f;
    private float _lastFramerate = 0.0f;

    private void Update()
    {
        if( _timeCounter < _refreshTime )
        {
            _timeCounter += Time.deltaTime;
            _frameCounter++;
        }
        else
        {
            _lastFramerate = _frameCounter / _timeCounter;
            _frameCounter = 0;
            _timeCounter = 0.0f;
        }

        _fpsCounterLabel.text = ((int)_lastFramerate).ToString();
    }
}
