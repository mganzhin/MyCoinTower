using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GlassScript : MonoBehaviour, IPointerClickHandler
{
    Vector3 startVector;
    public GameObject BallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        //startVector = eventData.pointerCurrentRaycast.gameObject.transform.position;
        startVector = eventData.pointerCurrentRaycast.worldPosition;
        if (startVector != null)
        {
            Debug.Log("Click x: " + startVector.x + "y: " + startVector.y + "z: " + startVector.z);
        }
        GameObject ball = GameObject.FindWithTag("BallTag");
        if (ball == null)
        {
            ball = Instantiate(BallPrefab,
                new Vector3(0, 22, -26),
                Quaternion.Euler(0, 0, 0));
            Rigidbody ballRigid = ball.GetComponent<Rigidbody>();
            ballRigid.AddForce(3 * (startVector - ball.transform.position), ForceMode.Impulse);
        }
    }
}
