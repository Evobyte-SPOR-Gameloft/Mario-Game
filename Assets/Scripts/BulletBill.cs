using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletBill : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 velocity = new Vector2();
    public LayerMask wallMask;
    void Start()
    {
        
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
            if(hitRay.collider.tag=="Player")
            {
                SceneManager.LoadScene("GameOver");
            }
            else Destroy(this.gameObject);
        }
    }

    

    void UpdateBulletPosition()
    {
        Vector3 position = transform.localPosition;
        position.x -= velocity.x * Time.deltaTime;
        CheckWalls(position,-1);
        transform.localPosition = position;
        
    }
    // Update is called once per frame
    void Update()
    {
        UpdateBulletPosition();
    }
}
