using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public Camera mainCamera;
    public InputAction clickAction;
    public int score;
    public int gems;
    public Vector2 bounds;
    public GameObject target;
    public GameObject gem;
    public float gemChance;
    public float timer;
    public TextMeshProUGUI UITimer;
    public TextMeshProUGUI UICurrentScore;
    public TextMeshProUGUI UITotalGems;
    public TextMeshProUGUI UIHighestScore;
    public bool reset = false;

    void Update()
    {
        UpdateText();
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            EndGame();
        }
        if (reset)
        {
            PlayerPrefs.SetInt("highscore", 0);
            PlayerPrefs.SetInt("gem", 0);
            reset = false;
        }
    }
    private void Awake()
    {
        // When click starts (pressed down)
        clickAction.started += OnClickStarted;
    }
    void UpdateText()
    {
        UIHighestScore.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highscore");
        UITotalGems.text = "GEMS: " + PlayerPrefs.GetInt("gem");
        UICurrentScore.text = "Score: " + score;
        UITimer.text = "Time Left: " + Mathf.Round(timer);
    }

    private void OnEnable()
    {
        clickAction.Enable();
    }

    private void OnDisable()
    {
        clickAction.Disable();
    }

    private void OnClickStarted(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldMousePos = mainCamera.ScreenToWorldPoint(mousePos);
        //Debug.Log(worldMousePos);
        RaycastHit2D ray = Physics2D.Raycast(worldMousePos, Vector3.forward, 1);
        if (ray)
        {
            Score(ray.collider.gameObject);
        }
    }

    private void Score(GameObject hitObject)
    {
        if (!hitObject) return;
        if (hitObject.tag == "gem")
        {
            score += 1;
            gems += 1;
            Destroy(hitObject);
            PlayerPrefs.SetInt("gem", PlayerPrefs.GetInt("gem") + 1);
            SpawnNext();
        }
        else if (hitObject.tag == "target")
        {
            score += 1;
            Destroy(hitObject);
            SpawnNext();
        }
    }
    private void SpawnNext()
    {
        float x = Random.Range(-bounds.x, bounds.x);
        float y = Random.Range(-bounds.y, bounds.y);
        if (Random.Range(0f, 1f) <= gemChance)
        {
            Instantiate(gem, new Vector3(x, y, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(target, new Vector3(x, y, 0), Quaternion.identity);
        }
    }
    private void EndGame()
    {
        if (score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
        }
        Application.Quit();
        this.enabled = false;
    }

}

