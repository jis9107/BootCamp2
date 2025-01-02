using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpanwer : MonoBehaviour
{
    public GameObject itemPrefab;
    
    public ItemData[] itemData;

    public float minSpawnTime;
    public float maxSpawnTime;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnItemCallback();
    }

    IEnumerator SpawnItem()
    {
        float nextRandomTime = Random.Range(minSpawnTime, maxSpawnTime);
        
        yield return new WaitForSeconds(nextRandomTime);
        
        SpawnItemCallback();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void SpawnItemCallback()
    {
        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        SpwanedItem spawnedItem = item.GetComponent<SpwanedItem>();
        int index = Random.Range(0, itemData.Length);
        spawnedItem.SetItemData(itemData[index]);
        
        // 익명함수 , 델리게이트 하나
        item.GetComponent<SpwanedItem>().OnDestroiedAction += () =>
        {
            StartCoroutine(SpawnItem());
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}