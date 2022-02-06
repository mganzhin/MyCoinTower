using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShitButtonBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject button;
    public static bool isPressed;
    private float time, cooldown = 10;

    private void Start()
    {
        time = 0;
        isPressed = false;
    }
    private void Update()
    {
        if (isPressed)
        {
            time += Time.deltaTime;
            if (time > cooldown)
            {
                time = 0;
                isPressed = false;
            }
        }
        if (GameController.countBrokenBricks > 20 && !button.activeSelf)
        {
            button.SetActive(true);
        }
        else if (GameController.countBrokenBricks <= 20 && button.activeSelf)
        {
            button.SetActive(false);
        }
    }

    public void LetItShit()
    {
        isPressed = true;
        GameController.countBrokenBricks = 0;
        button.SetActive(false);
    }




}
