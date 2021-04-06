using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private ParticleSystem particles;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        // Reset the particle system after it finished and set game object inactive
        if (particles.isPlaying) return;
        particles.Clear();
        gameObject.SetActive(false);
    }
}
