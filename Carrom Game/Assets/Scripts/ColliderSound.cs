using UnityEngine;

public class ColliderSound : MonoBehaviour
{
    public AudioClip puckCollisionSound, borderCollisionSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Black") || collision.gameObject.CompareTag("White") || collision.gameObject.CompareTag("Queen") || collision.gameObject.CompareTag("Striker"))
        {
            audioSource.PlayOneShot(puckCollisionSound);
        }
        else
        {
            audioSource.PlayOneShot(borderCollisionSound);
        }
    }
}