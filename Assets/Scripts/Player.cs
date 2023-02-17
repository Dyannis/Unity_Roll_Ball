using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private int ScoreValue;

    [SerializeField] private float speedBall = 5f;
    private float movementX;
    private float movementY;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private ScenarioData _scenario;
    [SerializeField] private GameObject _wallPrefab;

    public delegate void SoundEvent();
    public static event SoundEvent ballTouch;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        ScoreValue = PlayerPrefs.GetInt("Score");
        _scoreText.text = "Score : " + ScoreValue;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("ENTER ONLOAD");
        if (scene.name == "Level - 1")
        {
            PlayerPrefs.DeleteKey("Score");
            ScoreValue = 0;
            _scoreText.text = "Score : " + ScoreValue;
        }
    }
    void Update()
    {
        //if(Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) 
        //{
        //    _rigidbody.AddForce(Input.GetAxis("Horizontal") * speedBall, 0f, Input.GetAxis("Vertical")* speedBall);
        //}
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        _rigidbody.AddForce(movement * speedBall);
    }
    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target_Trigger"))
        {
            UpdateScore();
            ballTouch?.Invoke();
            Instantiate(_wallPrefab, other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            UpdateScore();
            ballTouch?.Invoke();
            Instantiate(_wallPrefab, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
    private void UpdateScore()
    {
        ScoreValue++;
        PlayerPrefs.SetInt("Score", ScoreValue);
        _scoreText.text = "Score : " + ScoreValue;

        PlayerPrefs.Save();

        if (ScoreValue == 8)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        }

        if (PlayerPrefs.GetInt("Score") == 16)
        {
            PlayerPrefs.DeleteKey("Score");
            ScoreValue = 0;
            _scoreText.text = "Score : " + ScoreValue;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

 

}
