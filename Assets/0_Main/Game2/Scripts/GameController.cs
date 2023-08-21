using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [Serializable]
    private class Enemy
    {
        [SerializeField] private Color _color;
        public Color Color => _color;

        [SerializeField] private ColorState _state;

        public ColorState State => _state;

        public Enemy(ColorState state)
        {
            _state = state;
        }
    }
    
    [SerializeField] private List<Enemy> _enemies = new List<Enemy>(
        new Enemy[]
        {
            new Enemy(ColorState.Black),
            new Enemy(ColorState.White)
        });
    
    [SerializeField] private Vector2 boundary;
    [SerializeField] private float startWait;
    [SerializeField] private float spawnWait;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Button _buttonChange;

    [SerializeField] private UnityEvent onLose;
    
    private bool gameOver;
    public static ColorState color;

    private void Start()
    {
        OnClickButtonChange();
        _buttonChange.onClick.AddListener(() =>
        {
            OnClickButtonChange();
        });
    }

    public void StartNewGame()
    {
        StartCoroutine(SpawnEnemies());
        color = ColorState.Black;
        player.gameObject.SetActive(true);
        GameOver(false);
    }

    private void OnClickButtonChange()
    {
        var color = ChangeColor();
        _buttonChange.image.color = color;
        _buttonChange.transform.Find("Text").GetComponent<Text>().color = GetReverseColor(color);
    }
    
    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds (startWait);

        while (true) {
            var spawnPosition = new Vector3 (
                Random.Range (-boundary.x, boundary.x),
                boundary.y,
                player.transform.position.z);
			
            var spawnRotation = Quaternion.identity;

            var obj = Instantiate (_prefab, spawnPosition, spawnRotation);
            obj.transform.parent = transform;
            var randomEnemy = _enemies[Random.Range(0, _enemies.Count)];
            obj.GetComponent<SpriteRenderer>().color = randomEnemy.Color;
            obj.tag = (((int) randomEnemy.State) + 1).ToString();
            yield return new WaitForSeconds (spawnWait);
            
            if (gameOver) break;
        }
    }

    private int _currentColor;

    private Color ChangeColor()
    {
        _currentColor = _currentColor == 0 ? 1 : 0;
        color = (ColorState) _currentColor;
        player.UpdateColor(GetByEnum(color));
        return GetByEnum(color);
    }

    public void GameOver(bool value)
    {
        if (value)
        {
            StopAllCoroutines();
            StartCoroutine(Lose());
        }
        player.SetActive(!value);
        gameOverUI.SetActive(value);
    }

    private IEnumerator Lose()
    {
        yield return new WaitForSeconds(2);
        onLose.Invoke();
    }

    private Color GetByEnum(ColorState state)
    {
        foreach (var enemy in _enemies.Where(enemy => enemy.State == state)) return enemy.Color;
        return Color.black;
    }

    private Color GetReverseColor(Color currentColor)
    {
        return currentColor == Color.black ? Color.white : Color.black;
    }
}

public enum ColorState
{
    Black = 1,
    White = 2
}
