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

                // Figure out side faces.
                var facesToCreate = face.HalfEdges
                    .Where(he => !internalHalfEdges.Contains(he))
                    .Select(he => new[]{ he.From, he.To, vertexMap[he.To], vertexMap[he.From] })
                    .ToArray();
                
                mesh.DetachFace(face);
                
                // Create side faces.
                foreach (var vertexList in facesToCreate)
                {
                    mesh.AddFace(vertexList);
                }
                
            }

            foreach (var halfEdge in internalHalfEdges)
            {
                mesh.DetachHalfEdge(halfEdge);
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