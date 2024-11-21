using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector2 cursorPosition;
    public void Update()
    {
        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector2(cursorPosition.x, cursorPosition.y);

    }
}
