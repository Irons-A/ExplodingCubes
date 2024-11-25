using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CubeCore))]

public class Spawner : MonoBehaviour
{
    [SerializeField] private CubeCore _cubeToSpawn;

    private CubeCore _initialCube;

    public event Action<List<Rigidbody>> ObjectExploding;

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
            fragment.SetParametersOnSpawn(newExplosionPower, newExplosionRadius, newSplitChance);

            if (fragment.TryGetComponent(out Rigidbody theRB))
            {
                affectedObjects.Add(theRB);
            }
        }

        ObjectExploding?.Invoke(affectedObjects);
    }
}
