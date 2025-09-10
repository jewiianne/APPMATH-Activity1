using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SampleMath : MonoBehaviour
{
    public Transform target;
    public Transform goal;

    public float moveSpeed = 5.0f;
    public float triggerDistance = 2f;
    public float dangerDistance = 1f;
    public float shakeIntensity = 50f;

    public Sprite target1;
    public Sprite target2;
    public SpriteRenderer targetSprite;

    public GameObject gameOverScreen;
    public GameObject gameWinScreen;

    void Awake()
    {
        gameOverScreen.SetActive(false);
        gameWinScreen.SetActive(false);
        
        if (target != null)
        {
            targetSprite = target.GetComponent<SpriteRenderer>();
            if (targetSprite != null && target1 != null)
            {
                targetSprite.sprite = target1;
            }
        }
    }

    void Update()
    {
        if (this.transform != null) playerMovement();

        if (target != null)
        {
            var distance = Mathf.Sqrt(Mathf.Pow(target.position.x - this.transform.position.x, 2)
            + Mathf.Pow(target.position.y - this.transform.position.y, 2));
            var vectorDist = Vector3.Distance(target.position, this.transform.position);
            Debug.Log($"Distance {distance:F2}, Vector {vectorDist:F2}");

            if(distance < dangerDistance)
            {
                GameOver();
            }

            else if(distance < triggerDistance)
            {
                Shake();
                Debug.Log("Shake");

                if(targetSprite != null && target2 != null)
                {
                    targetSprite.sprite = target2;
                }
                else
                {
                    if(targetSprite != null && target1 != null)
                    {
                        targetSprite.sprite = target1;
                    }
                }
            }
        }

        if (goal != null)
        {
            var distance = Mathf.Sqrt(Mathf.Pow(goal.position.x - this.transform.position.x, 2)
            + Mathf.Pow(goal.position.y - this.transform.position.y, 2));
            var vectorDist = Vector3.Distance(goal.position, this.transform.position);

            if(distance < dangerDistance)
            {
                GameWin();
            }
        }
    }

    private void playerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);
        transform.Translate(0, verticalInput * moveSpeed * Time.deltaTime, 0);
    }

    private void Shake()
    {
        var newVector = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        this.transform.position += newVector * Time.deltaTime * shakeIntensity;
    }

    private void GameOver()
    {
        gameOverScreen.SetActive(true);
        StartCoroutine(RestartScene());
    }

    private void GameWin()
    {
        gameWinScreen.SetActive(true);
        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync("SampleScene");
    }
}
