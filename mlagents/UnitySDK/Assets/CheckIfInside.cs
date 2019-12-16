using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfInside : MonoBehaviour
{

    public static bool isInside = false;
    public static bool isAlmostInside = false;

    public Collider boxCollider;

    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Car")
        {
            if (boxCollider.bounds.Contains(other.bounds.max) && boxCollider.bounds.Contains(other.bounds.min))
            {
                isAlmostInside = false;
                isInside = true;
            }
            else
            {
                isAlmostInside = true;
                isInside = false;
            }
        }
    }
}
