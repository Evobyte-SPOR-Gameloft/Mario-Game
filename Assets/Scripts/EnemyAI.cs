using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update

    public float gravity;

    public Vector2 velocity;
    public bool isWalkingLeft = true;

    private bool ground = false;

    private bool shouldDie = false;
    private float deathTimer = 0;

    public float timeBeforeDestroy = 2.0f;

    public LayerMask floorMask;
    public LayerMask wallMask;
    private enum EnemyState{
        walking,
        falling,
        dead
    }

    private EnemyState state = EnemyState.falling;
    void Start()
    {
        enabled = false;
        fall();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemyPosition();
        CheckCrushed();
    }
    void OnBecameVisible()
    {
        enabled = true;
    }

    public void Crush()
    {
        state = EnemyState.dead;
        GetComponent<Animator>().SetBool("isCrushed",true);
        GetComponent<Collider2D>().enabled = false;
        shouldDie = true;
    }

    void CheckCrushed()
    {
        if(shouldDie)
        {
            if(deathTimer <= timeBeforeDestroy)
            {
                deathTimer += Time.deltaTime;
            }
            else
            {
                shouldDie = false;
                Destroy(this.gameObject);
            }
        }
    }
    void UpdateEnemyPosition()
    {
        if(state != EnemyState.dead)
        {
            Vector3 position = transform.localPosition;
            Vector3 scale = transform.localScale;

            if(state == EnemyState.falling)
            {
                position.y += velocity.y * Time.deltaTime;
                velocity.y -= gravity * Time.deltaTime;
                if(isWalkingLeft)
                {
                    position.x -= velocity.x * Time.deltaTime;
                    scale.x = -1;
                }
                else
                {
                    position.x += velocity.x * Time.deltaTime;
                    scale.x = 1;
                }
            }
            if(state == EnemyState.walking)
            {
                if(isWalkingLeft)
                {
                    position.x -= velocity.x * Time.deltaTime;
                    scale.x = -1;
                }
                else
                {
                    position.x += velocity.x * Time.deltaTime;
                    scale.x = 1;
                }
            }

            if(velocity.y<=0)
            {
                position = CheckGround(position);
            }
            CheckWalls(position,scale.x);
            transform.localPosition = position;
            transform.localScale = scale;
        }
    }

    Vector3 CheckGround (Vector3 position)
    {
        Vector2 originLeft = new Vector2 (position.x - 0.5f+0.2f, position.y -0.5f);
        Vector2 originMid = new Vector2(position.x, position.y-0.5f);
        Vector2 originRight = new Vector2(position.x+0.5f-0.2f, position.y-0.5f);

        RaycastHit2D groundLeft = Physics2D.Raycast (originLeft, Vector2.down, velocity.y*Time.deltaTime,floorMask);
        RaycastHit2D groundMid = Physics2D.Raycast (originMid, Vector2.down, velocity.y*Time.deltaTime,floorMask);
        RaycastHit2D groundRight = Physics2D.Raycast (originRight, Vector2.down, velocity.y*Time.deltaTime,floorMask);

        if(groundLeft.collider != null || groundMid.collider != null || groundRight.collider != null)
        {
            RaycastHit2D hitRay = groundLeft;

            if(groundLeft)
                hitRay = groundLeft;
            if(groundMid)
                hitRay = groundMid;
            if(groundRight)
                hitRay = groundRight;
            if(hitRay.collider.tag == "Player"){
                SceneManager.LoadScene("GameOver");
            }
            position.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y /2 +0.5f;

            ground = true;
            velocity.y=0;
            state = EnemyState.walking;
        }
        else
        {
            if(state != EnemyState.falling)
                fall();
        }

        return position;
    }
    
    void CheckWalls(Vector3 position, float direction)
    {
        Vector2 originTop = new Vector2(position.x + direction * 0.4f, position.y + 0.5f - 0.2f);
        Vector2 originMid = new Vector2(position.x + direction * 0.4f, position.y);
        Vector2 originBottom = new Vector2(position.x + direction * 0.4f, position.y - 0.5f + 0.2f);
    
        RaycastHit2D wallTop = Physics2D.Raycast (originTop, new Vector2(direction,0), velocity.x*Time.deltaTime,wallMask);
        RaycastHit2D wallMid = Physics2D.Raycast (originMid, new Vector2(direction,0), velocity.x*Time.deltaTime,wallMask);
        RaycastHit2D wallBottom = Physics2D.Raycast (originBottom, new Vector2(direction,0), velocity.x*Time.deltaTime,wallMask);

        if(wallTop.collider != null || wallMid.collider != null || wallBottom.collider != null)
        {
            RaycastHit2D hitRay = wallTop;

            if(wallTop)
                hitRay = wallTop;
            if(wallMid)
                hitRay = wallMid;
            if(wallBottom)
                hitRay = wallBottom;

            isWalkingLeft = !isWalkingLeft;
        }
    }
    void fall()
    {
        velocity.y=0;
        state = EnemyState.falling;
        ground = false;
    }
}
