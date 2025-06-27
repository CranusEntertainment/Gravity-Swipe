using UnityEngine;

public class EnemySpaceship : MonoBehaviour
{
    private Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float fireRate = 0.5f;
    public float bulletSpeed = 30f;

    public float ascendHeight = 10f;
    public float ascendSpeed = 5f;
    public float chargeSpeed = 15f;
    public float resetDistance = 3f;

    private float fireTimer;
    private Vector3 startPosition;
    private EnemyState currentState = EnemyState.Ascending;

    private enum EnemyState { Ascending, Charging }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player tag'li obje bulunamadı!");

        startPosition = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        LookAtPlayer();
        HandleState();
        HandleShooting();
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    void HandleState()
    {
        switch (currentState)
        {
            case EnemyState.Ascending:
                Ascend();
                break;
            case EnemyState.Charging:
                Charge();
                break;
        }
    }

    void Ascend()
    {
        Vector3 targetPos = new Vector3(transform.position.x, startPosition.y + ascendHeight, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, ascendSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentState = EnemyState.Charging;
        }
    }

    void Charge()
    {
        Vector3 target = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, chargeSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < resetDistance)
        {
            // Tekrar yukarı çıkmak için başlangıç yüksekliğine dön
            startPosition = transform.position;
            currentState = EnemyState.Ascending;
        }
    }

    void HandleShooting()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            FireBullet();
        }
    }

    void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;
    }
}
