using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{

    public Text ScoreText;
    
    [SerializeField] private readonly List<GameObject> towerList = new List<GameObject>();
    [SerializeField] private GameObject TowerPrefab;
    private float time;
    [SerializeField] private readonly List<GameObject> templateList = new List<GameObject>();
    [SerializeField] private GameObject TemplateFlag;

    [SerializeField] private readonly List<GameObject> ballList = new List<GameObject>();
    [SerializeField] private GameObject BallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CoinScript>().CoinDownEvent += OnCoinDown;
        FindObjectOfType<GlassScript>().GlassPressedEvent += OnGlassPressed;
        RestartScene();
    }

    void RestartScene()
    {
        MakeTower();
        time = 0;
        MakeBalls();
    }

    void OnGlassPressed(GlassScript glassScript, Vector3 glassVector)
    {
        int i = 0;
        GameObject ball = null;
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
            Debug.Log("ball not null");
            Vector3 ballPosition = MainCameraScript.cameraPosition;
            ballPosition.y -= 1;
            ball.transform.position = ballPosition;
            ball.SetActive(true);
            Rigidbody ballRigid = ball.GetComponent<Rigidbody>();
            ballRigid.AddForce(5 * (glassVector - ball.transform.position), ForceMode.Impulse);
        }
        else
        {
            Debug.Log("ball null");
        }
    }

    void MakeBalls()
    {
        ballList.Clear();
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
        ScoreText.text = "Find a Coin under tower!";
        time += Time.deltaTime;
        if (time > 1)
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
        }
    }

    void MakeTower()
    {
        for (int i = towerList.Count - 1; i >= 0; i--)
        {
            Destroy(towerList[i]);
        }
        templateList.Clear();
        towerList.Clear();
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
                            new Vector3(x * 2f, 2f + y * 2f, z * 2f),
                            Quaternion.Euler(0, 0, 0)));
                        towerList.Add(Instantiate(TowerPrefab,
                            new Vector3(x * 2f, 2f + y * 2f, z * 2f),
                            Quaternion.Euler(0, 0, 0)));
                    }
                    else
                    {
                        if (y == 0)
                        {
                            templateList.Add(Instantiate(TemplateFlag,
                                new Vector3(x * 2f, 3f + y * 2f, z * 2f),
                                Quaternion.Euler(0, 0, 0)));
                            towerList.Add(Instantiate(TowerPrefab,
                                new Vector3(x * 2f, 3f + y * 2f, z * 2f),
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
                            new Vector3(x * 2f, 2f + y * 2f, z * 2f),
                            Quaternion.Euler(0, 0, 0)));
                    }
                }
            }
        }
        //Place other bricks
        float angle;
        for (int i = towerList.Count; i < 50; i++)
        {
            angle = Random.Range(0f, 14f);
            towerList.Add(Instantiate(TowerPrefab,
                            new Vector3(17*Mathf.Cos(angle), Random.Range(3, 15), 17 * Mathf.Sin(angle)),
                            Quaternion.Euler(0, 0, 0)));
        }
    }

    void OnCoinDown(CoinScript coinScript)
    {
        RestartScene();
    }
}
