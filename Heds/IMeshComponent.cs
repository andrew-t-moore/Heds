namespace Heds
{
    public interface IMeshComponent
    {
        Mesh Mesh { get; }
        
        bool IsDetached { get; }
    }
}