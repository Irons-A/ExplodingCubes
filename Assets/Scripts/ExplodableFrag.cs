using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableFrag : MonoBehaviour
{
    public float SplitChance = 100f;

    [SerializeField] private int _minFragmentSpawnAmount = 2;
    [SerializeField] private int _maxFragmentSpawnAmount = 7;
    [SerializeField] private float _explosionRadius = 100;
    [SerializeField] private float _explosionPower = 100;
    [SerializeField] private float _scaleMinifier = 2f;
    [SerializeField] private float _splitChanceMinifier = 2f;

    [SerializeField] private GameObject _fragment;

    private void OnMouseUpAsButton()
    {
        Explode();
    }

    private void Explode()
    {
        float splitChanceGamble = Random.Range(1f, 100f);

        if (splitChanceGamble <= SplitChance)
        {
            foreach (Rigidbody fragment in SpawnFragments())
            {
                fragment.AddExplosionForce(_explosionPower, transform.position, _explosionRadius);
            }
        }

        Destroy(gameObject);
    }

    private List<Rigidbody> SpawnFragments()
    {
        int fragmentCount = Random.Range(_minFragmentSpawnAmount, _maxFragmentSpawnAmount);
        List<Rigidbody> affectedObjects = new();

        for (int i = 0; i < fragmentCount; i++)
        {
            GameObject fragment = Instantiate(_fragment);
            ExplodableFrag fragmentRoot = fragment.GetComponent<ExplodableFrag>();
            MeshRenderer fragmentMR = fragment.GetComponent<MeshRenderer>();

            fragment.transform.localScale = transform.localScale / _scaleMinifier;
            fragmentRoot.SplitChance = SplitChance / _splitChanceMinifier;
            fragmentMR.material.color = Random.ColorHSV();

            if (fragment.TryGetComponent(out Rigidbody component))
            {
                affectedObjects.Add(component);
            }
        }

        return affectedObjects;
    }
}
