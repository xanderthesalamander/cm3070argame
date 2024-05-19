using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBulletImpact : MonoBehaviour
{
    public bool allowDestroy = true;
    
    private void OnCollisionEnter(Collision collision)
    {
        // Destroy when hit by a bullet
        if (allowDestroy && collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
