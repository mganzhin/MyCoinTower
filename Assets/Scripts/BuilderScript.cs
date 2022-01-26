using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuilderScript : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;
    
    private static GameObject builderSite;
    private static GameController gameController;

    private const int businessStay = 0;
    private const int businessGoToBrokenBrick = 1;
    private const int businessBringBrokenBrick = 2;
    private const int businessGoToBuilderSite = 3;
    private const int businessWalking = 4;
    private int Business { get; set; }
    private GameObject[] targetBuilderHouses, placeBrickHeres;
    private GameObject targetBrick, placeBrickHere, targetBuilderHouse;
    private NavMeshAgent agent;
    

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
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 2.5f;
        targetBuilderHouses = GameObject.FindGameObjectsWithTag("BuilderBuildings");
        placeBrickHeres = GameObject.FindGameObjectsWithTag("BrickHere");
        placeBrickHere = FindMinDist(placeBrickHeres);
        targetBuilderHouse = FindMinDist(targetBuilderHouses);
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

        if (ShitButtonBehaviour.isPressed) Business = businessWalking;

        switch (Business)
        {
            case businessStay:
                if (gameController != null)
                {
                    List<CubeBrickScript> towerList = gameController.GetBrickList();
                    List<GameObject> templateList = gameController.GetFlagList();
                    for (int i = 0; i < towerList.Count; i++)
                    {
                        if (!templateList[i].GetComponent<TemplateFlagScript>().IsInPlace()) // есть ли отсутствующие блоки на башне
                        {
                            Business = businessGoToBrokenBrick;
                        } 
                        else
                        {
                            agent.SetDestination(transform.position); // стоим
                        }
                    }
                    
                }
                
                //if (targetBrick != null)
                //{
                //    targetBrick.GetComponent<CubeBrickScript>().SetBrickInHands(false);
                //    targetBrick = null;
                //}
                //float distance = 100;
                //if (gameController != null)
                //{
                //    List<CubeBrickScript> towerList = gameController.GetBrickList();
                //    for (int i = 0; i < towerList.Count; i++)
                //    {
                //        //CubeBrickScript cubeScript = towerList[i].GetComponent<CubeBrickScript>();
                //        if ((!towerList[i].IsBrickInPlace()) && (!towerList[i].IsBrickInHands()))
                //        {
                //            /*
                //             Было : Поиск куба с минимальным расстоянием от строителя
                //             Должно стать: Ходьба до здания и забор куба 
                //             */
                //            if ((towerList[i].gameObject.transform.position - transform.position).magnitude < distance)
                //            {
                //                targetBrick = towerList[i].gameObject;
                //                //targetBrick.GetComponent<Renderer>().material = targetMaterial;
                //                distance = (towerList[i].gameObject.transform.position - transform.position).magnitude;
                //            }
                //        }
                //    }
                //    if (targetBrick != null)
                //    {
                //        Business = businessGoToBrokenBrick;
                //        targetBrick.GetComponent<CubeBrickScript>().SetBrickInHands(true);
                //    }
                //    else
                //    {
                //        Business = businessWalking;
                //    }
                //}
                break;
            case businessGoToBrokenBrick:
                agent.SetDestination(targetBuilderHouse.transform.position);
                break;
            case businessBringBrokenBrick:
                // определяем нужный блок
                bool hasBrick = false;
                List<CubeBrickScript> towerList1 = gameController.GetBrickList();
                for (int i = 0; i < towerList1.Count; i++)
                {
                    if (!towerList1[i].IsBrickInPlace() && !towerList1[i].isActiveAndEnabled)
                    {
                        SetTargetBrick(towerList1[i].gameObject);
                        hasBrick = true;
                    }
                }
                if (GetTargetBrick() != null && hasBrick)
                {
                    GetTargetBrick().SetActive(true);
                    GetTargetBrick().transform.position = placeBrickHere.transform.position;
                    Business = businessGoToBuilderSite;
                }
                else
                {
                    Business = businessStay;
                }
                break;
            case businessGoToBuilderSite:
                agent.SetDestination(builderSite.transform.position);
                GetTargetBrick().transform.position = Vector3.MoveTowards(GetTargetBrick().transform.position, transform.position, 3 * Time.deltaTime);
                
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
            if (Business == businessGoToBuilderSite)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BuilderBuildings")
        {
            if (Business == businessGoToBrokenBrick)
            {
                Business = businessBringBrokenBrick;
                Debug.Log("Entered to house");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("BuilderTrigger: " + other.gameObject.name);
        TriggerBuilderSite(other);
        
    }

    
}

