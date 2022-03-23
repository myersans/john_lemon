using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    private float speed = 50f;
    public TextMeshProUGUI scoreText;
    
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    private int score;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start ()
    {
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource> ();
        score = 0;

        SetScoreText();
    }

    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();
        m_Movement = m_Movement * speed * Time.deltaTime;

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);
        
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop ();
        }

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
    }

    void SetScoreText ()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void OnAnimatorMove ()
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation (m_Rotation);
    }

    private void speed_change ()
    {
        speed = 50f;
    }

    private void OnTriggerEnter(Collider other) 
	{
        switch(other.gameObject.tag)
        {
            case "Yellow":
                other.gameObject.SetActive(false);
                speed += speed;
                Invoke("speed_change", 10);
                break;

            case "Orange":
                other.gameObject.SetActive(false);
                speed = speed - 40f;
                Invoke("speed_change", 10);
                break;

            case "Brown":
                other.gameObject.SetActive(false);
                float[] randSpeed = {10f, 50f, 100f};
                speed = randSpeed[Random.Range(0,randSpeed.Length)];
                Invoke("speed_change", 10);
                break;

            case "Star":
                other.gameObject.SetActive(false);
                score = score + 15;
                SetScoreText();
                break;

            case "Heart":
                other.gameObject.SetActive(false);
                score = score + 50;
                SetScoreText();
                break;

            case "Cubie":
                other.gameObject.SetActive(false);
                score = score + 20;
                SetScoreText();
                break;

            case "Sphere":
                other.gameObject.SetActive(false);
                score = score + 100;
                SetScoreText();
                break;

            case "Hexagon":
                other.gameObject.SetActive(false);
                score = score + 30;
                SetScoreText();
                break;

            case "Diamond":
                other.gameObject.SetActive(false);
                score = score + 10;
                SetScoreText();
                break;
        }
    }
}
