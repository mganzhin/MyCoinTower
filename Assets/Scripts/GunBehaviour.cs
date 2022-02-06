using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{

    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject gunBarrel;
    private GameObject gObj;
    private Rigidbody rb;
    private Ray ray;
    private RaycastHit hit;
    private List<bool> rootList;
    private float time = 0;

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        time += Time.deltaTime;
        if (time > 1)
        {
            time = 0;
            StartCoroutine(CheckRootShooting());
        }
    }

    private void OnEnable()
    {
        rootList = new List<bool>();
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

    private IEnumerator CheckRootShooting()
    {
        ray = new Ray(transform.position, Camera.main.transform.position - transform.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider.tag != "TowerTag")
        {
            rootList.Add(true);
        }
        else if (hit.collider.tag == "TowerTag")
        {
            rootList.Clear();
        }
        if (rootList.Count == 3)
        {
            gObj = Instantiate(ball);
            gObj.transform.localScale = Vector3.one / 2f;
            rb = gObj.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            gObj.transform.position = gunBarrel.transform.position;
            yield return StartCoroutine(ToCamera(gObj));
            rootList.Clear();
        }
        yield return null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }






}
