using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    float lifeTime;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            lifeTime += Time.deltaTime;
            if ((gameObject.transform.position.y < -5) || (lifeTime > 6))
            {
                BackToBox();
            }
        }
    }

    private void BackToBox()
    {
        rigidBody.AddForce(Vector3.zero);
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(0, 58, 0);
        gameObject.SetActive(false);
        lifeTime = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CoinTag")
        {
            BackToBox();
        } 
        if (collision.gameObject.tag == "GroundTag")
        {
            BackToBox();
        }
    }

}
