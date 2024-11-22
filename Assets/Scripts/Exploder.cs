using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private CubeCore _cubeCore;

    private void Awake()
    {
        if (TryGetComponent(out CubeCore component))
        {
            _cubeCore = component;
        }
    }

    private void OnEnable()
    {
        _cubeCore.ObjectExploding += Explode;
    }

    private void OnDisable()
    {
        _cubeCore.ObjectExploding -= Explode;
    }

    private void Explode(float splitChance, float explosionPower, float explosionRadius, List<Rigidbody> SpawnFragments)
    {
        float splitChanceGamble = Random.Range(1f, 100f);

        if (splitChanceGamble <= splitChance)
        {
            foreach (Rigidbody fragment in SpawnFragments)
            {
                fragment.AddExplosionForce(explosionPower, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);
    }
}
