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
    public int LastPlayerRoll = 0;
    public int LastCpuRoll = 0;
    private int i;

    //turns
    public bool myTurn = true;

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
        diceRoll = (Random.Range(1, 6));
        
        if (myTurn)
            StartCoroutine(MovePlayerOne());
        else if (!myTurn)
            StartCoroutine(MovePlayerTwo());
    }

    //player
    IEnumerator MovePlayerOne()
    {

        yield return new WaitForSeconds(1);

        if (LastPlayerRoll < Tiles.Length)
        {
            for (i = 0; i <= diceRoll; i++)
            {
                //amount  by which the player is raised on move
                if (Tiles[i + LastPlayerRoll].transform.position.y > Tiles[LastPlayerRoll].transform.position.y)
                {
                    JumpHeight = Jump + (Tiles[i + LastPlayerRoll].transform.position.y- Tiles[LastPlayerRoll].transform.position.y);
                }   
                else if (Tiles[i + LastPlayerRoll].transform.position.y <= Tiles[LastPlayerRoll].transform.position.y) 
                {
                    JumpHeight = Jump;
                }  

                //move and lift the player one tile at the time
                if (i!=0)
                {
                    Sequence Move = DOTween.Sequence();
                    Move.PrependInterval(animationTime / 10)
                        .Append(PlayerOne.transform.DOMoveX(Tiles[i + LastPlayerRoll].transform.position.x, animationTime).SetEase(Ease.InOutCubic))
                        .Join(PlayerOne.transform.DOMoveZ(Tiles[i + LastPlayerRoll].transform.position.z, animationTime).SetEase(Ease.InOutCubic));

                    Sequence Jump = DOTween.Sequence();
                    Jump.PrependInterval(animationTime / 10)
                        .Append(PlayerOne.transform.DOMoveY(PlayerOne.transform.position.y + JumpHeight, animationTime / 2).SetEase(Ease.InOutCubic))
                        .Append(PlayerOne.transform.DOMoveY(Tiles[i + LastPlayerRoll].transform.position.y, animationTime / 2).SetEase(Ease.InOutCubic));

                    yield return new WaitForSeconds(animationTime+0.5f);
                }
            }

            //pick up from your last tile
            LastPlayerRoll = LastPlayerRoll + diceRoll;

            //swich turns
            myTurn = !myTurn;
            Invoke("DiceRoll", 1);
        }
        else if (LastCpuRoll >= Tiles.Length)
        {
            Debug.Log("you Win");
        }
    }

    //cpu
    IEnumerator MovePlayerTwo()
    {
        yield return new WaitForSeconds(1);

        if (LastCpuRoll < Tiles.Length)
        {
            for (i = 0; i <= diceRoll; i++)
            {
                //amount  by which the player is raised on move
                if (Tiles[i + LastCpuRoll].transform.position.y > Tiles[LastCpuRoll].transform.position.y)
                {
                    JumpHeight = Jump + (Tiles[i + LastCpuRoll].transform.position.y - Tiles[LastCpuRoll].transform.position.y);
                }
                else if (Tiles[i + LastCpuRoll].transform.position.y <= Tiles[LastCpuRoll].transform.position.y)
                {
                    JumpHeight = Jump;
                }

                //move and lift the player one tile at the time
                if (i != 0)
                {
                    Sequence Move = DOTween.Sequence();
                    Move.PrependInterval(animationTime / 10)
                        .Append(PlayerTwo.transform.DOMoveX(Tiles[i + LastCpuRoll].transform.position.x, animationTime).SetEase(Ease.InOutCubic))
                        .Join(PlayerTwo.transform.DOMoveZ(Tiles[i + LastCpuRoll].transform.position.z, animationTime).SetEase(Ease.InOutCubic));

                    Sequence Jump = DOTween.Sequence();
                    Jump.PrependInterval(animationTime / 10)
                        .Append(PlayerTwo.transform.DOMoveY(PlayerTwo.transform.position.y + JumpHeight, animationTime / 2).SetEase(Ease.InOutCubic))
                        .Append(PlayerTwo.transform.DOMoveY(Tiles[i + LastCpuRoll].transform.position.y, animationTime / 2).SetEase(Ease.InOutCubic));

                    yield return new WaitForSeconds(animationTime + 0.5f);
                }
            }

            //pick up from your last tile
            LastCpuRoll = LastCpuRoll + diceRoll;

            myTurn = !myTurn;
        }
        else if (LastCpuRoll >= Tiles.Length)
        {
            Debug.Log("you Lose");
        }
    }
}
