using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CubeCore))]

public class Exploder : MonoBehaviour
{
    private Spawner _spawner;
    private CubeCore _cubeCore;

    private void Awake()
    {
        _spawner = FindObjectOfType<Spawner>();
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

    private void Explode(CubeCore cubeCore)
    {
        foreach (Rigidbody affectedObject in cubeCore.affectedObjects)
        {
            affectedObject.AddExplosionForce(cubeCore.explosionPower, transform.position, cubeCore.explosionRadius);
        }
    }
}
