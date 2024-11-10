using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacerAVP : MonoBehaviour
{
    public GameObject blockPrefab;
    private GameObject placedBlocks;
    private MusicBlock[] musicBlocks;
    private bool isshowing = false;


    public void Start()
    {

    }

    public void PlaceBlocks()
    {
        musicBlocks = InitializeMusicBlocks();
        placedBlocks = new GameObject("placedBlocks");

        foreach (MusicBlock block in musicBlocks)
        {
            Vector3 position = block.GetPosition();
            Vector3 direction = position - MusicBlock.origin; // 从原点到方块位置的方向
            Quaternion rotation = Quaternion.LookRotation(direction); // 计算旋转
            GameObject placedblock = Instantiate(blockPrefab, position, rotation);
            placedblock.transform.parent = placedBlocks.transform;
        }

        isshowing = true;
    }

    public void DestroyBlocks()
    {
        if (placedBlocks != null)
        {
            isshowing = false;
            Destroy(placedBlocks);
            placedBlocks = null;
        }
        else
        {
            Debug.Log("No placedblocks yet");
        }
    }

    public void ShowBlocks()
    {
        StartCoroutine(PlaceandDestroyBlocks());
    }

    public IEnumerator PlaceandDestroyBlocks()
    {
        if (isshowing)
        {
            Debug.Log("Blocks are already showing");
            yield break;
        }
        else
        {
            PlaceBlocks();

            // 等待
            yield return new WaitForSeconds(5f);

            DestroyBlocks();
        }
    }

    public MusicBlock[] InitializeMusicBlocks()
    {
        int totalBlocks = 18;
        float initialStartTime = 5.0f;
        float lifespan = 2.0f;

        MusicBlock[] musicBlocks = new MusicBlock[totalBlocks];

        for (int i = 0; i < totalBlocks; i++)
        {
            musicBlocks[i] = new MusicBlock();

            if (i == 0)
            {
                musicBlocks[i].starttime = initialStartTime;
            }
            else
            {
                musicBlocks[i].starttime = musicBlocks[i - 1].starttime + musicBlocks[i - 1].lifespan;
            }

            musicBlocks[i].lifespan = lifespan;
            musicBlocks[i].gridYaw = i % 6;
            musicBlocks[i].gridPitch = i / 6;
        }

        return musicBlocks;
    }

}
