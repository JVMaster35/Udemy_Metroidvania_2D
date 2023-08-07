using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rgdby;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform shotPoint, bombPoint;
    [SerializeField] private BulletController shotToFire;
    [SerializeField] private SpriteRenderer spriteRend, afterImage;
    [SerializeField] private GameObject standingForm, ballForm;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Animator playerAnim, ballAnim;

    public float moveSpeed;
    public float jumpForce;
    public float dashSpeed, dashTime;
    public float afterImageLifeTime, timeBetweenAfterImages;
    public float waitAfterDashing;
    public float waitToBall;

    public bool canMove;

    public Color afterImageColor;

    public LayerMask isGroundLayer;

    private float dashCounter;
    private float afterImageCounter;
    private float dashRechargeCounter;
    private float ballCounter;

    private bool isGrounded;
    private bool canDoubleJump;

    private PlayerAbilityTracker abilities;

    private void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>();

        canMove = true;
    }

    private void Update()
    {
        //Player Movement
        PlayerMovement();

        //Player Animations
        PlayerAnimations();

        //Player Shooting
        PlayerShooting();

        //Player Ball Form
        BallForm();

        //Checks for Ground
        GroundCheck();
    }

    private void PlayerMovement()
    {
        if (canMove && Time.timeScale != 0f)
        {
            //Recharge Dash
            if (dashRechargeCounter > 0)
            {
                dashRechargeCounter -= Time.deltaTime;
            }
            else
            {
                //Player Dash
                if (Input.GetButtonDown("Fire2") && standingForm.activeSelf && abilities.canDash)
                {
                    dashCounter = dashTime;
                    AudioManager.Instance.PlaySFXAdjusted(7);
                    ShowAfterImage();
                }
            }

            if (dashCounter > 0)
            {
                PlayerDash();
            }
            else
            {
                //Move Sideways
                rgdby.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rgdby.velocity.y);

                //Handle Direction
                if (rgdby.velocity.x < 0)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else if (rgdby.velocity.x > 0)
                {
                    transform.localScale = Vector3.one;
                }
            }

            //Check for Jump Button & Ground Check
            if (Input.GetButtonDown("Jump") && (isGrounded || (canDoubleJump && abilities.canDoubleJump)))
            {
                if (isGrounded)
                {
                    canDoubleJump = true;

                    AudioManager.Instance.PlaySFXAdjusted(12);
                }
                else
                {
                    canDoubleJump = false;
                    AudioManager.Instance.PlaySFXAdjusted(9);
                    playerAnim.SetTrigger("DoubleJump");
                }

                //Player Jumping
                PlayerJump();
            }
        }
        else
        {
            rgdby.velocity = Vector2.zero;
        }
    }

    private void PlayerJump()
    {
        //Jump Up
        rgdby.velocity = new Vector2(rgdby.velocity.x, jumpForce);
    }

    public void PlayerShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (standingForm.activeSelf)
            {
                Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir =
                    new Vector2(transform.localScale.x, 0f);

                AudioManager.Instance.PlaySFXAdjusted(14);

                playerAnim.SetTrigger("ShotFired");
            } 
            else if (ballForm.activeSelf && abilities.canDropBomb)
            {
                AudioManager.Instance.PlaySFXAdjusted(13);
                Instantiate(bombPrefab, bombPoint.position, bombPoint.rotation);
            }
        }
    }

    public void PlayerDash()
    {
        dashCounter = dashCounter - Time.deltaTime;

        rgdby.velocity = new Vector2(dashSpeed * transform.localScale.x, rgdby.velocity.y);

        afterImageCounter -= Time.deltaTime;
        if( afterImageCounter < 0)
        {
            ShowAfterImage();
        }

        dashRechargeCounter = waitAfterDashing;

    }

    public void BallForm()
    {
        if(!ballForm.activeSelf)
        {
            if (Input.GetAxisRaw("Vertical") < -.9f && abilities.canBecomeBall)
            {
                ballCounter -= Time.deltaTime;

                if(ballCounter <= 0)
                {
                    ballForm.SetActive(true);
                    standingForm.SetActive(false);
                    AudioManager.Instance.PlaySFXAudio(6);
                }
            }
            else
            {
                ballCounter = waitToBall;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") > .9f)
            {
                ballCounter -= Time.deltaTime;

                if (ballCounter <= 0)
                {
                    ballForm.SetActive(false);
                    standingForm.SetActive(true);
                    AudioManager.Instance.PlaySFXAudio(10);
                }
            }
            else
            {
                ballCounter = waitToBall;
            }
        }
    }

    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = spriteRend.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        Destroy(image.gameObject, afterImageLifeTime);

        afterImageCounter = timeBetweenAfterImages;
    }

    private void PlayerAnimations()
    {
        if (standingForm.activeSelf)
        {
            playerAnim.SetBool("IsGrounded", isGrounded);
            playerAnim.SetFloat("Speed", Mathf.Abs(rgdby.velocity.x));
        }

        if(ballForm.activeSelf)
        {
            ballAnim.SetFloat("Speed", Mathf.Abs(rgdby.velocity.x));
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, isGroundLayer);
    }

    public Animator GetPlayerAnimator()
    {
        return playerAnim;
    }
}
