using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  [Header("Level Settings:")]
  public Transform[] startingPositions;
  public GameObject[] rooms;
  private int direction;
  public float moveAmount = 10f;
  private float timeBtwRoom;
  public float startTimeBtwRoom = 0.25f;
  public float minX = 5f;
  public float maxX = 35f;
  public float minZ = 5f;
  public bool stopGeneration;
  public LayerMask whatIsRoom;
  private int downCounter;
  private GameObject[] roomsSpawned;
  private GameObject[] balls;

  [Header("Game Settings:")]
  public GameObject player;
  public GameObject shield;
  public Slider slider;
  public PlayerHealth playerHealth;
  public GameObject mainMenu;
  public GameObject pauseMenu;
  public GameObject winLoseMenu;
  public GameObject winText;
  public GameObject loseText;
  public GameObject gameUI;
  public Toggle toggleKeyboard;
  public Toggle toggleTouch;

  public static bool useMobileController;

  private void Awake()
  {
    DisablePlayerControl();
  }

  public void StartBuild()
  {
    int randStartingPos = Random.Range(0, startingPositions.Length);
    transform.position = startingPositions[randStartingPos].position;
    Instantiate(rooms[0], transform.position, Quaternion.identity);

    direction = Random.Range(1, 6);
  }

  private void DisablePlayerControl()
  {
    player.GetComponent<PlayerShooting>().enabled = false;
    player.GetComponent<PlayerMovement>().enabled = false;
    gameUI.SetActive(false);
    pauseMenu.SetActive(false);
    winLoseMenu.SetActive(false);
  }

  private void EnablePlayerControl()
  {
    player.GetComponent<PlayerMovement>().enabled = true;
    player.GetComponent<PlayerShooting>().enabled = true;
    shield.GetComponent<Animator>().SetTrigger("shieldFlash");
  }


  private IEnumerator CloseLoadingScreen()
  {
    yield return new WaitForSeconds(.5f);
    mainMenu.SetActive(false);
    gameUI.SetActive(true);
    EnablePlayerControl();
    yield return new WaitForSeconds(3f);
    shield.SetActive(false);
  }

  private void OpenLoseScreen()
  {
    Cursor.visible = true;
    gameUI.SetActive(false);
    winLoseMenu.SetActive(true);
    loseText.SetActive(true);
    winText.SetActive(false);
  }

  private void OpenWinScreen()
  {
    player.GetComponent<PlayerShooting>().enabled = false;
    player.GetComponent<PlayerMovement>().enabled = false;
    Cursor.visible = true;
    gameUI.SetActive(false);
    winLoseMenu.SetActive(true);
    loseText.SetActive(false);
    winText.SetActive(true);
  }

  private IEnumerator ResetLevel()
  {
    yield return new WaitForSeconds(2f);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  private void LoadingSlider()
  {
    if (slider.value == 1)
    {
      return;
    }
    slider.value = roomsSpawned.Length / 16f;
  }

  private void Update()
  {
    if (timeBtwRoom <= 0 && stopGeneration == false)
    {
      Move();
      timeBtwRoom = startTimeBtwRoom;
    }
    else
    {
      timeBtwRoom -= Time.deltaTime;
    }

    roomsSpawned = GameObject.FindGameObjectsWithTag("Room");
    balls = GameObject.FindGameObjectsWithTag("Ball");

    if (roomsSpawned.Length > 0)
    {
      LoadingSlider();
    }

    if (roomsSpawned.Length == 16 && balls.Length == 0 && playerHealth.health > 0)
    {
      OpenWinScreen();
    }

    if (playerHealth.health <= 0)
    {
      OpenLoseScreen();
    }

    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (mainMenu.activeSelf)
      {
        return;
      }
      PauseGame();
    }
  }

  private void Move()
  {
    if (direction == 1 || direction == 2) // Move Right
    {
      if (transform.position.x < maxX)
      {
        downCounter = 0;
        Vector3 newPos = new Vector3(transform.position.x + moveAmount, 0, transform.position.z);
        transform.position = newPos;

        int rand = Random.Range(0, rooms.Length);
        Instantiate(rooms[rand], transform.position, Quaternion.identity);

        // Makes sure the level generator doesn't move left 
        direction = Random.Range(1, 6);
        if (direction == 3)
        {
          direction = 1;
        }
        else if (direction == 4)
        {
          direction = 5;
        }
      }
      else
      {
        direction = 5;
      }
    }
    else if (direction == 3 || direction == 4) // Move Left
    {
      if (transform.position.x > minX)
      {
        downCounter = 0;
        Vector3 newPos = new Vector3(transform.position.x - moveAmount, 0, transform.position.z);
        transform.position = newPos;

        int rand = Random.Range(0, rooms.Length);
        Instantiate(rooms[rand], transform.position, Quaternion.identity);

        direction = Random.Range(3, 6);
      }
      else
      {
        direction = 5;
      }
    }
    else if (direction == 5) // Move Down
    {

      downCounter++;

      if (transform.position.z > minZ)
      {
        Collider[] roomDetection = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, whatIsRoom);

        if (roomDetection[0].GetComponent<RoomType>().type != 1 && roomDetection[0].GetComponent<RoomType>().type != 3)
          if (downCounter >= 2)
          {
            roomDetection[0].GetComponent<RoomType>().RoomDestruction();
            Instantiate(rooms[3], transform.position, Quaternion.identity);
          }
          else
          {
            roomDetection[0].GetComponent<RoomType>().RoomDestruction();
            int randomBottomRoom = Random.Range(1, 4);
            if (randomBottomRoom == 2)
            {
              randomBottomRoom = 1;
            }
            Instantiate(rooms[randomBottomRoom], transform.position, Quaternion.identity);
          }

        Vector3 newPos = new Vector3(transform.position.x, 0, transform.position.z - moveAmount);
        transform.position = newPos;

        int rand = Random.Range(2, 4);
        Instantiate(rooms[rand], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
      }
      else
      {
        //   STOP
        stopGeneration = true;
        StartCoroutine(CloseLoadingScreen());
      }
    }
  }

  public void ToggleKeyboard()
  {
    if (useMobileController == true)
    {
      useMobileController = false;
    }
  }

  public void ToggleTouch()
  {
    if (useMobileController == false)
    {
      useMobileController = true;
    }
  }

  public void PauseGame()
  {
    Time.timeScale = 0f;
    pauseMenu.SetActive(true);
  }

  public void ResumeGame()
  {
    Time.timeScale = 1f;
    pauseMenu.SetActive(false);
  }

  public void QuitToMain()
  {
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  public void QuitGame()
  {
    Application.Quit();
  }

}
