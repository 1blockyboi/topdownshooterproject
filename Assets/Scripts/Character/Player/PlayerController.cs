using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum jumpDirection
{
    Left,
    Right,
    Up,
    Down,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
    None
}

public class PlayerController : MonoBehaviour
{
    //public variables
    public float xDirectionBufferAmount = 0.05f;
    public float yDirectionBufferAmount = 0.05f;
    public float movementSpeedx = 5.0f;
    public float movementSpeedy = 5.0f;
    public float frictionAmount = 50.0f;
    float jumpBufferAmount = 1.0f;
    public KeyCode leftDirectionInput = KeyCode.A;
    public KeyCode rightDirectionInput = KeyCode.D;
    public KeyCode upDirectionInput = KeyCode.W;
    public KeyCode downDirectionInput = KeyCode.S;
    public KeyCode jumpInput = KeyCode.Space;
    public float maxJumpHeight = 1.0f;
    public float jumpSpeed = 1.0f;
    SpriteHeightController heightController;



    Vector2 currentVelocity;
    float xDirectionBufferCount = 0.0f;
    float yDirectionBufferCount = 0.0f;
    float jumpBufferCount = 0.0f;
    bool jumpPressed = false;
    jumpDirection playerJumpDirection;

    // Start is called before the first frame update
    void Start()
    {
        heightController = GetComponent<SpriteHeightController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Input "buffering" tick reduction. Checks are created to ensure that either positive or negative, it will tick to 0, and not overload
        //to the opposing side.
        if (!jumpPressed)
        {
            if (xDirectionBufferCount != 0.0f)
            {
                if (xDirectionBufferCount > 0.0f)
                {
                    float xDirectionBufferCountTemp = xDirectionBufferCount - Time.deltaTime;
                    xDirectionBufferCount = xDirectionBufferCountTemp >= 0.0f ? xDirectionBufferCountTemp : 0.0f;
                }
                else
                {
                    float xDirectionBufferCountTemp = xDirectionBufferCount + Time.deltaTime;
                    xDirectionBufferCount = xDirectionBufferCountTemp <= 0.0f ? xDirectionBufferCountTemp : 0.0f;
                }
            }
            if (yDirectionBufferCount != 0.0f)
            {
                if (yDirectionBufferCount > 0.0f)
                {
                    float yDirectionBufferCountTemp = yDirectionBufferCount - Time.deltaTime;
                    yDirectionBufferCount = yDirectionBufferCountTemp >= 0.0f ? yDirectionBufferCountTemp : 0.0f;
                }
                else
                {
                    float yDirectionBufferCountTemp = yDirectionBufferCount + Time.deltaTime;
                    yDirectionBufferCount = yDirectionBufferCountTemp <= 0.0f ? yDirectionBufferCountTemp : 0.0f;
                }
            }
        }
        //Current tick movement calculation begins
        float ydirection = 0.0f;
        float xdirection = 0.0f;
        bool xDirectionPressed = false;
        bool yDirectionPressed = false;
        
        
        if (jumpBufferCount > 0.0f)
        {
            jumpBufferCount -= jumpSpeed * Time.deltaTime;
        }
        else
        {
            if (jumpPressed)
            {
                jumpPressed = false;
                jumpBufferCount = 0.0f;
            }
        }

        //Test once if Jump input is pressed.
        if (Input.GetKeyDown(jumpInput) && !jumpPressed)
        {
            jumpPressed = true;
            jumpBufferCount = jumpBufferAmount;
        }
        //Pre-check to help with xdirection calculations
        if (!jumpPressed)
        {
            if (Input.GetKey(upDirectionInput) || Input.GetKey(KeyCode.S))
            {
                yDirectionPressed = true;
            }
            if (Input.GetKey(leftDirectionInput) || (yDirectionPressed && xDirectionBufferCount < 0.0f))
            {
                xDirectionPressed = true;
                xdirection += -movementSpeedx;
                if (Input.GetKey(leftDirectionInput)) xDirectionBufferCount = -xDirectionBufferAmount;
            }
            if (Input.GetKey(rightDirectionInput) || (yDirectionPressed && xDirectionBufferCount > 0.0f))
            {
                xDirectionPressed = true;
                xdirection += movementSpeedx;
                if (Input.GetKey(rightDirectionInput)) xDirectionBufferCount = xDirectionBufferAmount;
            }
            if (Input.GetKey(upDirectionInput) || (xDirectionPressed && yDirectionBufferCount > 0.0f))
            {
                ydirection += movementSpeedy;
                if (Input.GetKey(upDirectionInput)) yDirectionBufferCount = yDirectionBufferAmount;
            }
            if (Input.GetKey(downDirectionInput) || (xDirectionPressed && yDirectionBufferCount < 0.0f))
            {
                ydirection += -movementSpeedy;
                if (Input.GetKey(downDirectionInput)) yDirectionBufferCount = -yDirectionBufferAmount;
            }

            if (xdirection != 0.0f && ydirection != 0.0f) { xdirection = xdirection / 2; ydirection = ydirection / 2; }

            //Movement based on input/buffer is applied.
           // this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x + (Time.deltaTime * xdirection), this.gameObject.transform.position.y + (Time.deltaTime * ydirection));
        }
        else
        {
            float additionalHeight = 0.0f;
            //Jump arc math
            additionalHeight = -4.0f * Mathf.Pow((jumpBufferAmount - jumpBufferCount) - 0.5f, 2) + 1;

            heightController.ChangeHeight(additionalHeight);
            //playerSprite.transform.position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + additionalHeight);

            if (xDirectionBufferCount > 0.0f)
            {
                xdirection += movementSpeedx;
            }
            else if (xDirectionBufferCount < 0.0f)
            {
                xdirection -= movementSpeedx;
            }
            if (yDirectionBufferCount > 0.0f)
            {
                ydirection += movementSpeedy;
            }
            else if (yDirectionBufferCount < 0.0f)
            {
                ydirection -= movementSpeedy;
            }       
        }
        //Movement based on input/buffer is applied.
        this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x + (Time.deltaTime * xdirection), this.gameObject.transform.position.y + (Time.deltaTime * ydirection));


        //Some "momentum" is accounted for, and when the input is no longer pressed, there is a little sliding before the player reaches a complete stop.
        //Not true momentum because no mass is accounted for. Start stop is constant.
        if (xdirection == 0.0f && ydirection == 0.0f && !jumpPressed)
        {
            xDirectionBufferCount = 0.0f;
            yDirectionBufferCount = 0.0f;
            if (currentVelocity.x != 0.0f)
            {
                currentVelocity.x = currentVelocity.x > 0.0f ? currentVelocity.x - (frictionAmount * Time.deltaTime) : currentVelocity.x + (frictionAmount * Time.deltaTime);
            }
            if (currentVelocity.y != 0.0f)
            {
                currentVelocity.y = currentVelocity.y > 0.0f ? currentVelocity.y - (frictionAmount * Time.deltaTime) : currentVelocity.y + (frictionAmount * Time.deltaTime);

            }

            //Momentum slide is applied.
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x + (currentVelocity.x*Time.deltaTime), this.gameObject.transform.position.y + (currentVelocity.y*Time.deltaTime));
        }
        else
        {
            //current velocity is set to the max applied from current tick movement calculations.
            currentVelocity = new Vector2(xdirection, ydirection);
        }


        //Actions
    }
}
