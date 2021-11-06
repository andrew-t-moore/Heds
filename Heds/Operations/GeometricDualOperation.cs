using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Heds.Operations
{
    /// <summary>
    /// Creates the geometric dual of a mesh, i.e. a mesh where each
    /// face is replaced by a vertex and each vertex is replaced with
    /// a face.
    /// </summary>
    public class GeometricDualOperation : IOperation
    {
        public void Apply(Mesh mesh)
        {
            var existingVertices = mesh.Vertices.ToArray();

            var faceMidpoints = mesh.Faces
                .ToDictionary(f => f, f => f.GetMidpoint());
            
            var adjacentFaceList = existingVertices
                .Select(GetIncidentFacesWithSameWindingOrder)
                .ToArray();
            
            mesh.DetachAll();

            var newVertexMap = faceMidpoints
                .ToDictionary(f => f.Key, f => mesh.AddVertex(f.Value));
            
            foreach (var faceList in adjacentFaceList)
            {
                var vertices = faceList
                    .Select(f => newVertexMap[f])
                    .ToArray();

                mesh.AddFace(vertices);
            }
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