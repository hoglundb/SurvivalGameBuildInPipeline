using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuildingBlock : MonoBehaviour
{
    [SerializeField] private SOBaseBuildingPiece _soBaseBuildingPiece;


    public SOBaseBuildingPiece GetBaseBuildingPieceScriptableObj()
    {
        return _soBaseBuildingPiece;
    }
}
