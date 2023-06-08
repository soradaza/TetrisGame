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
    private GameObject[,] _fieldBlocksObject = new GameObject[FIELD_SIZE_Y, FIELD_SIZE_X];
    private Block[,] _fieldBlocks = new Block[FIELD_SIZE_Y, FIELD_SIZE_X];
    
    //ブロックの状態
    private eBlockState[,] _fieldBlocksState = new eBlockState[FIELD_SIZE_Y, FIELD_SIZE_X];

    //最終的なブロックの状態
    private eBlockState[,] _fieldBlocksStateFinal = new eBlockState[FIELD_SIZE_Y, FIELD_SIZE_X];

    //動作中のブロック
    private eBlockState[,] _moveBlocksState = new eBlockState[MOVE_SIZE_Y, MOVE_SIZE_X];
    private int _moveBlockX =3;
    private int _moveBlockY =15;
    static readonly Color DEFAULT_COLOR = new Color(1.0f, 1.0f, 1.0f);
    static readonly Color FRAME_COLOR = new Color(0.1f, 0.1f, 0.1f);
    //static readonly ColowBlock = newObject.GetComponent<Block>();
    //             newBlock.SetColor((0<j && j< FIELD_SIZE_X - 1) ? DEFAULT_COLOR : FRAME_COLOR);
                //newBlor FRAME_COLOR = new Color(0.1f, 0.1f, 0.1f);
    private float _FallTime = 1.0f;
    private float _FallTimer = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        //初期設定
        for(int i = 0; i < FIELD_SIZE_Y; i++)
        {
            for(int j = 0; j < FIELD_SIZE_X; j++)
            {
                GameObject newObject = GameObject.Instantiate<GameObject>(_boardPrefab);
                Block newBlock = newObject.GetComponent<Block>();
                //newBlock.SetColor((0 < i && i < FIELD_SIZE_Y - 1 && 0 < j && j < FIELD_SIZE_X - 1) ? eBlockState.eFrame: FRAME_COLOR);
                //Block neck.SetState((0 < i && i < FIELD_SIZE_X - 1 && 0 < j && j < FIELD_SIZE_Y - 1) ? eBlockState.eFrame: FRAME_COLOR);
                newObject.transform.localPosition = new Vector3(j, i, 0.0f);
                _fieldBlocksObject[i, j] = newObject;
                _fieldBlocks[i, j] = newBlock;
                // _fieldBlocks[i, j] = newObject;
                
                //ブロックの状態
                _fieldBlocksState[i, j] = (0 < i && i < FIELD_SIZE_Y - 1 && 0 < j && j < FIELD_SIZE_X - 1) ? eBlockState.eNone: eBlockState.eFrame;
                //newBlock.SetState(_fieldBlocksState[i, j]);
                _fieldBlocksStateFinal[i, j] =_fieldBlocksState[i, j];
            }
        }
        StartMove();
    }

    // Update is called once per frame
    void Update()
    { 
        _FallTimer -= Time.deltaTime;
        if(_FallTimer <= 0.0f || Input.GetKey(KeyCode.D))
        {
            //ブロックあたり判定下のやつ
            bool isCollision = CheckCollision(0,-1);
            if(isCollision)
            {

            }else{
                //落下
                _moveBlockY--;
                _FallTimer = _FallTime;
            }
            
        }
        UpdateBlockState();
    }

    //ブロックのあたり判定
    bool CheckCollision(int offsetX, int offSetY)
    {
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                //ブロックの状態
                if(_fieldBlocksState[_moveBlockY + i + offSetY, _moveBlockX + j + offsetX] != eBlockState.eNone && _moveBlocksState[i, j] != eBlockState.eNone)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //ブロックの状態を更新
    void UpdateBlockState()
    {
        for(int i = 0; i < FIELD_SIZE_Y; i++)
        {
            for(int j = 0; j < FIELD_SIZE_X; j++)
            {
                //ブロックの状態
               _fieldBlocksStateFinal[i, j] =_fieldBlocksState[i, j];
                //_fieldBlocks[i, j].SetState(fieldBlocksStateFinal[i, j]);
            }
        }
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                //ブロックの状態
               
                _fieldBlocksStateFinal[_moveBlockY + i, _moveBlockX + j] =_moveBlocksState[i, j];
            }
        }
        for(int i = 0; i < FIELD_SIZE_Y; i++)
        {
            for(int j = 0; j < FIELD_SIZE_X; j++)
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
