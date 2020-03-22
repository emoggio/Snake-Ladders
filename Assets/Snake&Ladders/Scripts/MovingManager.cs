using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MovingManager : MonoBehaviour
{
    //gameobjects
    private GameObject[] Tiles;
    private GameObject PlayerOne;
    private GameObject PlayerTwo;

    private GameObject Manager;
    PlayerAnimation playerAnimation;
    UIManager uiManager;
    SwipeEventManager swipeEventManager;

    //Variables
    //dices
    public int diceRoll;
    public int LastPlayerRoll = 0;
    public int LastCpuRoll = 0;
    private int i;

    //snakes and ladders jumps
    private int Ladder = 5;
    private int Snake = 7;
    
    //tile rotation
    private Vector3 Firstquarter = new Vector3(90, 0, 0);

    //turns
    public bool myTurn = true;

    //players
    private float JumpHeight;
    [Range(0.1f, 10)]
    private float Jump=0.25f;

    //AnimationSpeed
    [Range(0.1f, 10)]
    public float animationTime;
    

    private void Awake()
    {
        //check that the manager exists
        Manager = GameObject.FindGameObjectWithTag("manager");
        if (Manager != null)
        {
            playerAnimation = Manager.GetComponent<PlayerAnimation>();
            uiManager = Manager.GetComponent<UIManager>();
            swipeEventManager = Manager.GetComponent<SwipeEventManager>();
        }

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

    private void Update()
    {
        if (myTurn)
            swipeEventManager.enabled = true;
        else if(!myTurn)
            swipeEventManager.enabled = false;
    }

    //resetThePlayers
    public void ResetPosition()
    {
        //move the players to the start
        if (PlayerOne != null)
            PlayerOne.transform.DOMove(Tiles[0].transform.position, 0);
        else
            return;

        if (PlayerTwo != null)
            PlayerTwo.transform.DOMove(Tiles[0].transform.position, 0);
        else
            return;

        LastCpuRoll = 0;
        LastPlayerRoll = 0;

        uiManager.YouLose.SetActive(false);
        uiManager.YouWin.SetActive(false);
    }

    //roll the dice
    public void DiceRoll()
    {
        diceRoll = (Random.Range(1, 6));

        if (myTurn)
            uiManager.UpdateDiceRollPlayer();
        else if (!myTurn)
            uiManager.UpdateDiceRollCpu();
    }

    //moveThePlayers
    public void moveThePlayers()
    {
        if (myTurn)
            StartCoroutine(MovePlayerOne());
        else if (!myTurn)
            StartCoroutine(MovePlayerTwo());
    }

    //player
    IEnumerator MovePlayerOne()
    {
        yield return new WaitForSeconds(.5f);

        if (LastPlayerRoll + diceRoll < Tiles.Length)
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

            //if I land on a specific ladder tile move me up
            //if I land on a specific snake tile move me down
            if (LastPlayerRoll == 3 || LastPlayerRoll == 11 || LastPlayerRoll == 22)
            {
                //Vector3 Firstquarter = new Vector3(90, 0, 0);

                //flip the tile I am currently on and move me to the new position
                Sequence FlipAndMoveStart = DOTween.Sequence();
                FlipAndMoveStart.PrependInterval(animationTime/10)
                    .Append(Tiles[LastPlayerRoll].transform.DORotate((Firstquarter * -1), animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Join(PlayerOne.transform.DORotate(Firstquarter * -1, animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Append(PlayerOne.transform.DOMove(Tiles[LastPlayerRoll + Ladder].transform.position, 0))
                    .Append(Tiles[LastPlayerRoll].transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));

                //simultaneously flip the tile where I should land on
                Sequence FlipAndMoveEnd = DOTween.Sequence();
                FlipAndMoveEnd.PrependInterval(animationTime / 10)
                    .Append(Tiles[LastPlayerRoll+Ladder].transform.DORotate((Firstquarter * -1), animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Append(PlayerOne.transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic))
                    .Join(Tiles[LastPlayerRoll+Ladder].transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));

                //update the current position
                LastPlayerRoll = LastPlayerRoll + Ladder;
            }
            else if(LastPlayerRoll == 13 || LastPlayerRoll == 17 || LastPlayerRoll == 32)
            {
                //flip the tile I am currently on and move me to the new position
                Sequence FlipAndMoveStart = DOTween.Sequence();
                FlipAndMoveStart.PrependInterval(animationTime / 10)
                    .Append(Tiles[LastPlayerRoll].transform.DORotate((Firstquarter * -1), animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Join(PlayerOne.transform.DORotate(Firstquarter * -1, animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Append(PlayerOne.transform.DOMove(Tiles[LastPlayerRoll - Snake].transform.position, 0))
                    .Append(Tiles[LastPlayerRoll].transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));

                //simultaneously flip the tile where I should land on
                Sequence FlipAndMoveEnd = DOTween.Sequence();
                FlipAndMoveEnd.PrependInterval(animationTime / 10)
                    .Append(Tiles[LastPlayerRoll - Snake].transform.DORotate((Firstquarter * -1), animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Append(PlayerOne.transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic))
                    .Join(Tiles[LastPlayerRoll - Snake].transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));

                //update the current position
                LastPlayerRoll = LastPlayerRoll - Snake;
            }

            //swich turns
            myTurn = !myTurn;

            //make the other player excited
            playerAnimation.Excitement();
        }
        else if (LastPlayerRoll + diceRoll >= Tiles.Length)
        {
            //for (i = 0; i <= diceRoll; i++)
            diceRoll = Mathf.Abs(Tiles.Length - (LastPlayerRoll + diceRoll));
            for (i = 0; i <= diceRoll; i++)
            {
                //amount  by which the player is raised on move
                if (Tiles[i + LastPlayerRoll].transform.position.y > Tiles[LastPlayerRoll].transform.position.y)
                {
                    JumpHeight = Jump + (Tiles[i + LastPlayerRoll].transform.position.y - Tiles[LastPlayerRoll].transform.position.y);
                }
                else if (Tiles[i + LastPlayerRoll].transform.position.y <= Tiles[LastPlayerRoll].transform.position.y)
                {
                    JumpHeight = Jump;
                }

                //move and lift the player one tile at the time
                if (i != 0)
                {
                    Sequence Move = DOTween.Sequence();
                    Move.PrependInterval(animationTime / 10)
                        .Append(PlayerOne.transform.DOMoveX(Tiles[i + LastPlayerRoll].transform.position.x, animationTime).SetEase(Ease.InOutCubic))
                        .Join(PlayerOne.transform.DOMoveZ(Tiles[i + LastPlayerRoll].transform.position.z, animationTime).SetEase(Ease.InOutCubic));

                    Sequence Jump = DOTween.Sequence();
                    Jump.PrependInterval(animationTime / 10)
                        .Append(PlayerOne.transform.DOMoveY(PlayerOne.transform.position.y + JumpHeight, animationTime / 2).SetEase(Ease.InOutCubic))
                        .Append(PlayerOne.transform.DOMoveY(Tiles[i + LastPlayerRoll].transform.position.y, animationTime / 2).SetEase(Ease.InOutCubic));

                    yield return new WaitForSeconds(animationTime + 0.5f);
                }
            }

            uiManager.Win();
            Debug.Log("you Win");
        }    
    }

    //cpu
    IEnumerator MovePlayerTwo()
    {
        yield return new WaitForSeconds(.5f);

        if (LastCpuRoll + diceRoll < Tiles.Length)
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

            //if I land on a specific ladder tile move me up
            if (LastCpuRoll == 3 || LastCpuRoll == 11 || LastCpuRoll == 22)
            {
                //flip the tile I am currently on and move me to the new position
                Sequence FlipAndMoveStart = DOTween.Sequence();
                FlipAndMoveStart.PrependInterval(animationTime / 10)
                    .Append(Tiles[LastCpuRoll].transform.DORotate((Firstquarter * -1), animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Join(PlayerTwo.transform.DORotate(Firstquarter * -1, animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Append(PlayerTwo.transform.DOMove(Tiles[LastCpuRoll + Ladder].transform.position, 0))
                    .Append(Tiles[LastCpuRoll].transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));

                //simultaneously flip the tile where I should land on
                Sequence FlipAndMoveEnd = DOTween.Sequence();
                FlipAndMoveEnd.PrependInterval(animationTime / 10)
                    .Append(Tiles[LastCpuRoll + Ladder].transform.DORotate((Firstquarter * -1), animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Append(PlayerTwo.transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic))
                    .Join(Tiles[LastCpuRoll + Ladder].transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));

                //update the current position
                LastCpuRoll = LastCpuRoll + Ladder;
            }
            else if (LastCpuRoll == 13 || LastCpuRoll == 17 || LastCpuRoll == 32)
            {
                //flip the tile I am currently on and move me to the new position
                Sequence FlipAndMoveStart = DOTween.Sequence();
                FlipAndMoveStart.PrependInterval(animationTime / 10)
                    .Append(Tiles[LastCpuRoll].transform.DORotate((Firstquarter * -1), animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Join(PlayerTwo.transform.DORotate(Firstquarter * -1, animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Append(PlayerTwo.transform.DOMove(Tiles[LastCpuRoll - Snake].transform.position, 0))
                    .Append(Tiles[LastCpuRoll].transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));

                //simultaneously flip the tile where I should land on
                Sequence FlipAndMoveEnd = DOTween.Sequence();
                FlipAndMoveEnd.PrependInterval(animationTime / 10)
                    .Append(Tiles[LastCpuRoll - Snake].transform.DORotate((Firstquarter * -1), animationTime, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic))
                    .Append(PlayerTwo.transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic))
                    .Join(Tiles[LastCpuRoll - Snake].transform.DORotate((Firstquarter * 3) * -1, animationTime * 3, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));

                //update the current position
                LastCpuRoll = LastCpuRoll - Snake;
            }

            //switch turns
            myTurn = !myTurn;
        }
        else if (LastCpuRoll + diceRoll >= Tiles.Length)
        {
            diceRoll = Mathf.Abs(Tiles.Length - (LastCpuRoll + diceRoll));
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

            uiManager.Lose();
            Debug.Log("you Lose");
        }
    }
}
