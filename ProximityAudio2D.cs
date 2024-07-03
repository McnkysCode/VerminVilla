using UnityEngine;

public class ProximityAudio2D : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform player;
    public float maxDistance = 20f;
    public float minDistance = 1f;

    void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        if (distance < minDistance)
        {
            audioSource.volume = 1f;
        }
        else if (distance > maxDistance)
        {
            audioSource.volume = 0f;
        }
        else
        {
            float t = (distance - minDistance) / (maxDistance - minDistance);
            audioSource.volume = 1f - t;
        }
    }
}
