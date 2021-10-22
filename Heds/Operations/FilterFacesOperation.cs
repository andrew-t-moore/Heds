using System.Collections.Generic;
using System.Linq;
using Heds.Selections;

namespace Heds.Operations
{
    public class FilterFacesOperation : IOperation
    {
        private readonly FaceSelection _selection;

        public FilterFacesOperation(FaceSelection selection)
        {
            _selection = selection;
        }
        
        public Mesh Apply(Mesh oldMesh)
        {
            var newMesh = new Mesh();
            
            var verticesToKeep = _selection.Faces.SelectMany(f => f.Vertices)
                .Distinct()
                .ToArray();

            var vertexMap = new Dictionary<Vertex, Vertex>();
            
            foreach (var oldVertex in verticesToKeep)
            {
                var newVertex = newMesh.AddVertex(oldVertex.Position);
                vertexMap[oldVertex] = newVertex;
            }

            foreach (var oldFace in _selection.Faces)
            {
                var vertices = oldFace.Vertices
                    .Select(v => vertexMap[v])
                    .ToArray();

                newMesh.AddFace(vertices, out _);
            }

            return newMesh;
        }
    }
}