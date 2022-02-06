using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuilderScript : MonoBehaviour
{
    [SerializeField] private float builderSpeed;

    private bool debug = true, delta = false;
    private int remb = -1;

    private static GameObject builderSite;
    private static GameController gameController;

    private const int businessFind = 0;
    private const int businessGoToBrokenBrick = 1;
    private const int businessBringBrokenBrick = 2;
    private const int businessGoToBuilderSite = 3;
    private const int businessWalking = 4;
    private const int businessStay = 5;
    private int Business { get; set; }
    private GameObject[] targetBuilderHouses, placeBrickHeres;
    private GameObject targetBrick, placeBrickHere, targetBuilderHouse;
    private NavMeshAgent agent;
    private GameObject cube;
    

    // Start is called before the first frame update
    void Start()
    {
        Business = businessFind;
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
        }
        if (builderSite == null)
        {
            builderSite = GameObject.FindGameObjectWithTag("BuildingSite");
        }
        agent = GetComponent<NavMeshAgent>();
        agent.speed = builderSpeed;
        targetBuilderHouses = GameObject.FindGameObjectsWithTag("BuilderBuildings");
        placeBrickHeres = GameObject.FindGameObjectsWithTag("BrickHere");
        placeBrickHere = FindMinDist(placeBrickHeres);
        targetBuilderHouse = FindMinDist(targetBuilderHouses);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)
        {
            float angle = Random.Range(0f, 14f);
            transform.position = new Vector3(21 * Mathf.Cos(angle), 3f, 21 * Mathf.Sin(angle));
            Business = businessFind;
        }
        if (ShitButtonBehaviour.isPressed)
        {
            if (!delta)
            {
                delta = true;
                remb = Business;
                Business = businessStay;
            }
        }
        if (GameController.isGunsShowed)
        {
            Business = businessStay;
        }
        switch (Business)
        {
            case businessFind:
                if (gameController != null)
                {
                    List<CubeBrickScript> towerList = gameController.GetBrickList();
                    List<GameObject> templateList = gameController.GetFlagList();
                    for (int i = 0; i < towerList.Count; i++)
                    {
                        if (!templateList[i].GetComponent<TemplateFlagScript>().IsInPlace() && !towerList[i].IsBrickInHands())
                        {
                            Deblog.Log(debug, $"{gameObject.name}: Find broken wall");
                            Business = businessGoToBrokenBrick;
                        } 
                    }
                }
                break;
            case businessGoToBrokenBrick:
                Deblog.Log(debug, $"{gameObject.name}: Go to Builders house");
                agent.SetDestination(targetBuilderHouse.transform.position);
                break;
            case businessBringBrokenBrick:
                bool hasBrick = false;
                int index = -1;
                List<CubeBrickScript> towerList1 = gameController.GetBrickList();
                for (int i = 0; i < towerList1.Count; i++)
                {
                    if (!towerList1[i].IsBrickInPlace() && !towerList1[i].isActiveAndEnabled && !towerList1[i].IsBrickInHands())
                    {
                        index = i;
                        cube = towerList1[i].gameObject;
                        hasBrick = true;
                    }
                }
                if (cube != null && hasBrick)
                {
                    towerList1[index].SetBrickInHands(true);
                    cube.SetActive(true);
                    cube.transform.position = placeBrickHere.transform.position;
                    cube.GetComponent<BoxCollider>().enabled = false;
                    cube.GetComponent<Rigidbody>().isKinematic = true;
                    Deblog.Log(debug, $"{gameObject.name}: Get the block");
                    Business = businessGoToBuilderSite;
                }
                else
                {
                    Business = businessFind;
                }
                break;
            case businessGoToBuilderSite:
                if (cube != null)
                {
                    Deblog.Log(debug, $"{gameObject.name}: Go to BuilderSite");
                    agent.SetDestination(builderSite.transform.position);
                    ControlDist(cube, gameObject, 2);
                }
                else
                {
                    Deblog.Log(debug, $"{gameObject.name}: Brick broken, try to find new brick");
                    Business = businessFind;
                }
                break;
            case businessWalking:
                Deblog.Log(debug, $"{gameObject.name}: Walking");
                agent.SetDestination(targetBuilderHouse.transform.position);
                break;
            case businessStay:
                Deblog.Log(debug, $"{gameObject.name}: Staying");
                agent.SetDestination(transform.position);
                break;
        }

        if (!ShitButtonBehaviour.isPressed && delta)
        {
            Business = remb;
            delta = false;
        }

    }

    void TriggerBuilderSite(Collider other)
    {
        if (other.gameObject.tag == "BuildingSite") 
        {
            if (Business == businessGoToBuilderSite)
            {
                Deblog.Log(debug, "Try to place");
                cube.GetComponent<BoxCollider>().enabled = true;
                cube.GetComponent<Rigidbody>().isKinematic = false;
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
                    if (cube != null)
                    {
                        cube.GetComponent<CubeBrickScript>().SetBrickInHands(false);
                        cube.GetComponent<Rigidbody>().AddForce(Vector3.zero);
                        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        cube.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        cube.transform.rotation = Quaternion.identity;
                        cube.transform.position = lowestBrokenFlag.transform.position;
                    }
                }
                Business = businessFind;
            }
        }
    }

    private GameObject FindMinDist(GameObject[] array)
    {
        GameObject gObj = null;
        float dist = float.MaxValue;
        foreach (var item in array)
        {
            if (dist > (transform.position - item.transform.position).magnitude)
            {
                dist = (transform.position - item.transform.position).magnitude;
                gObj = item;
            }
        }
        return gObj;
    }
    private GameObject GetTargetBrick()
    {
        return targetBrick;
    }

    private void SetTargetBrick(GameObject game)
    {
        targetBrick = game;
    }

    private void ControlDist(GameObject gObj1, GameObject gObj2, float dist)
    {
        if ((gObj1.transform.position - gObj2.transform.position).magnitude > dist)
        {
            gObj1.transform.position = Vector3.MoveTowards(gObj1.transform.position, gObj2.transform.position, agent.speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BuilderBuildings")
        {
            if (Business == businessGoToBrokenBrick && !GameController.isGunsShowed)
            {
                Business = businessBringBrokenBrick;
                Deblog.Log(debug, $"{gameObject.name} Entered to house (Enter)");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        TriggerBuilderSite(other);
        if (other.tag == "BuilderBuildings")
        {
            if (Business == businessGoToBrokenBrick && !GameController.isGunsShowed)
            {
                Business = businessBringBrokenBrick;
                Deblog.Log(debug, $"{gameObject.name} Entered to house (Stay)");
            }
        }
    }

    
}

