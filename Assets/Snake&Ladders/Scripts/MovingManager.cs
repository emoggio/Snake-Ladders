using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MovingManager : MonoBehaviour
{
    //gameobjects
    public GameObject[] Tiles;
    public GameObject PlayerOne;
    public GameObject PlayerTwo;

    //Variables
    public int diceRoll;

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
        diceRoll = (Random.Range(1, 2));
        //Invoke("MovePlayerOne", 0.1f);

        Sequence MoveAndJump = DOTween.Sequence();
        MoveAndJump.Join(PlayerOne.transform.DOMoveX(Tiles[1].transform.position.x, animationTime).SetEase(Ease.InOutCubic))
            .Join(PlayerOne.transform.DOMoveZ(Tiles[1].transform.position.z, animationTime).SetEase(Ease.InOutCubic))
            .Join(PlayerOne.transform.DOMoveY(1, animationTime/2).SetEase(Ease.InOutCubic).SetLoops(2, LoopType.Yoyo));
            //.AppendInterval(animationTime / 2)
           //.Join(PlayerOne.transform.DOMoveY(0, animationTime).SetEase(Ease.OutCubic));

        //StartCoroutine(MovePlayerOne());
    }

    IEnumerator MovePlayerOne()
    {
        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i <= diceRoll; i++)
        {
            Debug.Log("start");

            yield return new WaitForSeconds(3.0f);

            Sequence MoveAndJump = DOTween.Sequence();
            MoveAndJump.PrependInterval(0.3f)
                //.Append(PlayerOne.transform.DOMove(Tiles[i].transform.position, animationTime).SetEase(Ease.InOutCubic))
                .Append(PlayerOne.transform.DOMoveY(1, animationTime/2).SetEase(Ease.InOutCubic))
                .AppendInterval(animationTime / 2)
                .Append(PlayerOne.transform.DOMoveY(0, animationTime).SetEase(Ease.InOutCubic));

            Debug.Log("end");
        }
    }

}
