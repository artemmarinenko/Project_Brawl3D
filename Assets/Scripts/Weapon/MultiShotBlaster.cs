using GameEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotBlaster : MonoBehaviour, IWeapon
{
    private const float NintyDegrees = 90f;
    private const float ZeroDegrees = 0f;

    [SerializeField] private GameObject _startPoint;
    [SerializeField] private GameObject _laserBeamPrefab;
    [SerializeField] private float _fireDistance = 10f;
    [SerializeField] private float _laserBeamSpeed = 2f;
    [SerializeField] private float _angleFromMiddleBeam = -20f;
    [SerializeField] private float _angleStep = 5f;

    private List<GameObject> _laserBeams = new List<GameObject>();
    private List<Vector3> _endPoints = new List<Vector3>();

    private bool _isFire = false;
    private bool _isFireEnded = true;
    private float _time;

    

 
    void Update()
    {
        

        if (_isFire )
        {

            ShootLaserBeams();
        }
    }

    private void PrepareLaserBeams() {
       // _isFireEnded = false;

        for (float i = _angleFromMiddleBeam; i < Mathf.Abs(_angleFromMiddleBeam); i += _angleStep)
        {
            _laserBeams.Add(Instantiate(_laserBeamPrefab, _startPoint.transform.position, Quaternion.Euler(NintyDegrees, _startPoint.transform.rotation.eulerAngles.y + i, _startPoint.transform.rotation.eulerAngles.z)));
            _endPoints.Add(_startPoint.transform.position + (Quaternion.Euler(ZeroDegrees, ZeroDegrees + i, ZeroDegrees) * _startPoint.transform.forward * _fireDistance));
        }

        
    }

    private void ShootLaserBeams()
    {
        for (int i = 0; i < _laserBeams.Count; i++)
        {
            _laserBeams[i].transform.position = Vector3.Lerp(_startPoint.transform.position, _endPoints[i], _time * _laserBeamSpeed);
            //_laserBeams[i].transform.Translate(_endPoints[i]);
        }
        _time += Time.deltaTime;
        for(int i = 0; i < _laserBeams.Count; i++)
        {
            if (_laserBeams[i].transform.position == _endPoints[i])
            {
                foreach (GameObject beam in _laserBeams)
                {
                    Destroy(beam);
                }
                _endPoints.Clear();
                _laserBeams.Clear();
                EventAggregator.Post(this, new AttackEndedEvent());
                _isFire = false;
                //_isFireEnded = true;
                _time = 0;
                return;
            }
        }
        
    }

    public void Attack()
    {
        PrepareLaserBeams();
        _isFire = true;
    }
}
