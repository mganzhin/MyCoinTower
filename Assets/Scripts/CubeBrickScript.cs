using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBrickScript : MonoBehaviour
{
    [SerializeField] private Material materialBroken;
    [SerializeField] private Material materialInPlace;
    private Renderer brickRenderer;
    private bool isBrickInPlace;
    private bool isBrickInHands;

    // Start is called before the first frame update
    void Start()
    {
         brickRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)
        {
            float angle = Random.Range(0f, 14f);
            transform.position = new Vector3(17 * Mathf.Cos(angle), Random.Range(3, 15), 17 * Mathf.Sin(angle));
        }
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
            if (brickRenderer != null)
            {
                if (brickRenderer.material != materialInPlace)
                {
                    brickRenderer.material = materialInPlace;
                    isBrickInPlace = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "TemplateFlagTag")
        {
            if (brickRenderer != null)
            {
                brickRenderer.material = materialBroken;
                isBrickInPlace = false;
                GameController.countBrokenBricks++;
            }
        }
    }

}
