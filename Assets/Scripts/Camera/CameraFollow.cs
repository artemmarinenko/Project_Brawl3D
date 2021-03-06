using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{ 
    [SerializeField] private float _zCoordinateOffset;
    [SerializeField] private float _interpolationSpeed;

    public Transform FollowedObject { get; set; }

    void LateUpdate()
    { 
        if(FollowedObject != null) {
            Vector3 position = transform.position;
            position.z = Mathf.Lerp(transform.position.z, FollowedObject.position.z - _zCoordinateOffset, _interpolationSpeed * Time.deltaTime);
            transform.position = position;
        }

    }
}
