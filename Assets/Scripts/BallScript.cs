using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        lifeTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        if ((gameObject.transform.position.y < -5) || (lifeTime > 6))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Strange Ball Collision" + collision.gameObject.tag);
        if (collision.gameObject.tag == "CoinTag")
        {
            Debug.Log("Ball Collision" + collision.gameObject.tag);
            Destroy(gameObject);
        } 
        if (collision.gameObject.tag == "GroundTag")
        {
            Debug.Log("Ball Collision" + collision.gameObject.tag);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "WallTag")
        {
            Debug.Log("Ball Collision" + collision.gameObject.tag);
        }
    }

}
