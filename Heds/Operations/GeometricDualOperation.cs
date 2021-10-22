using System;
using System.Collections.Generic;
using System.Linq;

namespace Heds.Operations
{
    /// <summary>
    /// Creates the geometric dual of a mesh, i.e. a mesh where each
    /// face is replaced by a vertex and each vertex is replaced with
    /// a face.
    /// </summary>
    public class GeometricDualOperation : IOperation
    {
        public Mesh Apply(Mesh oldMesh)
        {
            var newMesh = new Mesh();

            var faceToVertexMap = oldMesh.Faces
                .ToDictionary(f => f, f => newMesh.AddVertex(f.GetMidpoint()));

            foreach (var oldVertex in oldMesh.Vertices)
            {
                var newFaceVertices = GetIncidentFacesWithSameWindingOrder(oldVertex)
                    .Select(f => faceToVertexMap[f])
                    .ToArray();

                newMesh.AddFace(newFaceVertices, out _);
            }

            return newMesh;
        }

        IReadOnlyList<Face> GetIncidentFacesWithSameWindingOrder(Vertex vertex)
        {
            if (vertex.OutgoingHalfEdges.Count == 0)
                return Array.Empty<Face>();

            var firstOutgoingHalfEdge = vertex.OutgoingHalfEdges[0];
            var currentOutgoingHalfEdge = firstOutgoingHalfEdge;
            var orderedFaces = new List<Face>();

            do
            {
                var currentFace = currentOutgoingHalfEdge.Face;
                orderedFaces.Add(currentFace);
                
                var currentIncomingHalfEdge = currentFace.HalfEdges
                    .First(he => he.To.Equals(vertex));

                currentOutgoingHalfEdge = currentIncomingHalfEdge.Twin;
            }
            while (!currentOutgoingHalfEdge.Equals(firstOutgoingHalfEdge));

            return orderedFaces;
        }
    }
}