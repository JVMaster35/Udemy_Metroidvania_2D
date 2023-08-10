using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    [SerializeField] private Animator bossAnim;
    [SerializeField] private GameObject bossBullet;
    [SerializeField] private GameObject winObjects;
    [SerializeField] private Transform theBoss;
    [SerializeField] private Transform camPosition;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private Transform[] spawnPoints;

    public int threshold1, threshold2;
    public float camSpeed;
    public float activeTime, fadeOutTime, inactiveTime;
    public float moveSpeed;
    public float timeBetweenShots1, timeBetweenShots2;
    public string bossRef;

    private float activeCounter, fadeCounter, inactiveCounter;
    private float shotCounter;
    private bool battleEnded;
    private CameraController mainCamera;
    private Transform targetPoint;


    void Start()
    {
        mainCamera = FindObjectOfType<CameraController>();
        mainCamera.enabled = false;

        battleEnded = false;

        activeCounter = activeTime;

        shotCounter = timeBetweenShots1;

        AudioManager.Instance.PlayBossMusic();
    }

    void Update()
    {
        mainCamera.transform.position = 
            Vector3.MoveTowards(mainCamera.transform.position, 
            camPosition.transform.position, 
            camSpeed * Time.deltaTime);

        if (!battleEnded)
        {
            if (BossHealthController.Instance.currentHealth > threshold1)
            {
                BossPhase1();
            }
            else
            {
                BossPhase2();
            }
        }
        else
        {
            BattleWon();
        }
    }

    public void BossPhase1()
    {
        if (activeCounter > 0)
        {
            activeCounter -= Time.deltaTime;
            if (activeCounter <= 0)
            {
                BossVanish();
            }

            BossShooting();
        }
        else if (fadeCounter > 0)
        {
            fadeCounter -= Time.deltaTime;
            if (fadeCounter <= 0)
            {
                BossFade();
            }
        }
        else if (inactiveCounter > 0)
        {
            inactiveCounter -= Time.deltaTime;
            if (inactiveCounter <= 0)
            {
                BossReappear();

                theBoss.gameObject.SetActive(true);

                activeCounter = activeTime;
                shotCounter = timeBetweenShots1;
            }
        }
    }

    public void BossPhase2()
    {
        if(targetPoint == null)
        {
            targetPoint = theBoss;
            BossVanish();
        }
        else
        {
            if (Vector3.Distance(theBoss.position, targetPoint.position) > .02f)
            {
                theBoss.position = Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(theBoss.position, targetPoint.position) <= .02f)
                {
                    BossVanish();
                }

                BossShooting();
            }
            else if (fadeCounter > 0)
            {
                fadeCounter -= Time.deltaTime;
                if (fadeCounter <= 0)
                {
                    BossFade();
                }
            }
            else if (inactiveCounter > 0)
            {
                inactiveCounter -= Time.deltaTime;
                if (inactiveCounter <= 0)
                {
                    BossReappear();

                    targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                    int whileBreaker = 0;

                    while(targetPoint.position == theBoss.position && whileBreaker < 100)
                    {
                        targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                        whileBreaker++;
                    }

                    theBoss.gameObject.SetActive(true);
                    shotCounter = timeBetweenShots2;
                }
            }
        }
    }

    public void BossShooting()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0)
        {
            if (BossHealthController.Instance.currentHealth > threshold2)
            {
                shotCounter = timeBetweenShots1;
            }
            else
            {
                shotCounter = timeBetweenShots2;
            }

            Instantiate(bossBullet, shotPoint.position, Quaternion.identity);
        }
    }

    public void BossVanish()
    {
        fadeCounter = fadeOutTime;
        bossAnim.SetTrigger("Vanish");
    }

    public void BossFade()
    {
        theBoss.gameObject.SetActive(false);
        inactiveCounter = inactiveTime;
    }

    public void BossReappear()
    {
        theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }

    public void BatleEnd()
    {
        battleEnded = true;

        fadeCounter = fadeOutTime;

        theBoss.GetComponent<Collider2D>().enabled = false;

        BossBullet[] ghostBullets = FindObjectsOfType<BossBullet>();
        if (ghostBullets.Length > 0)
        {
            foreach (BossBullet gb in ghostBullets)
            {
                Destroy(gb.gameObject);
            }
        }

    }

    public void BattleWon()
    {
        bossAnim.SetBool("IsDead", battleEnded);

        fadeCounter -= Time.deltaTime;
        if (fadeCounter < 0)
        {
            if (winObjects != null)
            {
                winObjects.SetActive(true);
                winObjects.transform.SetParent(null);
            }

            mainCamera.enabled = true;
            gameObject.SetActive(false);


            AudioManager.Instance.PlayLevelMusic();

            PlayerPrefs.SetInt(bossRef, 1);
        }
    }

}
