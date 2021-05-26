using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamBlasterBeam : MonoBehaviour
{
    float _damage = 10f;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            if(character != null)
            {
                character.RecieveDamage(_damage);
                gameObject.SetActive(false);
            }
           
        }
        
            gameObject.SetActive(false);
        
    }
}
