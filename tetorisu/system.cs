using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class system : MonoBehaviour
{
    const int FIELD_SIZE_X = 12;
    const int FIELD_SIZE_Y = 22;

    public const int MOVE_SIZE_X = 5;
    public const int MOVE_SIZE_Y = 5;

    const int DEFAULT_MOVE_X = 3;
    const int DEFAULT_MOVE_Y = 15;
    

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

    }

    //水色
    static readonly int[,] BLOCKS_SKYBLUE = new int[MOVE_SIZE_X, MOVE_SIZE_Y]
    {
        { 0, 0, 0, 0, 0, },
        { 2, 2, 2, 2, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
    };

    //黄色
    static readonly int[,] BLOCKS_YELLOW = new int[MOVE_SIZE_X, MOVE_SIZE_Y]
    {
        { 0, 0, 0, 0, 0, },
        { 0, 3, 3, 0, 0, },
        { 0, 3, 3, 0, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
    };

    //紫
    static readonly int[,] BLOCKS_PURPLE = new int[MOVE_SIZE_X, MOVE_SIZE_Y]
    {
        { 0, 0, 0, 0, 0, },
        { 0, 0, 4, 0, 0, },
        { 0, 4, 4, 4, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
    };

    //青
    static readonly int[,] BLOCKS_BLUE = new int[MOVE_SIZE_X, MOVE_SIZE_Y]
    {
        { 0, 0, 0, 0, 0, },
        { 0, 5, 0, 0, 0, },
        { 0, 5, 5, 5, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
    };

    //オレンジ
    static readonly int[,] BLOCKS_ORANGE = new int[MOVE_SIZE_X, MOVE_SIZE_Y]
    {
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 6, 0, },
        { 0, 6, 6, 6, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
    };

    //緑
    static readonly int[,] BLOCKS_GREEN = new int[MOVE_SIZE_X, MOVE_SIZE_Y]
    {
        { 0, 0, 0, 0, 0, },
        { 0, 0, 7, 7, 0, },
        { 0, 7, 7, 0, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
    };

    //赤
    static readonly int[,] BLOCKS_RED = new int[MOVE_SIZE_X, MOVE_SIZE_Y]
    {
        { 0, 0, 0, 0, 0, },
        { 0, 8, 8, 0, 0, },
        { 0, 0, 8, 8, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
    };

    static readonly int[][,] BLOCKS_LIST = new int[(int)eBlockState.eMax - (int)eBlockState.eSkyBlue][,]
    {
        BLOCKS_SKYBLUE,
        BLOCKS_YELLOW,
        BLOCKS_PURPLE,
        BLOCKS_BLUE,
        BLOCKS_ORANGE,
        BLOCKS_GREEN,
        BLOCKS_RED,
    };

    
    
    [SerializeField] GameObject _blockPrefab = null;

    //ブロックの実体
    private GameObject[,] _fieldBlocksObject = new GameObject[FIELD_SIZE_Y, FIELD_SIZE_X];
    private Block[,] _fieldBlocks = new Block[FIELD_SIZE_Y, FIELD_SIZE_X];
    
    //ブロックの状態
    private eBlockState[,] _fieldBlocksState = new eBlockState[FIELD_SIZE_Y, FIELD_SIZE_X];

    //最終的なブロックの状態
    private eBlockState[,] _fieldBlocksStateFinal = new eBlockState[FIELD_SIZE_Y, FIELD_SIZE_X];

    //動作中のブロック
    private eBlockState[,] _moveBlocksState = new eBlockState[MOVE_SIZE_Y, MOVE_SIZE_X];
    private int _moveBlockX =DEFAULT_MOVE_X;
    private int _moveBlockY =DEFAULT_MOVE_Y;

    private eBlockState[,] _tempBlocksState = new eBlockState[MOVE_SIZE_Y, MOVE_SIZE_X];
    private float _FallTime = 1.0f;
    private float _FallTimer = 1.0f;

    //キー入力
    private Dictionary<KeyCode, int> _keyImputTimer = new Dictionary<KeyCode, int>();
    private bool GetKeyEx(KeyCode keyCode)
    {
        if(!_keyImputTimer.ContainsKey(keyCode))
        {
            _keyImputTimer.Add(keyCode, -1);
        }

        if(Input.GetKey(keyCode))
        {
            _keyImputTimer[keyCode]++;
        }
        else
        {
            _keyImputTimer[keyCode] = -1;
        }
        return (_keyImputTimer[keyCode] == 0 || _keyImputTimer[keyCode] >= 10);
    }

    //状態遷移
    enum GameState
    {
        Active,
        GameOver,

        Max
    }
    GameState _gameState = GameState.Active;

    // Start is called before the first frame update
    void Start()
    {
        //初期設定
        for(int i = 0; i < FIELD_SIZE_Y; i++)
        {
            for(int j = 0; j < FIELD_SIZE_X; j++)
            {
                // ブロックの実体
                GameObject newObject = GameObject.Instantiate<GameObject>(_blockPrefab);
                Block newBlock = newObject.GetComponent<Block>();
                newObject.transform.localPosition = new Vector3(j, i, 0.0f);
                _fieldBlocksObject[i, j] = newObject;
                _fieldBlocks[i, j] = newBlock;
                
                //ブロックの状態
                _fieldBlocksState[i, j] = (0 < i && i < FIELD_SIZE_Y - 1 && 0 < j && j < FIELD_SIZE_X - 1) ? eBlockState.eNone: eBlockState.eFrame;
                _fieldBlocksStateFinal[i, j] =_fieldBlocksState[i, j];
            }
            
        }

        // 最初の抽選
        LotNextBlock();
        
        // ブロックを開始
        StartMove();
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバー時は行わない
        if(_gameState != GameState.Active)
        {
            return ;
        }

        //左ボタンを押下したとき
        if(GetKeyEx(KeyCode.LeftArrow))
        {
            bool isCollision = CheckCollision(-1,0);
             if(!isCollision)
            {
                //左へ移動
                _moveBlockX--;
            }
        }
        //右ボタンを押下したとき
        if(GetKeyEx(KeyCode.RightArrow))
        {
            bool isCollision = CheckCollision(1,0);
             if(!isCollision)
            {
                //右へ移動
                _moveBlockX++;
            }
        }

        //ブロック右回転
        if(GetKeyEx(KeyCode.D))
        {
            //あたり判定
            bool isCollision = CheckCollisionRotateRight();//CheckCollision(1,0);
             if(!isCollision)
            {
                //回転
                RotateBlockRight();
            }
        }

        //ブロック左回転
        if(GetKeyEx(KeyCode.A))
        {
            bool isCollision = CheckCollisionRotateLeft();//CheckCollision(1,0);
             if(!isCollision)
            {
                //右へ移動
                RotateBlockLeft();
            }
        }
        

        _FallTimer -= Time.deltaTime;
        //一秒経ったときか下ボタンを押下したとき
        if(_FallTimer <= 0.0f || GetKeyEx(KeyCode.DownArrow))
        {
            //ブロックあたり判定下
            bool isCollision = CheckCollision(0,-1);
            if(isCollision)
            {
                //ブロックの落下判定
                MergeBlock();
                //ブロック揃い判定
                CheckLine();
                CheckLine();
                CheckLine();
                CheckLine();
                _FallTimer = _FallTime;
                StartMove();
            }else{
                //落下
                _moveBlockY--;
                _FallTimer = _FallTime;
            }
            
        }
        
        UpdateBlockState();
    }
    void CacheTempState()
    {
        //退避
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                _tempBlocksState[i, j] = _moveBlocksState[i, j];
            }
        }
    }
    
    // 退避から戻す
    void RestoreTempState()
    {
        for (int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for (int j = 0; j < MOVE_SIZE_X; j++)
            {
                _moveBlocksState[i, j] = _tempBlocksState[i, j];
            }
        }
    }

    //ブロックの回転処理
    void RotateBlockRight()
    {
        CacheTempState();

        //回転
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                _moveBlocksState[i, j] = _tempBlocksState[j, MOVE_SIZE_Y - 1 - i];
            }
        }
    }

    //ブロックの回転処理
    void RotateBlockLeft()
    {
        CacheTempState();

        //回転
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                _moveBlocksState[i, j] = _tempBlocksState[MOVE_SIZE_X - 1 - j, i];
            }
        }
    }

    //ブロックのあたり判定
    bool CheckCollision(int offsetX, int offSetY)
    {
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                //ブロックの状態
                if (0 <= _moveBlockY + i + offSetY && _moveBlockY + i + offSetY < FIELD_SIZE_Y && 0 <= _moveBlockX + j + offsetX && _moveBlockX + j + offsetX < FIELD_SIZE_X)
                {
                    if (_fieldBlocksState[_moveBlockY + i + offSetY, _moveBlockX + j + offsetX] != eBlockState.eNone && _moveBlocksState[i, j] != eBlockState.eNone)
                        {
                            return true;
                        }
                }
            }
        }
        return false;
    }

    //ブロックのあたり判定(回転)
    bool CheckCollisionRotate()
    {
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                //ブロックの状態
                if (0 <= _moveBlockY + i  && _moveBlockY + i  < FIELD_SIZE_Y && 0 <= _moveBlockX + j  && _moveBlockX + j < FIELD_SIZE_X)
                {
                    if (_fieldBlocksState[_moveBlockY + i, _moveBlockX + j] != eBlockState.eNone && _tempBlocksState[i, j] != eBlockState.eNone)
                    {
                         return true;
                    }
                }
            }
        }
        return false;
    }

     //ブロックのあたり判定(右回転)
    bool CheckCollisionRotateRight()
    {
        //回転
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                _tempBlocksState[i, j] = _moveBlocksState[MOVE_SIZE_X - 1 - j, i];
            }
        }
        return CheckCollisionRotate();
    }

     //ブロックのあたり判定(回転)
    bool CheckCollisionRotateLeft()
    {
        //回転
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                _moveBlocksState[i, j] = _tempBlocksState[j, MOVE_SIZE_Y - 1 - i];
            }
        }
       return CheckCollisionRotate();
    }

    //ブロックの状態を更新
    void UpdateBlockState()
    {
        //フィールド上でのブロックの状態
        for(int i = 0; i < FIELD_SIZE_Y; i++)
        {
            for(int j = 0; j < FIELD_SIZE_X; j++)
            {
                //フィールド上でのブロックの状態
               _fieldBlocksStateFinal[i, j] =_fieldBlocksState[i, j];
            }
        }
        //ブロックの状態反映(動作中)
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                //ブロックの状態
                if (0 <= _moveBlockY + i && _moveBlockY + i < FIELD_SIZE_Y && 0 <= _moveBlockX + j && _moveBlockX + j < FIELD_SIZE_X)
                {
                    if (_fieldBlocksStateFinal[_moveBlockY + i, _moveBlockX + j] == eBlockState.eNone)
                    {
                        _fieldBlocksStateFinal[_moveBlockY + i, _moveBlockX + j] = _moveBlocksState[i, j];
                    }
                }
            }
        }
        //枠の中のブロック非表示処理
        for(int i = 0; i < FIELD_SIZE_Y; i++)
        {
            for(int j = 0; j < FIELD_SIZE_X; j++)
            {
                //ブロックの状態
               
                _fieldBlocks[i, j].SetState(_fieldBlocksStateFinal[i, j]);
            }
        }
    }

    //ゲームオーバー判定
    bool GameOver()
    {
        //ブロックの状態反映(動作中)
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                //ブロックの状態
                if (0 <= _moveBlockY + i && _moveBlockY + i < FIELD_SIZE_Y && 0 <= _moveBlockX + j && _moveBlockX + j < FIELD_SIZE_X)
                {
                    if (_fieldBlocksStateFinal[_moveBlockY + i, _moveBlockX + j] != eBlockState.eNone && _moveBlocksState[i, j] != eBlockState.eNone)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void MergeBlock()
    {
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                if (0 <= _moveBlockY + i && _moveBlockY + i < FIELD_SIZE_Y && 0 <= _moveBlockX + j && _moveBlockX + j < FIELD_SIZE_X)
                {
                    //ブロックの状態
                    if (_fieldBlocksState[_moveBlockY + i, _moveBlockX + j] == eBlockState.eNone)
                    {
                        _fieldBlocksState[_moveBlockY + i, _moveBlockX + j] =_moveBlocksState[i, j];
                    }
                }
            }
        }
    }

    void CheckLine()
    {
        for(int i = 1; i < FIELD_SIZE_Y; i++)
        {
            bool isBlank = false;
            for(int j = 1; j < FIELD_SIZE_X - 1; j++)
            {
                //ブロックの状態
                if (_fieldBlocksState[i, j] == eBlockState.eNone)
                {
                    isBlank = true;
                }
            }
            if(!isBlank)
            {
                DeleteLine(i);
            }
        }
    }

    //ブロック消す処理
    void DeleteLine(int y)
    {
        for(int i = y; i < FIELD_SIZE_Y - 1; i++)
        {
            for(int j = 1; j < FIELD_SIZE_X - 1; j++)
            {
                if(_fieldBlocksState[i, j] >= eBlockState.eSkyBlue)
                {
                    _fieldBlocksState[i, j] = _fieldBlocksState[i + 1, j];
                }
            }
        }
    }

     bool LotNextBlock()
    {
        //ランダム
        int pattern = Random.Range(0, eBlockState.eMax - eBlockState.eSkyBlue);
        int[,] blocks = BLOCKS_LIST[pattern];

        //状態を設定
        for(int i = 0; i < MOVE_SIZE_Y; i++)
        {
            for(int j = 0; j < MOVE_SIZE_X; j++)
            {
                _moveBlocksState[i, j] = (eBlockState)blocks[i, j];
            }
        }

        return GameOver();
    }

    //ブロック設置開始
    void StartMove()
    {
        //初期位置の設定
        _moveBlockX = DEFAULT_MOVE_X;
        _moveBlockY = DEFAULT_MOVE_Y;

        // 抽選
        bool isGameOver = LotNextBlock();

        // ゲームオーバーの場合はゲームオーバー処理
        if (isGameOver)
        {
            StartCoroutine(GameOverProc());
        }
    }

    private IEnumerator GameOverProc()
    {
        _gameState = GameState.GameOver;

        yield return null;

        for (int i = 0; i < FIELD_SIZE_Y; i++)
        {
            for (int j = 0; j < FIELD_SIZE_X; j++)
            {
                if(_fieldBlocksStateFinal[i, j] != eBlockState.eNone)
                {
                    _fieldBlocks[i, j].SetState(eBlockState.eFrame);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.0f);

        yield break;
    }
}
