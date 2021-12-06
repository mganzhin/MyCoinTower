using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{

    public Text ScoreText;
    
    public List<GameObject> towerList = new List<GameObject>();
    public GameObject TowerPrefab;
    float time;
    public GameObject Floor;


    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CoinScript>().CoinDownEvent += OnCoinDown;
        MakeTower();
        time = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = "Find a Coin! "+ (50 - towerList.Count).ToString()+" bricks to lose!";
        time += Time.deltaTime;
        if (time > 3)
        {
            time = 0;
            if (towerList.Count >= 50)
            {
                MakeTower();
            }
            else
            {
                if (towerList.Count > 0)
                {
                    towerList.Add(Instantiate(TowerPrefab,
                                    new Vector3(Random.Range(-5f, 5f), 20f, Random.Range(-5f, 5f)),
                                    Quaternion.Euler(0, 0, 0)));
                }
            }
        }
        //Floor.transform.Rotate(new Vector3(0, 10 * Time.deltaTime, 0));
    }

    void MakeTower()
    {
        for (int i = towerList.Count - 1; i >= 0 ; i--)
        {
            Destroy(towerList[i]);
        }
        towerList.Clear();
        for (int y = 0; y < 3; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    towerList.Add(Instantiate(TowerPrefab,
                        new Vector3(x * 2.1f, 2.15f + y * 4.4f, z * 2.1f),
                        Quaternion.Euler(0, 0, 0)));
                }
            }
        }
    }

    void OnCoinDown(CoinScript coinScript)
    {
        MakeTower();
    }
}
