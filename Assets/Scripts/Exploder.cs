using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spawner))]

public class Exploder : MonoBehaviour
{
    private Spawner _spawner;

    private void Awake()
    {
        _spawner = GetComponent<Spawner>();
    }
    private void OnEnable()
    {
        _spawner.ObjectExploding += Explode;
    }
    private void OnDisable()
    {
        _spawner.ObjectExploding -= Explode;
    }

    private void Explode(List<Rigidbody> SpawnFragments)
    {
        foreach (Rigidbody fragment in SpawnFragments)
        {
            fragment.AddForce(Random.insideUnitSphere, ForceMode.Force);
        }
    }
}
