using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    private float radius, time;

    public static Vector3 cameraPosition;

    void Start()
    {
        radius = 26;
        time = 0;
    }

    void Update()
    {
        time += 0.1f * Time.deltaTime;
        transform.position = new Vector3(radius * Mathf.Sin(time), 22 + Mathf.Sin(time * 5 * Random.Range(1,2)), radius * Mathf.Cos(time));
        cameraPosition = transform.position;
        transform.LookAt(Vector3.zero);
    }

}
