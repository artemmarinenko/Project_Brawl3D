using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _followedObject;
    [SerializeField] private float _zCoordinateOffset;

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    { 
        //new Vector3(transform.position.x, transform.position.y, _followedObject.position.z - _zCoordinateOffset);
        Vector3 position = transform.position;
        position.z = Mathf.Lerp(transform.position.z, _followedObject.position.z - _zCoordinateOffset, 0.5f * Time.deltaTime);
        transform.position = position;

    }
}
