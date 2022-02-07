using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateFlagScript : MonoBehaviour
{
    private bool isInPlace;

    public bool IsInPlace()
    {
        return isInPlace;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "TowerTag")
        {
            isInPlace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "TowerTag")
        {
            isInPlace = false;
        }
    }
}
