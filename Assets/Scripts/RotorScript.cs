using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotorScript : MonoBehaviour
{
    private float rotorAngle;

    // Start is called before the first frame update
    void Start()
    {
        rotorAngle = 200f;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * rotorAngle * Time.deltaTime);
    }
}
