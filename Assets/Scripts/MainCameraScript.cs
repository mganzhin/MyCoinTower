using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    private float radius;
    private float time;

    public static Vector3 cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        radius = 26;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += 0.1f * Time.deltaTime;
        transform.position = new Vector3(radius * Mathf.Sin(time), 22, radius * Mathf.Cos(time));
        cameraPosition = transform.position;
        transform.LookAt(Vector3.zero);
    }
}
