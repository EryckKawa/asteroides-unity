using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLimits : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}
