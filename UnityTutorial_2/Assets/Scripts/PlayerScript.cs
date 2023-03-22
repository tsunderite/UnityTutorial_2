using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    private int scoreValue = 0;
    private int lifeValue = 3;

    public float speed;
    public Text life;
    public Text score;
    public GameObject loseTextObject;
    public GameObject winTextObject;

    public AudioSource musicSource;
    public AudioClip bgMusic;
    public AudioClip winMusic;
    public float volume = 0.5f;

    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        life.text = lifeValue.ToString();

        loseTextObject.SetActive(false);
        winTextObject.SetActive(false);

        musicSource.clip = bgMusic;
        musicSource.Play();
        musicSource.loop = true;
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if (scoreValue == 4)
            {
                LoadLevel();
                lifeValue = 3;
                life.text = lifeValue.ToString();
            }

            if (scoreValue == 9)
            {
                winTextObject.SetActive(true);
                speed = 0;
                rd2d.drag = 20;

                musicSource.clip = bgMusic;
                musicSource.Stop();

                if (speed == 0)
                {
                    musicSource.clip = winMusic;
                    musicSource.PlayOneShot(winMusic, volume);
                }
            }
        }

        if (collision.collider.tag == "Enemy")
        {
            lifeValue -= 1;
            life.text = lifeValue.ToString();
            Destroy(collision.collider.gameObject);

            if (lifeValue <= 0)
            {
            loseTextObject.SetActive(true);
            speed = 0;
            rd2d.drag = 20;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void LoadLevel()
    {
        gameObject.transform.position = new Vector3(95, 0, 0);
    }
}
