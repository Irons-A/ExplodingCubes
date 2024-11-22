using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private CubeCore _cubeCore;
    [SerializeField] private GameObject _cubeToSpawn;

    private void Awake()
    {
        if (TryGetComponent(out CubeCore component))
        {
            _cubeCore = component;
        }
    }

    private void OnEnable()
    {
        _cubeCore.ObjectSplitting += SpawnFragments;
    }

    private void OnDisable()
    {
        _cubeCore.ObjectSplitting -= SpawnFragments;
    }

    private List<Rigidbody> SpawnFragments(int minFragmentSpawnAmount, int maxFragmentSpawnAmount, float scaleMinifier, float splitChance, float splitChanceMinifier)
    {
        int fragmentCount = Random.Range(minFragmentSpawnAmount, maxFragmentSpawnAmount + 1);
        float newSplitChance = splitChance / splitChanceMinifier;
        List<Rigidbody> affectedObjects = new();

        for (int i = 0; i < fragmentCount; i++)
        {
            GameObject fragment = Instantiate(_cubeToSpawn);
            fragment.transform.localScale = transform.localScale / scaleMinifier;

            if (TryGetComponent(out CubeCore theCC))
            {
                theCC.SetSplitChance(newSplitChance);
            }

            if (TryGetComponent(out MeshRenderer theMR))
            {
                theMR.material.color = Random.ColorHSV();
            }

            if (fragment.TryGetComponent(out Rigidbody theRB))
            {
                affectedObjects.Add(theRB);
            }
        }

        return affectedObjects;
    }
}
