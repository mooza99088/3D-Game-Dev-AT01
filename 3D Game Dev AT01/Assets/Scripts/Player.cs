using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Define delegate types and events here

    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }

    [SerializeField] private float speed = 4;
    private bool moving = false;
    private Vector3 currentDir;

    [SerializeField] private EventSystem _eventSystem;
    private bool canInput = true; // Added flag to control input



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
        if (moving)
        {
            if (Vector3.Distance(transform.position, TargetNode.transform.position) > 0.25f)
            {
                transform.Translate(currentDir * speed * Time.deltaTime);
            }
            else
            {
                moving = false;
                CurrentNode = TargetNode;
                canInput = true; // Enable input when movement is done
            }
        }
        else if (canInput) // Check if input is allowed
        {
            PlayerMoveInput();
        }
    }

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

    // Implement mouse interaction method here

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

    public void CheckForNode(Vector3 checkDirection)
    {
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
        if (!moving)
        {
            TargetNode = node;
            currentDir = TargetNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
            moving = true;
            canInput = false; // Disable input when moving
        }
    }
}

