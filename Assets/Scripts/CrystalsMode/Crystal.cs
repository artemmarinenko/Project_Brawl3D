using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character) ){

            character.CrystalsInPack++;
            Destroy(gameObject);
        }
    }
}
