using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBrickScript : MonoBehaviour
{
    private const int maxHitPoints = 100;
    private const int fireType = 0; //red
    private const int ecoType = 1; //green
    private const int waterType = 2; //blue
    private const int maxTypes = 3; //count of types
    private static int[] brickTypes = { fireType, ecoType, waterType };
    [SerializeField] private Material[] materialDamaged;
    [SerializeField] private Material materialBroken;
    [SerializeField] private Material[] materialInPlace;
    private Renderer brickRenderer;
    private bool isBrickInPlace;
    private bool isBrickInHands;
    private int hitPoints;
    private int brickType;
    private float deadTime;

    // Start is called before the first frame update
    void Start()
    {
        brickRenderer = GetComponent<Renderer>();
        hitPoints = maxHitPoints;
        brickType = Random.Range(0, maxTypes);
        SetMaterial(CheckMaterial());
        deadTime = 0;
    }

    private void SetMaterial(Material material)
    {
        if (brickRenderer != null)
        {
            if (brickRenderer.material != material)
            {
                brickRenderer.material = material;
            }
        }
    }

    private Material CheckMaterial()
    {
        if (isBrickInPlace)
        {
            if (hitPoints > 80)
            {
                return materialInPlace[brickType];
            }
            else
            {
                if (hitPoints == 0)
                {
                    return materialBroken;
                }
                else
                {
                    return materialDamaged[brickType];
                }
            }
        }
        else
        {
            return materialBroken;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if brick is falling down
        if (transform.position.y < -5)
        {
            float angle = Random.Range(0f, 14f);
            transform.position = new Vector3(17 * Mathf.Cos(angle), Random.Range(3, 15), 17 * Mathf.Sin(angle));
        }

        if (hitPoints == 0)
        {
            deadTime += Time.deltaTime;
            if (deadTime > 5)
            {
                //Destroy?
            }
        }
    }

    public void SetDeltaHitPoints(int deltaHitPoints)
    {
        if (hitPoints > 0)
        {
            hitPoints = Mathf.Max(0, Mathf.Min(maxHitPoints, hitPoints + deltaHitPoints));
        }
    }

    public int GetHitPoints()
    {
        return hitPoints;
    }

    public bool IsBrickInPlace()
    {
        return isBrickInPlace;
    }

    public bool IsBrickInHands()
    {
        return isBrickInHands;
    }

    public void SetBrickInHands(bool inHands)
    {
        isBrickInHands = inHands;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "TemplateFlagTag")
        {
            isBrickInPlace = true;
            SetMaterial(CheckMaterial());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("TriggerExit!");
        if (other.gameObject.tag == "TemplateFlagTag")
        {
            isBrickInPlace = false;
            SetDeltaHitPoints(-maxHitPoints);
            GameController.countBrokenBricks++;
            SetMaterial(CheckMaterial());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BallTag")
        {
            if (collision.gameObject.GetComponent<BallScript>().GetBulletType() == brickType)
            {
                SetDeltaHitPoints(-5);
            }
            else
            {
                SetDeltaHitPoints(-3);
            }
        }
    }

}
