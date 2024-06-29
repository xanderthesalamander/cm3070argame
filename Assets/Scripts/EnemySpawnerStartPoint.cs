using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerStartPoint : MonoBehaviour
{
    public void SetNoParent()
    {
        transform.SetParent(null);
    }
}
