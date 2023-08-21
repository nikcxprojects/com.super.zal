using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private Vector3 _pointA = new Vector3(19f, -2, 1);
    private Vector3 _pointB = new Vector3(-19f, -2, 1);

    private SpriteRenderer _sprite;

    [SerializeField] private GameController gameController;
    [SerializeField] private CreditController creditController;
    [SerializeField] [Range(0, 100)] private int percentOfSpeed;

    private bool _movement;

    public void SetActive(bool value)
    {
        _movement = value;
        gameObject.SetActive(value);
    }

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }
    
    private float t = 0;
    void Update()
    {
        if(!_movement) return;
        t += Time.deltaTime * percentOfSpeed / 100;
        transform.position = Vector3.Lerp(_pointA, _pointB, Mathf.PingPong(t, 1));
    }

    public void UpdateColor(Color color)
    {
        _sprite.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (((int) GameController.color + 1) != Convert.ToInt32(collider.tag)) gameController.GameOver(true);
        else creditController.AddCredits(200);
        Destroy(collider.gameObject);
    }
}
