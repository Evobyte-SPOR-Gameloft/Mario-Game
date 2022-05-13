using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 velocity;

    public LayerMask wallMask;

    private bool walk, walk_left, walk_right, jump;

    public float jumpVelocity;

    public float gravity;
    public LayerMask floorMask;

    public enum PlayerState
    {
        jumping,
        idle,
        walking
    }

    private PlayerState playerState = PlayerState.idle;

    private bool ground = false;

    void Start()
    {
        //fall();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();
        UpdatePlayerPosition();
    }

    void UpdatePlayerPosition()
    {
        Vector3 position = transform.localPosition;
        Vector3 scale = transform.localScale;
        if(walk)
        {
            if (walk_left)
            {
                position.x -= velocity.x*Time.deltaTime;
                scale.x = -1;
            }
            if (walk_right)
            {
                position.x += velocity.x * Time.deltaTime;
                scale.x = 1;
            }
            position = CheckWallRays(position,scale.x);
        }

        if(jump && playerState != PlayerState.jumping)
        {
            playerState = PlayerState.jumping;
            velocity = new Vector2(velocity.x, jumpVelocity);
        }

        if(playerState == PlayerState.jumping)
        {
            position.y = velocity.y * Time.deltaTime;
            velocity.y -= gravity * Time.deltaTime;
        }

        if(velocity.y==0)
        {
            position = CheckFloorRays(position);
        }

        transform.localPosition = position;
        transform.localScale = scale;
    }

    void CheckPlayerInput()
    {
        bool input_left = Input.GetKey(KeyCode.LeftArrow);
        bool input_right = Input.GetKey(KeyCode.RightArrow);
        bool input_space = Input.GetKey(KeyCode.Space);

        walk = input_left || input_right;
        walk_left = input_left && !input_right;
        walk_right = input_right && !input_left;
        jump = input_space;
    }

    Vector3 CheckWallRays (Vector3 position, float direction)
    {
        Vector2 originTop = new Vector2(position.x + direction * .4f, position.y + 1f - 0.2f);
        Vector2 originMid = new Vector2(position.x + direction * .4f, position.y);
        Vector2 originBot = new Vector2(position.x + direction * .4f, position.y - 1f + 0.2f);

        RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallMid = Physics2D.Raycast(originMid, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallBot = Physics2D.Raycast(originBot, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);

        if (wallTop.collider != null || wallMid.collider != null || wallBot.collider != null)
        {
            position.x -= velocity.x * Time.deltaTime * direction;
        }

        return position;
    }

    Vector3 CheckFloorRays(Vector3 position)
    {
        Vector2 originLeft = new Vector2 (position.x - 0.5f + 0.2f, position.y-1f);
        Vector2 originMid = new Vector2 (position.x, position.y-1f);
        Vector2 originRight = new Vector2 (position.x + 0.5f-0.2f, position.y-1f);

        RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorMid = Physics2D.Raycast(originMid, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

        if (floorLeft.collider != null || floorMid.collider != null || floorRight.collider != null)
        {
            RaycastHit2D hitRay=floorLeft;
            
            if(floorLeft)
                hitRay = floorLeft;
            if (floorMid)
                hitRay = floorMid;
            if (floorRight)
                hitRay = floorRight;
            
            playerState = PlayerState.idle;

            ground = true;
            
            velocity.y=0;

            position.y=hitRay.collider.bounds.center.y + (hitRay.collider.bounds.size.y/2)+1;
            
            
            
        }

        return position;
    }

    void fall()
    {
        velocity.y=0;
        playerState = PlayerState.jumping;
        ground=false;
    }
}
