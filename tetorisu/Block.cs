using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    static readonly Color DEFAULT_COLOR = new Color(1.0f, 1.0f, 1.0f);
    static readonly Color FRAME_COLOR = new Color(0.1f, 0.1f, 0.1f);
    [SerializeField] Material _material = null;
    Material _myMaterial = null;

    [SerializeField] MeshRenderer _cubeA = null;
    [SerializeField] MeshRenderer _cubeB = null;
    // Start is called before the first frame update
    void Start()
    {
        //_myMaterial = GameObject.Instantiate<Material>(_material);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color color)
    {
        if(_myMaterial == null)
        {
            _myMaterial = GameObject.Instantiate<Material>(_material);
            _cubeA.material = _myMaterial;
            _cubeB.material = _myMaterial;
        }
        _myMaterial.color = color;
    }

    public void SetState(system.eBlockState state)
    {
        bool isActive = (state != system.eBlockState.eNone);
        {
            _cubeA.gameObject.SetActive(isActive);
            _cubeB.gameObject.SetActive(isActive);
        }
        switch (state)
        {
            case system.eBlockState.eFrame:
                SetColor(FRAME_COLOR);
            break;
            case system.eBlockState.eSkyBlue:
            break;
            case system.eBlockState.eYellow:
            break;
            case system.eBlockState.ePurple:
            break;
            case system.eBlockState.eBlue:
            break;
            case system.eBlockState.eOrange:
            break;
            case system.eBlockState.eGreen:
            break;
            case system.eBlockState.eRed:
            break;          
        }
    }
}
