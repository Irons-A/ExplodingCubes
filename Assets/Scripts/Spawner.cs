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

    private void SpawnFragments(CubeCore cubeCore)
    {
        List<Rigidbody> affectedObjects = new();

        for (int i = 0; i < cubeCore.fragmentCount; i++)
        {
            CubeCore fragment = Instantiate(_cubeToSpawn);
            fragment.transform.localScale = cubeCore.newScale;
            fragment.SetParametersOnSpawn(cubeCore.newExplosionPower, cubeCore.newExplosionRadius, cubeCore.newSplitChance);
            affectedObjects.Add(cubeCore.rigidBody);
        }

        ObjectExploding?.Invoke(affectedObjects);
    }
}
