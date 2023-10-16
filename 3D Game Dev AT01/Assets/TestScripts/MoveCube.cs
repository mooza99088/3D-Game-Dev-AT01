using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script will detect input on the keyboard or input on the on-screen buttons
 * and check if the player cube is able to move in the chosen direction
 * then move the cube
 *   
 */

public class MoveCube : MonoBehaviour
{
    //variable of type 'Node' called 'targetNode'
    //this is the destination we want the player to move towards
    private Node targetNode;

    //variable of type bool for checking if the player is currently 'moving'
    private bool moving = false;

    //variable of type float for checking the 'distance' to the 'targetNode'
    [SerializeField] private float distance = 0.5f;

    //variable of type int called 'direction'
    //0 = north
    //1 = east
    //2 = south
    //3 = west


    public delegate void DirectionalButtonPush(bool greenOrRed, string buttonID);
    public static DirectionalButtonPush directionalButtonPushEvent;


    private void Update()
    {
        //check if the player is moving
        if (!moving)
        {
            //if the player is not moving check for input
            Vector3 moveDir = Vector3.zero;

            moveDir.x = Input.GetAxisRaw("Horizontal");
            moveDir.z = Input.GetAxisRaw("Vertical");

            if(moveDir.x < 0)
            {
                //call the check direction method and pass it 3
                CheckDirection(3);
            }
            else if(moveDir.x > 0)
            {
                //the check direction method and pass it a 1
                CheckDirection(1);
            }
            else if(moveDir.z < 0)
            {
                //call the check direction method and pass it 2
                CheckDirection(2);
            }
            else if(moveDir.z > 0)
            {
                //call the check direction method and pass it 0
                CheckDirection(0);
            }            
        }
        else
        {
            //keep checking if the player has arrived at the target node
            //if the player IS moving then move them towards the target node
            if (targetNode != null)
            {
                if (Vector3.Distance(transform.position, targetNode.transform.position) > distance)
                {
                    transform.position = Vector3.Lerp(transform.position, targetNode.transform.position, 3f * Time.deltaTime);
                }
                else
                {
                    //if they arrive, then switch 'moving' to false
                    moving = false;
                }
            }
        }


    }

    //method for updating the destination of the player
    //it takes in a variable of type 'Node' as a parameter
    public void SetDestination(Node node)
    {
        targetNode = node;
        //if the player choose a valid direction switch 'moving' to true
        moving = true;
        //update the direction the player is facing to face towards the targetNode
    }


    //method for checking if a chosen direction is 'valid'
    //it takes in an integer as a parameter
    //and returns a variable of type 'Node'
    public void CheckDirection(int testDir)
    {
        RaycastHit hit;

        switch (testDir)
        {
            case 0:
                
                if(Physics.Raycast(transform.position, Vector3.forward,out hit, 100f))
                {
                    if(hit.collider.transform.TryGetComponent<Node>(out Node _node))
                    {
                        //update the destination of the player
                        SetDestination(_node);
                        directionalButtonPushEvent(true, "north");
                    }
                    else
                    {
                        directionalButtonPushEvent(false, "north");
                    }
                }
                break;
            case 1:
                
                if (Physics.Raycast(transform.position, Vector3.right,out hit, 100f))
                {
                    if (hit.collider.transform.TryGetComponent<Node>(out Node _node))
                    {
                        //update the destination of the player
                        SetDestination(_node);
                        directionalButtonPushEvent(true, "east");
                    }
                    else
                    {
                        directionalButtonPushEvent(false, "east");
                    }
                }
                break;
            case 2:

                if (Physics.Raycast(transform.position, -Vector3.forward,out hit, 100f))
                {
                    if (hit.collider.transform.TryGetComponent<Node>(out Node _node))
                    {
                        //update the destination of the player
                        SetDestination(_node);
                        directionalButtonPushEvent(true, "south");
                    }
                    else
                    {
                        directionalButtonPushEvent(false, "south");
                    }
                }
                break;
            case 3:

                if (Physics.Raycast(transform.position, -Vector3.right,out hit, 100f))
                {
                    if (hit.collider.transform.TryGetComponent<Node>(out Node _node))
                    {
                        //update the destination of the player
                        SetDestination(_node);
                        directionalButtonPushEvent(true, "west");
                    }
                    else
                    {
                        directionalButtonPushEvent(false, "west");
                    }
                }
                break;
        }
    }


}
