using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public static int bulletRed = 0;
    public static int bulletGreen = 1;
    public static int bulletBlue = 2;

    private static int maxType = 2;

    public int BulletType { get; private set; }

    public int BulletNumber { get; set; }

    [SerializeField] private Material[] materials;

    public delegate void ballDown(BallScript ballScript);
    public event ballDown BallDownEvent;

    private float lifeTime;
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.mass = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rigidBody.isKinematic)
        {
            lifeTime += Time.deltaTime;
            if ((gameObject.transform.position.y < -5) || (lifeTime > 4))
            {
                BackToBox();
            }
        }
    }

    public void SetType()
    {
        BulletType = Random.Range(0, maxType + 1);
        GetComponent<Renderer>().material = materials[BulletType];
    }

    private void BackToBox()
    {
        rigidBody.AddForce(Vector3.zero);
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        //transform.position = new Vector3(0, 58, 0);
        //gameObject.SetActive(false);
        rigidBody.isKinematic = true;
        lifeTime = 0;
        BallDownEvent?.Invoke(this);
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
