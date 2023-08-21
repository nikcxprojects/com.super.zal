using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    public GameObject[] _bonuses;

    private void Start()
    {
        var obj = _bonuses[Random.Range(0, _bonuses.Length)];
        Instantiate(obj, transform.position, transform.rotation);
    }
}
