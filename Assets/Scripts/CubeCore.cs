using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCore : MonoBehaviour
{
    public event Action<int, int, float, float, float> ObjectSplitting;
    public event Action<float, float, float, List<Rigidbody>> ObjectExploding;

    [SerializeField] private int _minFragmentSpawnAmount = 2;
    [SerializeField] private int _maxFragmentSpawnAmount = 6;
    [SerializeField] private float _explosionRadius = 100;
    [SerializeField] private float _explosionPower = 100;
    [SerializeField] private float _scaleMinifier = 2f;
    [SerializeField] private float _splitChanceMinifier = 2f;
    [SerializeField] private float _splitChance = 100f;

    [SerializeField] private GameObject _fragment;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Exploder _exploder;

    private void Awake()
    {
        if (TryGetComponent(out Spawner spawner))
        {
            _spawner = spawner;
        }

        if (TryGetComponent(out Exploder exploder))
        {
            _exploder = exploder;
        }
    }

    private void OnMouseUpAsButton()
    {
        List<Rigidbody> rigidbodies = ObjectSplitting?.Invoke(_minFragmentSpawnAmount, _maxFragmentSpawnAmount, _scaleMinifier, _splitChance, _splitChanceMinifier);
        ObjectExploding?.Invoke();
        Destroy(gameObject);
    }

    public void SetSplitChance(float newSplitChance)
    {
        _splitChance = newSplitChance;
    }
}
