using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateFlagScript : MonoBehaviour
{
    private bool isInPlace;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
