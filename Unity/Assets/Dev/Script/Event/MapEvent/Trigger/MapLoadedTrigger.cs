using System;
using System.Collections;

public class MapLoadedTrigger : MapTriggerBase
{
    private void Start()
    {
        StartCoroutine(CoUpdate());
    }

    private IEnumerator CoUpdate()
    {
        while (true)
        {
            if (GameObjectStorage.Instance.TryGetPlayerController(out var pc))
            {
                Trigger(pc.Interaction);
                break;
            }
            else
            {
                yield return null;
            }
        }
    }
}