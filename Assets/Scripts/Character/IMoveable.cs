using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable 
{
    void MoveControll(Vector3 direction);
    void DOMove(Vector3 direction);
}
