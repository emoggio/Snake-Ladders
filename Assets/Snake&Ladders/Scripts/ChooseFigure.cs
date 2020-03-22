using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFigure : MonoBehaviour
{
    //gameobjects
    private GameObject PlayerOne;
    public GameObject[] FiguresOne;
    private GameObject FigureOne;

    private GameObject PlayerTwo;
    private GameObject FigureTwo;

    private int x=3;

    //private GameObject Manager;
    //MovingManager movingManager;

    private void Awake()
    {
        PlayerOne = GameObject.FindGameObjectWithTag("Player");
        PlayerTwo = GameObject.FindGameObjectWithTag("cpu");

        FiguresOne = GameObject.FindGameObjectsWithTag("figure");

        if (PlayerOne != null)
        {
            for (int i = 0; i < PlayerOne.transform.childCount; i++)
            {
                if (PlayerOne.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    FigureOne = PlayerOne.transform.GetChild(i).gameObject;
                    FigureOne.SetActive(false);
                }
            }
        }

        if (PlayerTwo != null)
        {
            for (int i = 0; i < PlayerTwo.transform.childCount; i++)
            {
                if (PlayerTwo.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    FigureTwo = PlayerTwo.transform.GetChild(i).gameObject;
                    FigureTwo.SetActive(false);
                }
            }
        }

        Invoke("FigureOneChosen", 0);
    }

    public void FigureOneChosen()
    {
        Debug.Log("Invoked");

        for (int i = 0; i < PlayerOne.transform.childCount; i++)
        {
            Debug.Log("test");

            //if (PlayerOne.transform.GetChild(i).gameObject.activeSelf == true)
            //{
                FigureOne = PlayerOne.transform.GetChild(x).gameObject;
                //FigureOne.SetActive(false);

                Debug.Log(FigureOne.name);
           // }
        }

        //FigureOne = PlayerOne.transform.GetChild(x).gameObject;
        //FigureOne.SetActive(true);
    }

}

