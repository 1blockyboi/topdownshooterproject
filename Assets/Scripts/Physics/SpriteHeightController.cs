using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHeightController : MonoBehaviour
{
    public GameObject playerSprite;
    [SerializeField]
    float height = 0.0f;
    public float maxHeight = 1.0f;
    public float minHeight = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeHeight(float newHeight)
    {
        newHeight = minMaxHeightCheck(newHeight);
        height = newHeight;
        playerSprite.transform.position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + newHeight);
    }

    public void AddHeight(float newHeight)
    {
        newHeight = minMaxHeightCheck(newHeight);
        height = newHeight;
        playerSprite.transform.position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + newHeight);
    }

    public float GetCurrentHeight()
    {
        return height;
    }

    private float minMaxHeightCheck(float checkedHeight)
    {
        if (checkedHeight > maxHeight)
        {
            return maxHeight;
        }
        else if (checkedHeight < minHeight)
        {
            return minHeight;
        }
        return checkedHeight;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
