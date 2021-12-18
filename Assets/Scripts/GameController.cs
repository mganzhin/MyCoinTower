using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponType
{
    Single,
    Triple
}


public class GameController : MonoBehaviour
{

    private readonly float builderSiteFloor = 2.5f;
    public Text ScoreText;
    private WeaponType weapon;
    
    [SerializeField] private readonly List<GameObject> towerList = new List<GameObject>();
    [SerializeField] private GameObject TowerPrefab;
    private float time;
    [SerializeField] private readonly List<GameObject> templateList = new List<GameObject>();
    [SerializeField] private GameObject TemplateFlag;

    [SerializeField] private readonly List<GameObject> ballList = new List<GameObject>();
    [SerializeField] private GameObject BallPrefab;

    [SerializeField] private readonly List<GameObject> builderList = new List<GameObject>();
    [SerializeField] private GameObject BuilderPrefab;

    // Start is called before the first frame update
    void Start()
    {
        weapon = WeaponType.Single;
        FindObjectOfType<CoinScript>().CoinDownEvent += OnCoinDown;
        FindObjectOfType<GlassScript>().GlassPressedEvent += OnGlassPressed;
        RestartScene();
    }

    void RestartScene()
    {
        ScoreText.text = "Find a Coin under tower!";
        MakeTower();
        time = 0;
        MakeBalls();
        MakeBuilders();
    }

    public List<GameObject> GetBrickList()
    {
        return towerList;
    }

    public List<GameObject> GetFlagList()
    {
        return templateList;
    }

    void OnGlassPressed(GlassScript glassScript, Vector3 glassVector)
    {
        int i = 0;
        GameObject ball = null;
        Vector3 ballPosition = MainCameraScript.cameraPosition;
        ballPosition.y -= 1;
        switch (weapon)
        {
            case WeaponType.Single:
                while ((i < ballList.Count) && (ball == null))
                {
                    if (!ballList[i].activeSelf)
                    {
                        ball = ballList[i];
                    }
                    i++;
                }
                if (ball != null)
                {
                    ball.transform.position = ballPosition;
                    ball.SetActive(true);
                    Rigidbody ballRigid = ball.GetComponent<Rigidbody>();
                    ballRigid.AddForce(5 * ballRigid.mass * (glassVector - ball.transform.position), ForceMode.Impulse);
                }
                break;
            case WeaponType.Triple:
                bool isNotAll = false;
                foreach (var item in ballList)
                {
                    if (item.activeSelf)
                        isNotAll = true;
                }
                if (!isNotAll)
                {
                    Vector3 tempPos;
                    for (int ind = 0; ind < ballList.Count; ind++)
                    {
                        tempPos = new Vector3(ballPosition.x + Mathf.Sin(time * 0.05f * ind), ballPosition.y + ind % 2, ballPosition.z + Mathf.Cos(time * 0.05f * ind));
                        ballList[ind].transform.position = tempPos;
                        ballList[ind].SetActive(true);
                        Rigidbody ballRigid = ballList[ind].GetComponent<Rigidbody>();
                        ballRigid.AddForce(5 * ballRigid.mass * (glassVector - ballPosition), ForceMode.Impulse);
                    }
                }
                break;
        }
        
    }

    void ClearObjectList(List<GameObject> objectList)
    {
        for (int i = objectList.Count - 1; i >= 0; i--)
        {
            Destroy(objectList[i]);
            objectList.RemoveAt(i);
        }
    }

    void MakeBuilders()
    {
        ClearObjectList(builderList);
        float angle;
        for (int i = 0; i < 3; i++)
        {
            angle = Random.Range(0f, 14f);
            builderList.Add(Instantiate(BuilderPrefab,
                new Vector3(21 * Mathf.Cos(angle), 3f, 21 * Mathf.Sin(angle)),
                Quaternion.identity));
        }
    }

    void MakeBalls()
    {
        ClearObjectList(ballList);
        for (int i = 0; i < 3; i++)
        {
            ballList.Add(Instantiate(BallPrefab,
                new Vector3(0, 58, 0),
                Quaternion.identity));
            ballList[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weapon = WeaponType.Triple;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            weapon = WeaponType.Single;
        }
        time += Time.deltaTime;
        /*if (time > 1)
        {
            time = 0;
            //Find broken tower wall
            GameObject lowestBrokenFlag = null;
            float y = 100;
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
                //Find nearest broken brick
                GameObject nearestBrokenBrick = null;
                float distance = 100;
                for (int i = 0; i < towerList.Count; i++)
                {
                    if (!towerList[i].GetComponent<CubeBrickScript>().IsBrickInPlace())
                    {
                        if ((towerList[i].gameObject.transform.position - lowestBrokenFlag.transform.position).magnitude < distance)
                        {
                            nearestBrokenBrick = towerList[i];
                            distance = (towerList[i].gameObject.transform.position - lowestBrokenFlag.transform.position).magnitude;
                        }
                    }
                }
                if (nearestBrokenBrick != null)
                {
                    //Place brick to flag;
                    nearestBrokenBrick.GetComponent<Rigidbody>().AddForce(Vector3.zero);
                    nearestBrokenBrick.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    nearestBrokenBrick.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    nearestBrokenBrick.transform.rotation = Quaternion.identity;
                    nearestBrokenBrick.transform.position = lowestBrokenFlag.transform.position;
                }
            }
        }*/
    }

    void MakeTower()
    {
        ClearObjectList(towerList);
        ClearObjectList(templateList);
        //Start tower and flags for build
        for (int y = 0; y < 3; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if ((x != 0) || (z != 0))
                    {
                        templateList.Add(Instantiate(TemplateFlag,
                            new Vector3(x * 2f, builderSiteFloor + y * 2f, z * 2f),
                            Quaternion.Euler(0, 0, 0)));
                        towerList.Add(Instantiate(TowerPrefab,
                            new Vector3(x * 2f, builderSiteFloor + y * 2f, z * 2f),
                            Quaternion.Euler(0, 0, 0)));
                    }
                    else
                    {
                        if (y == 0)
                        {
                            templateList.Add(Instantiate(TemplateFlag,
                                new Vector3(x * 2f, builderSiteFloor + y * 2f, z * 2f),
                                Quaternion.Euler(0, 0, 0)));
                            towerList.Add(Instantiate(TowerPrefab,
                                new Vector3(x * 2f, builderSiteFloor + y * 2f, z * 2f),
                                Quaternion.Euler(0, 0, 0)));
                        }
                    }
                }
            }
        }
        //flags to build high floors
        for (int y = 3; y < 8; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if ((x != 0) || (z != 0))
                    {
                        templateList.Add(Instantiate(TemplateFlag,
                            new Vector3(x * 2f, builderSiteFloor + y * 2f, z * 2f),
                            Quaternion.Euler(0, 0, 0)));
                    }
                }
            }
        }
        //Place other bricks
        float angle;
        for (int i = towerList.Count; i < 28; i++)
        {
            angle = Random.Range(0f, 14f);
            towerList.Add(Instantiate(TowerPrefab,
                            new Vector3(17*Mathf.Cos(angle), Random.Range(3, 15), 17 * Mathf.Sin(angle)),
                            Quaternion.Euler(0, 0, 0)));
        }
        for (int i = 0; i< towerList.Count; i++)
        {
            towerList[i].GetComponent<CubeBrickScript>().SetBrickInHands(false);
        }
    }

    void OnCoinDown(CoinScript coinScript)
    {
        RestartScene();
    }
}
