using GameEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotBlaster : MonoBehaviour, IWeapon
{
    private const float NintyDegrees = 90f;
    private const float ZeroDegrees = 0f;

    [SerializeField] private GameObject _startPoint;
    [SerializeField] private GameObject _laserBeamPrefab;
    [SerializeField] private float _fireDistance = 10f;
    [SerializeField] private float _laserBeamSpeed = 2f;
    

    private bool _isFire = false;
    //private bool _isFireEnded = true;
    private float _time;

    private Vector3 _endPoint;
    private GameObject _bigLaserBeam;

    private void OnDestroy()
    {
        Destroy(_bigLaserBeam);
    }

    void Update()
    {
        
        if (_isFire)
        {
            ShootLaserBeams();
        }
    }

    private void PrepareLaserBeams()
    {
        //_isFireEnded = false;

        _bigLaserBeam = Instantiate(_laserBeamPrefab, _startPoint.transform.position, Quaternion.Euler(NintyDegrees, _startPoint.transform.rotation.eulerAngles.y, _startPoint.transform.rotation.eulerAngles.z));
        _endPoint = _startPoint.transform.position + ( _startPoint.transform.forward * _fireDistance);
    }

    private void ShootLaserBeams()
    {
            _bigLaserBeam.transform.position = Vector3.Lerp(_startPoint.transform.position, _endPoint, _time * _laserBeamSpeed);
            //_laserBeams[i].transform.Translate(_endPoints[i]);
        
        _time += Time.deltaTime;
        
            if (_bigLaserBeam.transform.position == _endPoint)
            {
                
                 Destroy(_bigLaserBeam);
                _endPoint = Vector3.zero;
                
                EventAggregator.Post(this, new AttackEndedEvent());
                _isFire = false;
                //_isFireEnded = true;
                _time = 0;
                return;
            }
        

    }

    public void Attack()
    {
        PrepareLaserBeams();
        _isFire = true;
    }
}
