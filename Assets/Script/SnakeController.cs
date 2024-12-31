using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(20, 20);
    public Vector2 direction = Vector2.right;
    public GameObject bodyPrefab;
    private List<Transform> snakeBody = new List<Transform>();
    private List<Vector3> previousPositions = new List<Vector3>(); // Danh sách vị trí lịch sử
    private float moveDelay = 0.2f;
    private float timer;

    // Biến mới
    public GameObject gameOverCanvas;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    // Âm thanh
    public AudioClip eatSound;
    public AudioClip gameOverSound;
    private AudioSource audioSource;

    void Start()
    {
        snakeBody.Add(transform); // Thêm đầu rắn vào danh sách
        audioSource = GetComponent<AudioSource>();
        gameOverCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        MoveSnake();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
        {
            direction = Vector2.up;
            RotateHead(180);
        }
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
        {
            direction = Vector2.down;
            RotateHead(0);
        }
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
        {
            direction = Vector2.left;
            RotateHead(-90);
        }
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
        {
            direction = Vector2.right;
            RotateHead(90);
        }
    }

    void RotateHead(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void MoveSnake()
    {
        timer += Time.deltaTime;
        if (timer < moveDelay) return;

        timer = 0;

        // Lưu vị trí hiện tại của đầu rắn vào danh sách lịch sử
        previousPositions.Insert(0, transform.position);

        // Di chuyển đầu rắn
        transform.position += new Vector3(direction.x, direction.y, 0);

        // Di chuyển từng phần thân rắn
        for (int i = 1; i < snakeBody.Count; i++)
        {
            snakeBody[i].position = previousPositions[i - 1];
        }

        // Giới hạn danh sách vị trí lịch sử
        if (previousPositions.Count > snakeBody.Count)
        {
            previousPositions.RemoveAt(previousPositions.Count - 1);
        }
    }

    void GameOver()
    {
        Time.timeScale = 0; // Dừng thời gian
        gameOverCanvas.SetActive(true); // Hiển thị Game Over UI
    }

    public void Grow()
    {
        // Tạo phần thân mới
        GameObject newBodyPart = Instantiate(bodyPrefab);

        // Đặt phần thân mới tại vị trí cuối cùng của thân rắn
        newBodyPart.transform.position = snakeBody[snakeBody.Count - 1].position;

        // Thêm phần thân mới vào danh sách
        snakeBody.Add(newBodyPart.transform);

        // Thêm vị trí tương ứng vào danh sách vị trí lịch sử
        if (previousPositions.Count > 0)
        {
            previousPositions.Add(previousPositions[previousPositions.Count - 1]);
        }

        // Tăng tốc độ (giảm thời gian trễ)
        moveDelay = Mathf.Max(0.05f, moveDelay - 0.01f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            Grow();
            Destroy(collision.gameObject);
            FindObjectOfType<FoodSpawner>().SpawnFood();
            score += 10;
            scoreText.text = score.ToString();
            audioSource.PlayOneShot(eatSound);
        }
        else if (collision.CompareTag("Wall"))
        {
            audioSource.PlayOneShot(gameOverSound);
            GameOver();
        }
    }

    // Hàm xử lý nút Restart
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
