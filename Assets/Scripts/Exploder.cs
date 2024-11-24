using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spawner))]
[RequireComponent(typeof(CubeCore))]

public class Exploder : MonoBehaviour
{
    private Spawner _spawner;
    private CubeCore _cubeCore;

    private void Awake()
    {
        _spawner = GetComponent<Spawner>();
        _cubeCore = GetComponent<CubeCore>();
    }
    private void OnEnable()
    {
        _spawner.ObjectExploding += PushFragments;
        _cubeCore.TriggeringFinalExplosion += Explode;
    }
    private void OnDisable()
    {
        _spawner.ObjectExploding -= PushFragments;
        _cubeCore.TriggeringFinalExplosion -= Explode;
    }

    private void PushFragments(List<Rigidbody> SpawnFragments)
    {
        foreach (Rigidbody fragment in SpawnFragments)
        {
            fragment.AddForce(Random.insideUnitSphere, ForceMode.Force);
        }
    }

    private void Explode(List<Rigidbody> affectedObjects, float explosionPower, float explosionRadius)
    {
        foreach (Rigidbody affectedObject in affectedObjects)
        {
            affectedObject.AddExplosionForce(explosionPower, transform.position, explosionRadius);
        }
    }
}
