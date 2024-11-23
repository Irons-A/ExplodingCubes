using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]

public class CubeCore : MonoBehaviour
{
    public event Action<int, float, Vector3> ObjectSplitting;

    [SerializeField] private int _minFragmentSpawnAmount = 2;
    [SerializeField] private int _maxFragmentSpawnAmount = 6;
    [SerializeField] private float _scaleMinifier = 2f;
    [SerializeField] private float _splitChanceMinifier = 2f;
    [SerializeField] private float _splitChance = 100f;

    private float _randomFloor = 0f;
    private float _randomCeiling = 100f;

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

            ObjectSplitting?.Invoke(fragmentCount, newSplitChance, newScale);
        }

        Destroy(gameObject);
    }

    public void SetSplitChance(float newSplitChance)
    {
        _splitChance = newSplitChance;
    }
}
