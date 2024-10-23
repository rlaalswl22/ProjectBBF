using UnityEngine;


public class Npc : ActorProxy
{
    [SerializeField] private ActorFavorablity _favorablity;
        
    protected override void OnInit()
    {
        _favorablity.Init(Owner);

        ContractInfo
            .AddBehaivour<IBADialogue>(_favorablity)
            ;
    }

    protected override void OnDoDestroy()
    {
    }
}