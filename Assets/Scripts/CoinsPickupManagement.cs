using UnityEngine;

public class CoinsManagement : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;

    private bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
            Destroy(gameObject);
        }
    }
}
