using UnityEngine;

public class SingleBullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] int bulletSpeed = 200;

    [SerializeField] float lifeSpan = 2;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * bulletSpeed;
        Invoke("KillBullet", lifeSpan);
    }

    void KillBullet()
    {
        if (gameObject) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        KillBullet();
    }




}
