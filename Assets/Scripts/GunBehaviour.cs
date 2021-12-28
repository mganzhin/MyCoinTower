using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{

    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject gunBarrel;
    private GameObject gObj;
    private Rigidbody rb;
    private float time = 0;
    private Ray ray;
    private RaycastHit hit;

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        time += Time.deltaTime;
        ray = new Ray(transform.position, Camera.main.transform.position - transform.position);
        Physics.Raycast(ray, out hit);
        if (time > 3 && hit.collider.tag != "TowerTag")
        {
            time = 0;
            gObj = Instantiate(ball);
            rb = gObj.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            gObj.transform.position = gunBarrel.transform.position;
            StartCoroutine(ToCamera(gObj));
        }
    }

    private IEnumerator ToCamera(GameObject gObj)
    {
        while (gObj.transform.position.magnitude < Camera.main.transform.position.magnitude - 1)
        {
            gObj.transform.position = Vector3.MoveTowards(gObj.transform.position, Camera.main.transform.position, 0.5f);
            yield return null;
        }
        Destroy(gObj);
    }

    


}
