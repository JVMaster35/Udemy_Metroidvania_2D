using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Rigidbody2D rgdby;
    [SerializeField] private Animator enemyAnim;

    public float moveSpeed, waitAtPoints;
    public float jumpForce;

    private int currentPoint;
    private float waitCounter;

    void Start()
    {
        waitCounter = waitAtPoints;

        foreach(Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }
    }

    void Update()
    {
        if(Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > .2f)
        {
            if(transform.position.x < patrolPoints[currentPoint].position.x)
            {
                rgdby.velocity = new Vector2(moveSpeed, rgdby.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                rgdby.velocity = new Vector2(-moveSpeed, rgdby.velocity.y);
                transform.localScale = Vector3.one;
            }

            if(transform.position.y < patrolPoints[currentPoint].position.y -.25f && rgdby.velocity.y < .1f)
            {
                rgdby.velocity = new Vector2(rgdby.velocity.x, jumpForce);
            }
        }
        else
        {
            rgdby.velocity = new Vector2(0, rgdby.velocity.y);

            waitCounter -= Time.deltaTime;

            if(waitCounter <= 0)
            {
                waitCounter = waitAtPoints;

                currentPoint++;

                if(currentPoint >= patrolPoints.Length)
                {
                    currentPoint = 0;
                }
            }
        }

        enemyAnim.SetFloat("Speed", Mathf.Abs(rgdby.velocity.x));
    }
}
