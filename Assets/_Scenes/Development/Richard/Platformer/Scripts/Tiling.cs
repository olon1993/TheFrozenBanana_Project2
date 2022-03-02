using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    public int offsetX = 2;

    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;


    private float spriteWidth = 0f;
    private Camera cam;
    private Transform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    void Update()
    {
        // if already has buddies we don't need to do anything else in the update
        if (hasALeftBuddy && hasARightBuddy)
            return;

        // calculate the extent of what the camera can see in world coordinates
        float camHorizontalExtent = cam.orthographicSize * Screen.width / Screen.height;

        // calculate the x position for the camera where it can see the edge of the sprite
        float edgePositionVisibleRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtent;
        float edgePositionVisibleLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtent;

        // check if we can see the edge of the sprite and call MakeNewBuddy if we can
        if (cam.transform.position.x >= edgePositionVisibleRight - offsetX && !hasARightBuddy)
        {
            MakeNewBuddy(1);
            hasARightBuddy = true;
        }
        else if (cam.transform.position.x <= edgePositionVisibleLeft + offsetX && !hasALeftBuddy)
        {
            MakeNewBuddy(-1);
            hasALeftBuddy = true;
        }
    }

    // function to create a new buddy on the appropriate side
    void MakeNewBuddy(int rightOrLeft)
    {
        // calculate position for new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        // instantiate new buddy and store as variabe
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        // if not tileable reverse the x size of object to make a seamless join
        if (reverseScale)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;

        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
