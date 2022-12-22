using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Vector2 currentVelocity;
    float xDirectionBufferCount = 0.0f;
    float yDirectionBufferCount = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Input "buffering" tick reduction. Checks are created to ensure that either positive or negative, it will tick to 0, and not overload
        //to the opposing side.
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
        
        //Current tick movement calculation begins
        float ydirection = 0.0f;
        float xdirection = 0.0f;
        bool xDirectionPressed = false;
        bool yDirectionPressed = false;

        //Pre-check to help with xdirection calculations
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            yDirectionPressed = true;
        }
        if (Input.GetKey(KeyCode.A) || (yDirectionPressed && xDirectionBufferCount < 0.0f))
        {
            xDirectionPressed = true;
            xdirection += -5.0f;
            if (Input.GetKey(KeyCode.A)) xDirectionBufferCount = -0.05f;
        }
        if (Input.GetKey(KeyCode.D) || (yDirectionPressed && xDirectionBufferCount > 0.0f))
        {
            xDirectionPressed = true;
            xdirection += 5.0f;
            if (Input.GetKey(KeyCode.D)) xDirectionBufferCount = 0.05f;
        }
        if (Input.GetKey(KeyCode.W) || (xDirectionPressed && yDirectionBufferCount > 0.0f))
        {
            ydirection += 5.0f;
            if (Input.GetKey(KeyCode.W)) yDirectionBufferCount = 0.05f;
        }
        if (Input.GetKey(KeyCode.S) || (xDirectionPressed && yDirectionBufferCount < 0.0f))
        {
            ydirection += -5.0f;
            if (Input.GetKey(KeyCode.S)) yDirectionBufferCount = -0.05f;
        }

        if (xdirection != 0.0f && ydirection != 0.0f) { xdirection = xdirection / 2; ydirection = ydirection / 2; }
        //Movement based on input/buffer is applied.
        this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x + (Time.deltaTime * xdirection), this.gameObject.transform.position.y + (Time.deltaTime * ydirection));

        //Some "momentum" is accounted for, and when the input is no longer pressed, there is a little sliding before the player reaches a complete stop.
        //Not true momentum because no mass is accounted for. Start stop is constant.
        if (xdirection == 0.0f && ydirection == 0.0f)
        {
            xDirectionBufferCount = 0.0f;
            yDirectionBufferCount = 0.0f;
            if (currentVelocity.x != 0.0f)
            {
                currentVelocity.x = currentVelocity.x > 0.0f ? currentVelocity.x - (50.0f * Time.deltaTime) : currentVelocity.x + (50.0f * Time.deltaTime);
            }
            if (currentVelocity.y != 0.0f)
            {
                currentVelocity.y = currentVelocity.y > 0.0f ? currentVelocity.y - (50.0f * Time.deltaTime) : currentVelocity.y + (50.0f * Time.deltaTime);

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
