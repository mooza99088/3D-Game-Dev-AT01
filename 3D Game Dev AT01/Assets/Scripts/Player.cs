using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    //Define delegate types and events here

    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }

    [SerializeField] private float speed = 4;
    private bool moving = false;
    private Vector3 currentDir;


    [SerializeField] EventSystem _eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Node node in GameManager.Instance.Nodes)
        {
            if (node.Parents.Length > 2 && node.Children.Length == 0)
            {
                CurrentNode = node;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving == false)
        {
            //Implement inputs and event-callbacks here
        }
        else
        {
            if (Vector3.Distance(transform.position, TargetNode.transform.position) > 0.25f)
            {
                transform.Translate(currentDir * speed * Time.deltaTime);
            }
            else
            {
                moving = false;
                CurrentNode = TargetNode;
            }
        }
        PlayerMoveInput();
    }

    //Implement mouse interaction method here
    private void PlayerMoveInput()
    {



        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            CheckForNode(-Vector3.right);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            CheckForNode(Vector3.right);
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            CheckForNode(-Vector3.forward);
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            CheckForNode(Vector3.forward);
        }
    }

    public void PlayerMouseInput(int direction)
    {
        switch (direction)
        {
            case 0:
                CheckForNode(Vector3.forward);
                return;
            case 1:
                CheckForNode(Vector3.right);
                return;
            case 2:
                CheckForNode(-Vector3.forward);
                return;
            case 3:
                CheckForNode(-Vector3.right);
                return;
        }
    }
    //call the input(direction) method
    //invoke 'change colour' event

    /// <summary>
    /// Sets the players target node and current directon to the specified node.
    /// </summary>
    /// <param name="node"></param>
    /// 
    public void CheckForNode(Vector3 checkDirection)
    {
        /*takes in int to determine direction
         * 0 = north
         * 1 = east
         * 2 = south
         * 3 = west
         */


        Vector3 direction = Vector3.zero;

        RaycastHit hit;
        Node node;

        if (Physics.Raycast(transform.position, checkDirection, out hit, 50f))
        {
            if (hit.collider.TryGetComponent<Node>(out node))
            {
                MoveToNode(node);
            }
        }
        else
        {
            Debug.Log("No valid node in that direction");
        }

    }

    public void MoveToNode(Node node)
    {
        if (moving == false)
        {
            TargetNode = node;
            currentDir = TargetNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
            moving = true;
        }
    }
}
