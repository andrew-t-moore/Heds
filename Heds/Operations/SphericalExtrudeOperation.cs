using System.Collections.Generic;
using System.Linq;
using Heds.Selections;
using Heds.Utilities;

namespace Heds.Operations
{
    public class SphericalExtrudeOperation : IOperation
    {
        private readonly FaceSelection _selection;
        private readonly float _distance;

        public SphericalExtrudeOperation(FaceSelection selection, float distance)
        {
            _selection = selection;
            _distance = distance;
        }

        public void Apply(Mesh mesh)
        {
            var vertexMap = new Dictionary<Vertex, Vertex>();
            var internalHalfEdges = CalculateInternalHalfEdges(_selection);

            Vertex GetOrCreateNewVertex(Vertex oldVertex)
            {
                if (vertexMap.TryGetValue(oldVertex, out var newVertex))
                {
                    return newVertex;
                }
                else
                {
                    var newVertexPosition = oldVertex.Position.normalized * (oldVertex.Position.magnitude + _distance);
                    newVertex = mesh.AddVertex(newVertexPosition);
                    vertexMap[oldVertex] = newVertex;
                    return newVertex;
                }
            }

            foreach (var face in _selection.Faces)
            {
                // Create new face
                var newVertices = face.Vertices
                    .Select(GetOrCreateNewVertex)
                    .ToArray();

                mesh.AddFace(newVertices);
                
                // Remove old face.
                mesh.RemoveFace(face);

                // Create side faces.
                foreach (var oldHalfEdge in face.HalfEdges)
                {
                    if (!internalHalfEdges.Contains(oldHalfEdge))
                    {
                        mesh.AddFace(new[]
                        {
                            oldHalfEdge.From,
                            oldHalfEdge.To,
                            vertexMap[oldHalfEdge.To],
                            vertexMap[oldHalfEdge.From]
                        });
                    }
                }
            }

            foreach (var halfEdge in internalHalfEdges)
            {
                mesh.RemoveHalfEdge(halfEdge);
            }
        }

        private HashSet<HalfEdge> CalculateInternalHalfEdges(FaceSelection selection)
        {
            var halfEdges = selection.Faces
                .SelectMany(f => f.HalfEdges)
                .ToHashSet();

            return selection.Faces
                .SelectMany(f => f.HalfEdges)
                .Where(he => he.Twin != null && halfEdges.Contains(he.Twin))
                .ToHashSet();
        }
    }
}