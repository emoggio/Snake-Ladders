using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MovingManager : MonoBehaviour
{
    public GameObject[] Tiles;
    public GameObject PlayerOne;
    public GameObject PlayerTwo;

    private void Awake()
    {
        //find all tiles and order them alphabetically
        Tiles = GameObject.FindGameObjectsWithTag("tile").OrderBy(go => go.name).ToArray();

        //player
        PlayerOne = GameObject.FindGameObjectWithTag("Player");
        PlayerTwo = GameObject.FindGameObjectWithTag("cpu");

        //move the players to the start
        PlayerOne.transform.DOMove(Tiles[1].transform.position, 0);
        PlayerTwo.transform.DOMove(Tiles[2].transform.position, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
