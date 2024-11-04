


using ProjectBBF.Event;

public interface IBOInteractive : IObjectBehaviour
{
    public void UpdateInteract(CollisionInteractionMono caller);
}
public interface IBOInteractiveTool : IObjectBehaviour
{
    public bool IsVaildTool(ToolRequireSet toolSet);
    public void UpdateInteract(CollisionInteractionMono caller);
}