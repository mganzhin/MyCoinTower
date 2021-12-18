using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderScript : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;
    private static GameObject builderSite;
    private static GameController gameController;

    private const int businessStay = 0;
    private const int businessGoToBrokenBrick = 1;
    private const int businessBringBrokenBrick = 2;
    private const int businessWalking = 3;
    private int Business { get; set; }
    private GameObject targetBrick;
    

    // Start is called before the first frame update
    void Start()
    {
        Business = businessStay;
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
        }
        if (builderSite == null)
        {
            builderSite = GameObject.FindGameObjectWithTag("BuildingSite");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation != Quaternion.identity)
        {
            transform.rotation = Quaternion.identity;
        }

        if (transform.position.y < -5)
        {
            float angle = Random.Range(0f, 14f);
            transform.position = new Vector3(21 * Mathf.Cos(angle), 3f, 21 * Mathf.Sin(angle));
            Business = businessStay;
        }

        if (ShitButtonBehaviour.isPressed) Business = businessStay;

        switch (Business)
        {
            case businessStay:
                if (targetBrick != null)
                {
                    targetBrick.GetComponent<CubeBrickScript>().SetBrickInHands(false);
                    targetBrick = null;
                }
                float distance = 100;
                if (gameController != null)
                {
                    List<GameObject> towerList = gameController.GetBrickList();
                    for (int i = 0; i < towerList.Count; i++)
                    {
                        CubeBrickScript cubeScript = towerList[i].GetComponent<CubeBrickScript>();
                        if ((!cubeScript.IsBrickInPlace()) && (!cubeScript.IsBrickInHands()))
                        {
                            if ((towerList[i].gameObject.transform.position - transform.position).magnitude < distance)
                            {
                                targetBrick = towerList[i];
                                targetBrick.GetComponent<Renderer>().material = targetMaterial;
                                distance = (towerList[i].gameObject.transform.position - transform.position).magnitude;
                            }
                        }
                    }
                    if (targetBrick != null)
                    {
                        Business = businessGoToBrokenBrick;
                        targetBrick.GetComponent<CubeBrickScript>().SetBrickInHands(true);
                    }
                    else
                    {
                        Business = businessWalking;
                    }
                }
                break;
            case businessGoToBrokenBrick:
                if (targetBrick != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetBrick.transform.position, 3 * Time.deltaTime);
                }
                break;
            case businessBringBrokenBrick:
                if (targetBrick != null)
                {
                    if ((transform.position - targetBrick.transform.position).magnitude < 4)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, builderSite.transform.position, 2 * Time.deltaTime);
                        targetBrick.transform.position = Vector3.MoveTowards(targetBrick.transform.position, transform.position, 2 * Time.deltaTime);
                    }
                    else
                    {
                        Debug.Log("Faraway");
                        Business = businessStay;
                    }
                }
                break;
            case businessWalking:
                if ((transform.position - builderSite.transform.position).magnitude < 20)
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position * 2, 3 * Time.deltaTime);
                }
                else
                {
                    Business = businessStay;
                }
                break;
        }

    }

    void TriggerBuilderSite(Collider other)
    {
        if (other.gameObject.tag == "BuildingSite") 
        {
            if (Business == businessGoToBrokenBrick)
            {
                if (targetBrick == null)
                {
                    Business = businessStay;
                }
                else
                {
                    if ((transform.position - targetBrick.transform.position).magnitude > 4)
                    {
                        Business = businessWalking;
                        targetBrick.GetComponent<CubeBrickScript>().SetBrickInHands(false);
                        targetBrick = null;
                    }
                    else
                    {
                        Business = businessBringBrokenBrick;
                    }
                }
            }
            else
            {
                if (Business == businessBringBrokenBrick)
                {
                    Debug.Log("Try to place");
                    //Find broken tower wall
                    GameObject lowestBrokenFlag = null;
                    float y = 100;
                    List<GameObject> templateList = gameController.GetFlagList();
                    for (int i = 0; i < templateList.Count; i++)
                    {
                        if (!templateList[i].GetComponent<TemplateFlagScript>().IsInPlace())
                        {
                            if (templateList[i].gameObject.transform.position.y < y)
                            {
                                lowestBrokenFlag = templateList[i];
                                y = templateList[i].gameObject.transform.position.y;
                            }
                        }
                    }
                    //if tower has broken wall
                    if (lowestBrokenFlag != null)
                    {
                        //Place brick to flag;
                        if (targetBrick != null)
                        {
                            targetBrick.GetComponent<CubeBrickScript>().SetBrickInHands(false);
                            targetBrick.GetComponent<Rigidbody>().AddForce(Vector3.zero);
                            targetBrick.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            targetBrick.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                            targetBrick.transform.rotation = Quaternion.identity;
                            targetBrick.transform.position = lowestBrokenFlag.transform.position;
                        }
                    }
                    Business = businessStay;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("BuilderTrigger: " + other.gameObject.name);
        TriggerBuilderSite(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("BuilderTrigger: " + collision.gameObject.name);
        if ((collision.gameObject.tag == "TowerTag") && (Business == businessGoToBrokenBrick))
        {
            Business = businessBringBrokenBrick;
        }
    }
}

