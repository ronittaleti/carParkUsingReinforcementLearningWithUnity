using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    public static bool touching = false;

    public Collider boxCollider;

    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Car")
        {

            touching = true;
        }
    }
}
