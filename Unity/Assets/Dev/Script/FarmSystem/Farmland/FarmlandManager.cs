using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class FarmlandGrownInfo
{
    private GrownDefinition _definition;
    private Vector3Int _cellPos;

    public bool Empty => _definition == false;

    public GrowingTile CurrentTile { get; set; }
    public GrownDefinition Definition => _definition;
    public Vector3Int CellPos => _cellPos;
    public int TotalStep { get; set; }

    public int GrownStep { get; set; }

    public FarmlandGrownInfo(GrownDefinition definition, Vector3Int cellPos)
    {
        CurrentTile = definition.FirstTile;
        _definition = definition;
        _cellPos = cellPos;
        TotalStep = 0;
        GrownStep = 0;
    }
}

public class FarmlandManager : MonoBehaviour
{
    [SerializeField] private FarmlandTileController _controller;
    
    
    private async void Awake()
    {
        while (true)
        {
            await UniTask.Delay(1000, DelayType.DeltaTime, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);

            GrowUp();
        }
    }

    public void GrowUp(int step = 1)
    {
        var grownInfos = _controller.GetAllGrownInfo();

        foreach (FarmlandGrownInfo info in grownInfos)
        {
            SetGrownState(info, info.TotalStep + 1);
        }
    }

    public void SetGrownState(FarmlandGrownInfo info, int step)
    {
        var def = info.Definition;
        
        step = Mathf.Clamp(step, 0, def.NeedGrowingToNextGrowingStep.Count - 1);
        info.TotalStep = step;
        GrowingTile nextTile = info.CurrentTile;
        
        for (int i = 0; i < def.NeedGrowingToNextGrowingStep.Count; i++)
        {
            var set = def.NeedGrowingToNextGrowingStep.ElementAtOrDefault(i);
                
            if (set is not null && step >= set.NeedStep)
            {
                nextTile = set.Tile;
                info.GrownStep = Mathf.Max(i, def.NeedGrowingToNextGrowingStep.Count - 1);
            }
        }

        info.CurrentTile = nextTile;
        _controller.RefeshPlantTile(info.CellPos);
    }
}
