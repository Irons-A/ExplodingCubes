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
    [SerializeField] private float _explosionPower = 100f;
    [SerializeField] private float _explosionRadius = 100f;
    [SerializeField] private float _explosionPowerMultiplier = 2f;

    private float _randomFloor = 0f;
    private float _randomCeiling = 100f;

    public event Action<int, float, Vector3, float, float> ObjectSplitting;
    public event Action<List<Rigidbody>, float, float> TriggeringFinalExplosion;

    private void Awake()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }

    private void OnMouseUpAsButton()
    {
        float splitChanceGamble = Random.Range(_randomFloor, _randomCeiling);

        if (splitChanceGamble <= _splitChance)
        {
            float newSplitChance = _splitChance / _splitChanceMinifier;
            int fragmentCount = Random.Range(_minFragmentSpawnAmount, _maxFragmentSpawnAmount + 1);
            Vector3 newScale = transform.localScale / _scaleMinifier;
            float newExplosionPower = _explosionPower * _explosionPowerMultiplier;
            float newExplosionRadius = _explosionRadius * _explosionPowerMultiplier;

            ObjectSplitting?.Invoke(fragmentCount, newSplitChance, newScale, newExplosionPower, newExplosionRadius);
        }
        else
        {
            TriggeringFinalExplosion?.Invoke(GetAffectedObjects(), _explosionPower, _explosionRadius);
        }

        Destroy(gameObject);
    }

    public void SetParametersOnSpawn(float newExplosionPower, float newExplosionRadius, float newSplitChance)
    {
        _explosionPower = newExplosionPower;
        _explosionRadius = newExplosionRadius;
        _splitChance = newSplitChance;
    }

    private List<Rigidbody> GetAffectedObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        List<Rigidbody> affectedObjects = new();

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                affectedObjects.Add(hit.attachedRigidbody);
            }
        }

        return affectedObjects;
    }
}
