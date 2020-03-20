using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MovingManager : MonoBehaviour
{
    //gameobjects
    public GameObject[] Tiles;
    private GameObject PlayerOne;
    private GameObject PlayerTwo;

    //Variables
    //dices
    public int diceRoll;
    public int LastRoll = 0;
    private int i;

    //players
    public float JumpHeight;
    [Range(0.1f, 10)]
    private float Jump=0.25f;

    //AnimationSpeed
    [Range(0.1f, 10)]
    public float animationTime;
    

    private void Awake()
    {
        //find all tiles and order them alphabetically
        Tiles = GameObject.FindGameObjectsWithTag("tile").OrderBy(go => go.name).ToArray();
        if (GameObject.FindGameObjectsWithTag("tile").Length == 0)
            Debug.LogError(Tiles + " " + "were not found");

        //find player & cpu
        PlayerOne = GameObject.FindGameObjectWithTag("Player");
        PlayerTwo = GameObject.FindGameObjectWithTag("cpu");

        //move the players to the start
        if (PlayerOne != null)
            PlayerOne.transform.DOMove(Tiles[0].transform.position, 0);
        else
            return;

        if (PlayerTwo != null)
            PlayerTwo.transform.DOMove(Tiles[0].transform.position, 0);
        else
            return;
    }

    //roll the dice
    public void DiceRoll()
    {
        diceRoll = (Random.Range(1, 5));
       // Height = ((Tiles[0].transform.position - Tiles[1].transform.position)/3).magnitude;
        StartCoroutine(MovePlayerOne());
    }

    IEnumerator MovePlayerOne()
    {
        for (i = 0; i <= diceRoll; i++)
        {
            //amount  by which the player is raised on move
            if (Tiles[i + LastRoll].transform.position.y > Tiles[LastRoll].transform.position.y)
            {
                JumpHeight = Jump + (Tiles[i + LastRoll].transform.position.y- Tiles[LastRoll].transform.position.y);
                Debug.Log("tes1");
            }   
            else if (Tiles[i + LastRoll].transform.position.y <= Tiles[LastRoll].transform.position.y) 
            {
                JumpHeight = Jump;
                Debug.Log("tes2");
            }  

            //move and lift the player one tile at the time
            if (i!=0)
            {
                Sequence Move = DOTween.Sequence();
                Move.PrependInterval(animationTime / 10)
                    .Append(PlayerOne.transform.DOMoveX(Tiles[i + LastRoll].transform.position.x, animationTime).SetEase(Ease.InOutCubic))
                    .Join(PlayerOne.transform.DOMoveZ(Tiles[i + LastRoll].transform.position.z, animationTime).SetEase(Ease.InOutCubic));

                Sequence Jump = DOTween.Sequence();
                Jump.PrependInterval(animationTime / 10)
                    .Append(PlayerOne.transform.DOMoveY(PlayerOne.transform.position.y + JumpHeight, animationTime / 2).SetEase(Ease.InOutCubic))
                    .Append(PlayerOne.transform.DOMoveY(Tiles[i + LastRoll].transform.position.y, animationTime / 2).SetEase(Ease.InOutCubic));

                yield return new WaitForSeconds(animationTime+0.5f);
            }
        }

        //pick up from your last tile
        LastRoll = LastRoll+ diceRoll;
    }
}
