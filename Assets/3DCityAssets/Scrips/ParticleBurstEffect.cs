using UnityEngine;

public class ParticleBurstEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject particleEffectPrefab;

    [SerializeField]
    public bool shouldPlayOnAwake;

    private Vector3 position;

    [SerializeField]
    private Vector3 spawnRotation = Vector3.zero;

    void Awake()
    {
        if (shouldPlayOnAwake)
        {
            position = transform.position;
            TriggerDustEffect(position);
        }
    }
    public void TriggerDustEffect(Vector3 position)
    {
        if (particleEffectPrefab != null)
        {
            GameObject dustEffect = Instantiate(particleEffectPrefab, position, Quaternion.Euler(spawnRotation));

            ParticleSystem particleSystem = dustEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                Destroy(dustEffect, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(dustEffect, 2f);
            }
        }
    }
}
