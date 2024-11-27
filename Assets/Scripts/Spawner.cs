using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private CubeCore _cubeToSpawn;

    private List<CubeCore> _initialCubes = new List<CubeCore>();

    public event Action<List<Rigidbody>> ObjectExploding;

    private void Awake()
    {
        _initialCubes = FindObjectsOfType<CubeCore>().ToList<CubeCore>();
    }
    private void OnEnable()
    {
        foreach (var item in _initialCubes)
        {
            item.ObjectSplitting += SpawnFragments;
        }
    }
    private void OnDisable()
    {
        foreach (var item in _initialCubes)
        {
            item.ObjectSplitting -= SpawnFragments;
        }
    }

    private void SpawnFragments(CubeCore cubeCore)
    {
        List<Rigidbody> affectedObjects = new();

        for (int i = 0; i < cubeCore.fragmentCount; i++)
        {
            CubeCore fragment = Instantiate(_cubeToSpawn, cubeCore.transform.position, cubeCore.transform.rotation);
            fragment.ObjectSplitting += SpawnFragments;
            _initialCubes.Add(fragment);
            fragment.transform.localScale = cubeCore.newScale;
            fragment.SetParametersOnSpawn(cubeCore.newExplosionPower, cubeCore.newExplosionRadius, cubeCore.newSplitChance);
            affectedObjects.Add(cubeCore.rigidBody);
        }

        ObjectExploding?.Invoke(affectedObjects);
    }
}
