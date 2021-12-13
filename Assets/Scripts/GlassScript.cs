using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GlassScript : MonoBehaviour, IPointerClickHandler
{
    Vector3 startVector;
    public GameObject BallPrefab;

    public delegate void glassPressed(GlassScript glassScript, Vector3 glassVector);
    public event glassPressed GlassPressedEvent;

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

        GlassPressedEvent?.Invoke(this, startVector);

        /*GameObject ball = GameObject.FindWithTag("BallTag");
        if (ball == null)
        {
            ball = Instantiate(BallPrefab,
                MainCameraScript.cameraPosition,
                Quaternion.identity);
            Rigidbody ballRigid = ball.GetComponent<Rigidbody>();
            ballRigid.AddForce(5 * (startVector - ball.transform.position), ForceMode.Impulse);
        }*/
    }
}
