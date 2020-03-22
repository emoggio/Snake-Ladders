using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    private GameObject Manager;
    SwipeEventManager swipeEventManager;

    //spawn object and location
    public Rigidbody Dice;
    Rigidbody DiceClone;
    private Transform SpawnLocation;

    //variables
    private float SwipeSpeed;
    private float MinSpeed = 0.1f;

    private void Awake()
    {
        Manager = GameObject.FindGameObjectWithTag("manager");
        if (Manager != null)
            swipeEventManager = Manager.GetComponent<SwipeEventManager>();

        //SwipeSpeed = swipeEventManager.swipeDist;

        SpawnLocation = this.transform;
    }
    public void Roll()
    {
        SwipeSpeed =300;

        // Create an instance of the dice and store a reference to it's rigidbody.
        DiceClone = Instantiate(Dice, SpawnLocation.position, SpawnLocation.rotation) as Rigidbody;
        DiceClone.useGravity = true;

        // Set the Dice velocity to the launch force in the fire position's forward direction.
        DiceClone.velocity = SwipeSpeed/100 * SpawnLocation.forward; ;

        // Reset the swipe Speed.(just a precaution)
        SwipeSpeed = MinSpeed;
    }
}
