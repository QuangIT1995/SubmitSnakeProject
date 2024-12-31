using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public Vector2Int gridSize = new Vector2Int(20, 20);

    public void SpawnFood()
    {
        int x = Random.Range(-gridSize.x / 2, gridSize.x / 2);
        int y = Random.Range(-gridSize.y / 2, gridSize.y / 2);
        Instantiate(foodPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }

    private void Start()
    {
        SpawnFood();
    }
}
