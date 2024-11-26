using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]

public class CubeCore : MonoBehaviour
{
    [SerializeField] private int _minFragmentSpawnAmount = 2;
    [SerializeField] private int _maxFragmentSpawnAmount = 6;
    [SerializeField] private float _scaleMinifier = 2f;
    [SerializeField] private float _splitChanceMinifier = 2f;
    [SerializeField] private float _splitChance = 100f;
    [SerializeField] private float _explosionPowerMultiplier = 2f;

    private float _randomFloor = 0f;
    private float _randomCeiling = 100f;

    public event Action<CubeCore> ObjectSplitting;
    public event Action<CubeCore> TriggeringFinalExplosion;

    public float newSplitChance { get; private set; }
    public int fragmentCount { get; private set; }
    public Vector3 newScale { get; private set; }
    public float newExplosionPower { get; private set; }
    public float newExplosionRadius { get; private set; }
    public CubeCore cubeCore { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public List<Rigidbody> affectedObjects { get; private set; }
    public float explosionPower { get; private set; } = 100f;
    public float explosionRadius { get; private set; } = 100f;

    private void Awake()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
        cubeCore = GetComponent<CubeCore>();
        rigidBody = GetComponent<Rigidbody>();
        affectedObjects = new List<Rigidbody>();
    }

    private void OnMouseUpAsButton()
    {
        float splitChanceGamble = Random.Range(_randomFloor, _randomCeiling);

        if (splitChanceGamble <= _splitChance)
        {
            newSplitChance = _splitChance / _splitChanceMinifier;
            fragmentCount = Random.Range(_minFragmentSpawnAmount, _maxFragmentSpawnAmount + 1);
            newScale = transform.localScale / _scaleMinifier;
            newExplosionPower = explosionPower * _explosionPowerMultiplier;
            newExplosionRadius = explosionRadius * _explosionPowerMultiplier;

            ObjectSplitting?.Invoke(cubeCore);
        }
        else
        {
            GetAffectedObjects();
            TriggeringFinalExplosion?.Invoke(cubeCore);
        }

        Destroy(gameObject);
    }

    public void SetParametersOnSpawn(float newExplosionPower, float newExplosionRadius, float newSplitChance)
    {
        explosionPower = newExplosionPower;
        explosionRadius = newExplosionRadius;
        _splitChance = newSplitChance;
    }

    private void GetAffectedObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                affectedObjects.Add(hit.attachedRigidbody);
            }
        }
    }
}
