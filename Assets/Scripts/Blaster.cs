using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour
{
    [SerializeField] private GameObject _startPoint;
    [SerializeField] private GameObject _laserBeamPrefab;
    

    Vector3 endPoint1;
    Vector3 endPoint2;
    Vector3 endPoint3;
    GameObject laserBeam1;
    GameObject laserBeam2;
    GameObject laserBeam3;
    bool isFire = false;
    bool isFireEnded = true;
    float t;

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isFireEnded)
        {
            isFireEnded = false;
            Destroy(laserBeam1);
            
            laserBeam1 = Instantiate(_laserBeamPrefab, _startPoint.transform.position, Quaternion.Euler(90f, _startPoint.transform.rotation.eulerAngles.y, _startPoint.transform.rotation.eulerAngles.z));
            laserBeam2 = Instantiate(_laserBeamPrefab, _startPoint.transform.position, Quaternion.Euler(90f, _startPoint.transform.rotation.eulerAngles.y, _startPoint.transform.rotation.eulerAngles.z));
            laserBeam3 = Instantiate(_laserBeamPrefab, _startPoint.transform.position, Quaternion.Euler(90f, _startPoint.transform.rotation.eulerAngles.y, _startPoint.transform.rotation.eulerAngles.z));

            endPoint1 = _startPoint.transform.position + (_startPoint.transform.forward * 10f);
            endPoint2 = _startPoint.transform.position + (Quaternion.Euler(0,20,0) * _startPoint.transform.forward * 10f);
            endPoint3 = _startPoint.transform.position + (Quaternion.Euler(0, -20, 0) * _startPoint.transform.forward * 10f);

            isFire = true;

        }

        if (isFire)
        {
            
            laserBeam1.transform.position = Vector3.Lerp(_startPoint.transform.position, endPoint1, t*2);
            laserBeam2.transform.position = Vector3.Lerp(_startPoint.transform.position, endPoint2, t * 2);
            laserBeam3.transform.position = Vector3.Lerp(_startPoint.transform.position, endPoint3, t * 2);
            t += Time.deltaTime;

            if(laserBeam1.transform.position == endPoint1 )
            {
                Destroy(laserBeam1);
                Destroy(laserBeam2);
                Destroy(laserBeam3);
                isFire = false;
                isFireEnded = true;
                t = 0;
            }

        }



    }
}
