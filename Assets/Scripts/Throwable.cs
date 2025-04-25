using UnityEngine;
using System.Collections;

public class Throwable : MonoBehaviour
{
    public GameObject ballPrefab;      // Reference to the ball prefab
    public GameObject rocketPrefab;    // Reference to the rocket prefab
    public GameObject bombPrefab;      // Reference to the bomb prefab
    public GameObject knifePrefab;     // Reference to the knife prefab
    public GameObject surikenPrefab;   // Reference to the suriken prefab

    public Trajectory trajectory;      // Reference to the Trajectory script
    private GameObject currentThrowable;      // Reference to the currently active throwable object
    private int selectedThrowableIndex = 0;     // Tracks the selected throwable (0 = ball, 1 = rocket, 2 = bomb, 3 = knife, 4 = suriken)
    private int previousThrowableIndex = -1;    // Tracks the previously selected throwable
    private bool isSpawning = false;            // Tracks whether a new throwable is being spawned

    void Start()
    {
        StartCoroutine(SpawnNewThrowableWithDelay(0.5f));
    }

    void Update()
    {
        HandleScrollInput();

        if (Input.GetMouseButtonDown(0) && currentThrowable != null)
        {
            // Start dragging
            Vector2 startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            trajectory.StartDragging(startPoint);
        }

        if (Input.GetMouseButton(0) && currentThrowable != null)
        {
            // Update trajectory preview
            Vector2 endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            trajectory.UpdateDragging(endPoint);
        }

        if (Input.GetMouseButtonUp(0) && currentThrowable != null)
        {
            // Launch the throwable object
            trajectory.Launch();
            StartCoroutine(SpawnNewThrowableWithDelay(0.5f));
        }
    }

    private void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) // Scroll up
        {
            selectedThrowableIndex = (selectedThrowableIndex + 1) % 5; // Cycle through 0, 1, 2, 3, 4
        }
        else if (scroll < 0f) // Scroll down
        {
            selectedThrowableIndex = (selectedThrowableIndex - 1 + 5) % 5; // Cycle through 4, 3, 2, 1, 0
        }

        // Only change the throwable if the selection has changed
        if (selectedThrowableIndex != previousThrowableIndex)
        {
            previousThrowableIndex = selectedThrowableIndex;
            ChangeThrowable(); // Change the throwable object

            // Debug log for selected throwable
            string selectedThrowable = selectedThrowableIndex switch
            {
                0 => "Ball",
                1 => "Rocket",
                2 => "Bomb",
                3 => "Knife",
                4 => "Suriken",
            };
            Debug.Log($"{selectedThrowable} Selected");
        }
    }

    private IEnumerator SpawnNewThrowableWithDelay(float delay)
    {
        if (isSpawning) yield break; // Prevent multiple coroutines from running
        isSpawning = true;

        yield return new WaitForSeconds(delay);

        // Instantiate a new throwable object based on the selected type
        if (selectedThrowableIndex == 0)
        {
            currentThrowable = Instantiate(ballPrefab); // Ball
        }
        else if (selectedThrowableIndex == 1)
        {
            currentThrowable = Instantiate(rocketPrefab); // Rocket
        }
        else if (selectedThrowableIndex == 2)
        {
            currentThrowable = Instantiate(bombPrefab); // Bomb
        }
        else if (selectedThrowableIndex == 3)
        {
            currentThrowable = Instantiate(knifePrefab); // Knife
        }
        else if (selectedThrowableIndex == 4)
        {
            currentThrowable = Instantiate(surikenPrefab); // Suriken
        }

        // Set the Rigidbody2D to Kinematic to prevent it from falling
        Rigidbody2D throwableRigidbody = currentThrowable.GetComponent<Rigidbody2D>();
        throwableRigidbody.bodyType = RigidbodyType2D.Kinematic;

        // Pass the current throwable object to the Trajectory script
        trajectory.SetCurrentProjectile(currentThrowable);

        isSpawning = false; // Allow new coroutines to run
    }

    void ChangeThrowable()
    {
        if (currentThrowable != null)
        {
            Destroy(currentThrowable); // Destroy the current throwable object
        }
        StartCoroutine(SpawnNewThrowableWithDelay(0.5f)); // Spawn a new one
    }
}
