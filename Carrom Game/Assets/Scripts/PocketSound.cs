using UnityEngine;

public class PocketSound : MonoBehaviour
{
    public AudioClip pocketCollisionSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        audioSource.PlayOneShot(pocketCollisionSound);
    }
}