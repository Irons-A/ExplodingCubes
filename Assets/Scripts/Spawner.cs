using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CubeCore))]

public class Spawner : MonoBehaviour
{
    public event Action<List<Rigidbody>> ObjectExploding;

    [SerializeField] private CubeCore _cubeToSpawn;

    private CubeCore _initialCube;

    private void Awake()
    {
        if (TryGetComponent(out CubeCore component))
        {
            _initialCube = component;
        }
    }
    private void OnEnable()
    {
        _initialCube.ObjectSplitting += SpawnFragments;
    }
    private void OnDisable()
    {
        _initialCube.ObjectSplitting -= SpawnFragments;
    }

    private void SpawnFragments(int fragmentCount, float newSplitChance, Vector3 newScale, float newExplosionPower, float newExplosionRadius)
    {
        List<Rigidbody> affectedObjects = new();

        for (int i = 0; i < fragmentCount; i++)
        {
            CubeCore fragment = Instantiate(_cubeToSpawn);
            fragment.transform.localScale = newScale;
            fragment.SetSplitChance(newSplitChance);
            fragment.SetExplosionPower(newExplosionPower, newExplosionRadius);

            if (fragment.TryGetComponent(out Rigidbody theRB))
            {
                affectedObjects.Add(theRB);
            }
        }

        ObjectExploding?.Invoke(affectedObjects);
    }
}
