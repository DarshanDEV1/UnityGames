using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerDirection
{
    forward,
    backward,
    left,
    right
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] PlayerDirection moveDirection = new PlayerDirection();
    [SerializeField] GameManager gameManager;
    private int playerRow, playerCol;

    private void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        ContinousMovement();
    }

    private void Move()
    {
        GetComponent<Animator>().Play("m_pistol_run");

        Vector2 direction = joystick.GetJoystickDirection();
        Vector3 move = new Vector3(direction.x, 0f, direction.y);
        //transform.Translate(move * Time.deltaTime * moveSpeed, Space.World);
        PlayerCardinalMovement(direction);
    }

    private void ContinousMovement()
    {
        PlayerPosition player = gameManager.GetPlayerPostition();

        if (moveDirection == PlayerDirection.forward) 
        { transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * 1.2f, Space.World); }
        else if (moveDirection == PlayerDirection.backward) 
        { transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * 1.2f, Space.World); }
        else if (moveDirection == PlayerDirection.left) 
        { transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * 1.2f, Space.World); }
        else if (moveDirection == PlayerDirection.right) 
        { transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * 1.2f, Space.World); }
        else { return; }
    }

    private void PlayerCardinalMovement(Vector2 direction)
    {
        if (direction.magnitude < 0.1f)
        {
            // Joystick is not being moved
            return;
        }

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement
            if (direction.x > 0)
            {
                //Debug.Log("Right");
                moveDirection = PlayerDirection.right;
            }
            else
            {
                //Debug.Log("Left");
                moveDirection = PlayerDirection.left;
            }
        }
        else
        {
            // Vertical movement
            if (direction.y > 0)
            {
                //Debug.Log("Up");
                moveDirection = PlayerDirection.forward;
            }
            else
            {
                //Debug.Log("Down");
                moveDirection = PlayerDirection.backward;
            }
        }
    }
}
