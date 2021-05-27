using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaBlasterBeam : MonoBehaviour
{
    // Start is called before the first frame update
    float _damage = 20f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            if (character != null)
            {
                character.RecieveDamage(_damage);
                gameObject.SetActive(false);
            }

        }
        
            gameObject.SetActive(false);
        
    }
}
