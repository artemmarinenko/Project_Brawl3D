using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalEmmiter : MonoBehaviour
{
    [SerializeField] private GameObject _crystalPrefab;
    [SerializeField] private Transform _emmiter;
    [SerializeField] private float _emissionForce;
    [SerializeField] private float _emissionAngle;
    [SerializeField] private float _secondsBetweenEmission;
    private bool _isEmissionDone = true;

    private void Start()
    {
        GameObject crystal = Instantiate(_crystalPrefab, _emmiter.position, Quaternion.identity);
        crystal.GetComponent<Rigidbody>().AddForce(_emmiter.forward * _emissionForce, ForceMode.Impulse);
        _emmiter.forward = Quaternion.Euler(_emmiter.rotation.x, _emmiter.rotation.y + _emissionAngle, _emmiter.rotation.z) * _emmiter.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEmissionDone)
        {
            StartCoroutine(CrystalCoroutine(_secondsBetweenEmission));
        }
        
    }

    IEnumerator CrystalCoroutine(float seconds)
    {
        _isEmissionDone = false;
        yield return new WaitForSeconds(seconds);
        GameObject crystal = Instantiate(_crystalPrefab, _emmiter.position, Quaternion.identity);
        crystal.GetComponent<Rigidbody>().AddForce(_emmiter.forward * _emissionForce, ForceMode.Impulse);
        _emmiter.forward = Quaternion.Euler(_emmiter.rotation.x, _emmiter.rotation.y + _emissionAngle, _emmiter.rotation.z) * _emmiter.forward;
        _isEmissionDone = true; ;
    }
}
