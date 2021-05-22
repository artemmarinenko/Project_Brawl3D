using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour
{
    private const float NintyDegrees = 90f;

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
        if (Input.GetKeyDown(KeyCode.E) && _isFireEnded)
        {
            _isFireEnded = false;
            
            for (float i = _angleFromMiddleBeam; i < Mathf.Abs(_angleFromMiddleBeam); i += _angleStep) { 
                _laserBeams.Add(Instantiate(_laserBeamPrefab, _startPoint.transform.position, Quaternion.Euler(NintyDegrees, _startPoint.transform.rotation.eulerAngles.y, _startPoint.transform.rotation.eulerAngles.z)));

                _endPoints.Add(_startPoint.transform.position + (Quaternion.Euler(0, 0 + i, 0) * _startPoint.transform.forward * _fireDistance));
            }

            _isFire = true;

        }

        if (_isFire)
        {

            for (int i = 0; i < _laserBeams.Count; i++)
            {
                _laserBeams[i].transform.position = Vector3.Lerp(_startPoint.transform.position, _endPoints[i], _time * _laserBeamSpeed);
            }

            _time += Time.deltaTime;

            if (_laserBeams[0].transform.position == _endPoints[0] )
            {
                foreach (GameObject beam in _laserBeams)
                {
                    Destroy(beam);
                }
                _endPoints.Clear();
                _laserBeams.Clear();

                _isFire = false;
                _isFireEnded = true;
                _time = 0;
            }

        }



    }
}
