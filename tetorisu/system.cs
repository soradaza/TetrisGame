using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class system : MonoBehaviour
{
    const int FIELD_SIZE_X = 12;
    const int FIELD_SIZE_Y = 22;

    const int MOVE_SIZE_X = 5;
    const int MOVE_SIZE_Y = 5;
    

    public enum eBlockState
    {
        eNone,
        eFrame,
        eSkyBlue,
        eYellow,
        ePurple,
        eBlue,
        eOrange,
        eGreen,
        eRed,

        eMax

    };
    
    
    [SerializeField] GameObject _boardPrefab = null;

    //ブロックの実体
    private GameObject[,] _fieldBlocksObject = new GameObject[FIELD_SIZE_X, FIELD_SIZE_Y];
     private Block[,] _fieldBlocks = new Block[FIELD_SIZE_X, FIELD_SIZE_Y];
    
    //ブロックの状態
    private eBlockState[,] _fieldBlocksState = new eBlockState[FIELD_SIZE_X, FIELD_SIZE_Y];

    //最終的なブロックの状態
    private eBlockState[,] _fieldBlocksStateFinal = new eBlockState[FIELD_SIZE_X, FIELD_SIZE_Y];

    //動作中のブロック
    private eBlockState[,] _moveBlocksState = new eBlockState[MOVE_SIZE_X, MOVE_SIZE_Y];
    private int _moveBlockX =3;
    private int _moveBlockY =15;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < FIELD_SIZE_X; i++)
        {
            for(int j = 0; j < FIELD_SIZE_Y; j++)
            {
                GameObject newObject = GameObject.Instantiate<GameObject>(_boardPrefab);
                Block newBlock = newObject.GetComponent<Block>();
                //newBlock.SetState((0 < i && i < FIELD_SIZE_X - 1 && 0 < j && j < FIELD_SIZE_Y - 1) ? eBlockState.eFrame: FRAME_COLOR);
                newObject.transform.localPosition = new Vector3(i, j, 0.0f);
                _fieldBlocksObject[i, j] = newObject;
                _fieldBlocks[i, j] = newBlock;
                //ブロックの状態
                _fieldBlocksState[i, j] = (0 < i && i < FIELD_SIZE_X - 1 && 0 < j && j < FIELD_SIZE_Y - 1) ? eBlockState.eFrame: eBlockState.eNone;
                //newBlock.SetState(_fieldBlocksState[i, j]);
                _fieldBlocksStateFinal[i, j] =_fieldBlocksState[i, j];
            }
        }
        StartMove();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < FIELD_SIZE_X; i++)
        {
            for(int j = 0; j < FIELD_SIZE_Y; j++)
            {
                //ブロックの状態
               _fieldBlocksStateFinal[i, j] =_fieldBlocksState[i, j];
                //_fieldBlocks[i, j].SetState(fieldBlocksStateFinal[i, j]);
            }
        }
        for(int i = 0; i < MOVE_SIZE_X; i++)
        {
            for(int j = 0; j < MOVE_SIZE_Y; j++)
            {
                //ブロックの状態
               
                _fieldBlocksStateFinal[_moveBlockX + i, _moveBlockY + j] =_moveBlocksState[i, j];
            }
        }
        for(int i = 0; i < FIELD_SIZE_X; i++)
        {
            for(int j = 0; j < FIELD_SIZE_Y; j++)
            {
                //ブロックの状態
               
                _fieldBlocks[i, j].SetState(_fieldBlocksStateFinal[i, j]);
            }
        }
    }

    void StartMove()
    {
        for(int i = 0; i < FIELD_SIZE_X; i++)
        {
            for(int j = 0; j < FIELD_SIZE_Y; j++)
            {
                _moveBlocksState[i, j] = eBlockState.eBlue;
            }
        }
    }
}
