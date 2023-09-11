using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed modifier.")]
    [SerializeField] private float speed = 3;
    private Node currentNode;
    private Vector3 currentDir;
    private bool playerCaught = false;

    public delegate void GameEndDelegate();
    public event GameEndDelegate GameOverEvent = delegate { };

    private bool targetFound;
    private Stack<Node> unsearchedNodes = new Stack<Node>();


    // Start is called before the first frame update
    void Start()
    {
        InitializeAgent();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCaught == false)
        {
            if (currentNode != null)
            {
                // If within 0.25 units of the current node.
                if (Vector3.Distance(transform.position, currentNode.transform.position) > 0.25f)
                {
                    transform.Translate(currentDir * speed * Time.deltaTime);
                }
                else
                {
                    // Reached the current node, perform DFS to find the player's previous node
                    DepthFirstSearch();
                }
            }
            else
            {
                Debug.LogWarning($"{name} - No current node");
            }

            Debug.DrawRay(transform.position, currentDir, Color.cyan);
        }
    }

    //Called when a collider enters this object's trigger collider.
    //Player or enemy must have rigidbody for this to function correctly.
    private void OnTriggerEnter(Collider other)
    {
        if (playerCaught == false)
        {
            if (other.tag == "Player")
            {
                playerCaught = true;
                GameOverEvent.Invoke(); //invoke the game over event
            }
        }
    }

    /// <summary>
    /// Sets the current node to the first in the Game Managers node list.
    /// Sets the current movement direction to the direction of the current node.
    /// </summary>
    void InitializeAgent()
    {
        currentNode = GameManager.Instance.Nodes[0];
        currentDir = currentNode.transform.position - transform.position;
        currentDir = currentDir.normalized;
    }

    //Write DFS here
    public void DepthFirstSearch()
    {
        // Reset the targetFound flag
        targetFound = false;

        // Iterate through the Nodes on GameManager and set 'Visited' to false
        foreach (Node node in GameManager.Instance.Nodes)
        {
            node.Visited = false;
        }

        // Clear the unsearchedNodes stack if it's empty
        {
            unsearchedNodes.Push(GameManager.Instance.Nodes[0]);
        }

        // Loop until the target node is found or there are no more unsearched nodes
        while (!targetFound)
        {
            // Take the last node in the unsearchedNodes stack
            Node nodeCurrentlyBeingSearched = unsearchedNodes.Pop();

            // Check if nodeCurrentlyBeingSearched is the target node or the player's previous node
            if (nodeCurrentlyBeingSearched == GameManager.Instance.Player.CurrentNode ||
                nodeCurrentlyBeingSearched == GameManager.Instance.Player.TargetNode)
            {
                // Assign nodeCurrentlyBeingSearched as currentNode
                currentNode = nodeCurrentlyBeingSearched;
                targetFound = true;
                break;
            }

            // Add each child of nodeCurrentlyBeingSearched to the unsearchedNodes stack
            foreach (Node childNode in nodeCurrentlyBeingSearched.Children)
            {
                if (!childNode.Visited)
                {
                    unsearchedNodes.Push(childNode);
                }
                else
                {
                    Debug.Log("This node is already visited.");
                }
            }

            // Mark nodeCurrentlyBeingSearched as visited
            nodeCurrentlyBeingSearched.Visited = true;
        }

        // Clear the unsearchedNodes stack after all nodes have been searched
        unsearchedNodes.Clear();

        // Update the current direction towards the new current node
        if (currentNode != null)
        {
            currentDir = currentNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
        }

    }
}
