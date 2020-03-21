using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    private GameObject Manager;
    SwipeEventManager swipeEventManager;

    //spawn object and location
    public Rigidbody Dice;
    public Transform SpawnLocation;

    //variables
    private float SwipeSpeed;
    private float MinSpeed = 0.1f;

    private void Awake()
    {
        Manager = GameObject.FindGameObjectWithTag("manager");
        if (Manager != null)
            swipeEventManager = Manager.GetComponent<SwipeEventManager>();

        //SwipeSpeed = swipeEventManager.swipeDist;
    }
    public void Roll()
    {
        SwipeSpeed =300;

        // Create an instance of the dice and store a reference to it's rigidbody.
        Rigidbody DiceInstance = Instantiate(Dice, SpawnLocation.position, SpawnLocation.rotation) as Rigidbody;
        DiceInstance.useGravity = true;

        // Set the Dice velocity to the launch force in the fire position's forward direction.
        DiceInstance.velocity = SwipeSpeed/100 * SpawnLocation.forward; ;

        // Reset the swipe Speed.(just a precaution)
        SwipeSpeed = MinSpeed;

        Invoke("DestroyGameObject", 3);
    }

    //destroy the dice after rolling it
    void DestroyGameObject()
    {
        Destroy(Dice);
    }

}
