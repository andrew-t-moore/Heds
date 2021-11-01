using UnityEngine;

namespace Heds.Operations
{
    /// <summary>
    /// An operation that translates (moves) the mesh around.
    /// </summary>
    public class TranslateOperation : IOperation
    {
        private readonly Vector3 _translation;

        public TranslateOperation(Vector3 translation)
        {
            _translation = translation;
        }

        public void Apply(Mesh mesh)
        {
            foreach (var vertex in mesh.Vertices)
            {
                vertex.Position = vertex.Position + _translation;
            }
        }
    }
}