using UnityEngine;
using UnityEngine.SceneManagement;

public class Trajectory : MonoBehaviour
{
    public LineRenderer trajectoryLine; // LineRenderer for trajectory
    public float launchForce = 10f;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private GameObject currentProjectile; // Reference to the currently active projectile
    private Scene simulationScene;
    private PhysicsScene2D physicsScene; // Physics Scene for simulation

    // Common flag to determine if the object is a ghost
    public  static bool IsGhost { get; set; } = false;

    private void Start()
    {
        CreatePhysicsScene();
    }

    public void SetCurrentProjectile(GameObject projectile)
    {
        currentProjectile = projectile;
    }

    public void StartDragging(Vector2 start)
    {
        startPoint = start;
    }

    public void UpdateDragging(Vector2 end)
    {
        endPoint = end;
        ShowTrajectory();
    }

    public void Launch()
    {
        if (currentProjectile == null) return;

        IsGhost = false; // Reset the ghost flag

        Rigidbody2D projectileRigidbody = currentProjectile.GetComponent<Rigidbody2D>();
        Vector2 direction = (startPoint - endPoint).normalized;
        float distance = Vector2.Distance(startPoint, endPoint);

        // Set the initial rotation of the projectile to align with the launch direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentProjectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        projectileRigidbody.bodyType = RigidbodyType2D.Dynamic; // Enable physics
        projectileRigidbody.AddForce(direction * distance * launchForce, ForceMode2D.Impulse);

        // Clear the reference to the current projectile
        currentProjectile = null;
        trajectoryLine.positionCount = 0; // Clear trajectory
    }

    private void ShowTrajectory()
    {
        if (currentProjectile == null) return;

        // Clone the projectile into the physics scene
        GameObject ghostProjectile = Instantiate(currentProjectile, new Vector2(-6,-3), Quaternion.identity);
        ghostProjectile.GetComponent<Renderer>().enabled = false; // Hide the ghost object
        SceneManager.MoveGameObjectToScene(ghostProjectile, simulationScene);

        IsGhost  = true; // Set the ghost flag

        Rigidbody2D ghostRigidbody = ghostProjectile.GetComponent<Rigidbody2D>();
        Vector2 direction = (startPoint - endPoint).normalized;
        float distance = Vector2.Distance(startPoint, endPoint);
        Vector2 velocity = direction * distance * launchForce;

        // Set the initial rotation of the projectile to align with the launch direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentProjectile.transform.rotation = Quaternion.Euler(0, 0, angle);


        ghostRigidbody.bodyType = RigidbodyType2D.Dynamic;
        ghostRigidbody.AddForce(direction * distance * launchForce, ForceMode2D.Impulse);


        trajectoryLine.positionCount = 100; // Number of points in the trajectory
        for (int i = 0; i < 100; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime); // Simulate physics in small time steps
            trajectoryLine.SetPosition(i, ghostRigidbody.position);
        }

        Destroy(ghostProjectile); // Clean up the ghost object
    }

    private void CreatePhysicsScene()
    {
        // Create a new physics scene for simulation
        simulationScene = SceneManager.CreateScene("TrajectorySimulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene = simulationScene.GetPhysicsScene2D();

        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Ground"))
        {
            GameObject ghostObstacle = Instantiate(obstacle, obstacle.transform.position, obstacle.transform.rotation);
            ghostObstacle.GetComponent<Renderer>().enabled = false; // Hide the ghost object
            SceneManager.MoveGameObjectToScene(ghostObstacle, simulationScene); // Move to simulation scene
        }
    }

}
