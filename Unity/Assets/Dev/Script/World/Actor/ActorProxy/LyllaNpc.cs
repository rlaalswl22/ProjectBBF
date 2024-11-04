


using UnityEngine;

public class LyllaNpc : ActorProxy
{
    [SerializeField] private LyllaFavorability _favorability;
    [SerializeField] private LyllaMove _move;
    protected override void OnInit()
    {
        _move.Init(Owner);
        _favorability.Init(Owner);

        ContractInfo
            .AddBehaivour<IBODialogue>(_favorability)
            .AddBehaivour<IBOInteractive>(_favorability)
            ;
    }

    protected override void OnDoDestroy()
    {
    }
}