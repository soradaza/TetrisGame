using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockNext : MonoBehaviour
{
    [SerializeField] GameObject _blockPrefab = null;

    //ブロックの実体
    private GameObject[,] _fieldBlocksObject = new GameObject[system.MOVE_SIZE_Y, system.MOVE_SIZE_X];
    private Block[,] _fieldBlocks = new Block[system.MOVE_SIZE_Y, system.MOVE_SIZE_X];

    //ブロックの状態
    private system.eBlockState[,] _fieldBlocksState = new system.eBlockState[system.MOVE_SIZE_Y, system.MOVE_SIZE_X];
    // Start is called before the first frame update
    void Start()
    {
        int nx = system.MOVE_SIZE_X;
        int ny = system.MOVE_SIZE_Y;

    //初期設定
        for(int i = 0; i < ny; i++)
        {
            for(int j = 0; j < nx; j++)
            {
                GameObject newObject = GameObject.Instantiate<GameObject>(_blockPrefab);
                Block newBlock = newObject.GetComponent<Block>();
                newObject.transform.localPosition = new Vector3(j, i, 0.0f);
                _fieldBlocksObject[i, j] = newObject;
                _fieldBlocks[i, j] = newBlock;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
