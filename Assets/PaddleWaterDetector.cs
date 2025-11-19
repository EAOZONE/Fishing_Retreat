using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleWaterDetector : MonoBehaviour
{

    private bool isInWater = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            isInWater = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
            isInWater = false;
    }

    public bool IsInWater()
    {
        return isInWater;
    }
}
