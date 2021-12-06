using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public delegate void coinDown(CoinScript coinScript);
    public event coinDown CoinDownEvent;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < -5) 
        {
            CoinDown();
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Coin Collision"+ collision.gameObject.tag);
        /*if (collision.gameObject.tag == "GroundTag")
        {
            CoinDown();
        }*/
        if (collision.gameObject.tag == "BallTag")
        {
            Destroy(collision.gameObject);
            CoinDown();
        }
    }

    void CoinDown()
    {
        //Coin to zero
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.zero);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        Vector3 startVector = new Vector3(0f, 0.5f, 0f);
        gameObject.transform.position = startVector;

        //Tower to zero
        CoinDownEvent?.Invoke(this);
    }
}
