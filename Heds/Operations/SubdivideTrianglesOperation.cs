using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Heds.Operations
{
    public class SubdivideTrianglesOperation : IOperation
    {
        private readonly int _numLevels;

        private static readonly int[][] SubdividedFaceIndices =
        {
            new[] { 0, 1, 5 },
            new[] { 1, 2, 3 },
            new[] { 5, 1, 3 },
            new[] { 5, 3, 4 }
        };

        public SubdivideTrianglesOperation(int numLevels)
        {
            _numLevels = numLevels;
        }
        
        public Mesh Apply(Mesh oldMesh)
        {
            var currentMesh = oldMesh;
            
            for (var i = 0; i < _numLevels; i++)
            {
                currentMesh = ApplyInternal(currentMesh);
            }

            return currentMesh;
        }

        private Mesh ApplyInternal(Mesh oldMesh)
        {
            var newMesh = new Mesh();
            var vertexMap = new Dictionary<SubdivisionVertexId, Vertex>();

            Vertex GetOrCreateVertex(SubdivisionVertexId key)
            {
                if (vertexMap.TryGetValue(key, out var vertex))
                {
                    return vertex;
                }
                else
                {
                    var newVertexPosition = key.GetPosition();

                    var newVertex = newMesh.AddVertex(newVertexPosition);
                    vertexMap[key] = newVertex;
                    return newVertex;
                }
            }

            foreach (var face in oldMesh.Faces)
            {
                if (!face.IsTriangle)
                    throw new InvalidOperationException($"Can't subdivide a mesh that contains non-triangular faces.");
                
                var oldVertices = face.Vertices.ToArray();

                var newVertexKeys = new[]
                {
                    SubdivisionVertexId.FromExistingVertex(oldVertices[0]),
                    SubdivisionVertexId.FromMidpointBetweenTwoVertices(oldVertices[0], oldVertices[1]),
                    SubdivisionVertexId.FromExistingVertex(oldVertices[1]),
                    SubdivisionVertexId.FromMidpointBetweenTwoVertices(oldVertices[1], oldVertices[2]),
                    SubdivisionVertexId.FromExistingVertex(oldVertices[2]),
                    SubdivisionVertexId.FromMidpointBetweenTwoVertices(oldVertices[2], oldVertices[0])
                };

                foreach (var indices in SubdividedFaceIndices)
                {
                    var vertices = indices
                        .Select(i => GetOrCreateVertex(newVertexKeys[i]))
                        .ToArray();

                    newMesh.AddFace(vertices, out _);
                }
            }

            return newMesh;
        }
        
        /// <summary>
        /// Models a point in space that is either on an existing vertex or halfway
        /// between two existing vertices.
        /// </summary>
        private class SubdivisionVertexId : IEquatable<SubdivisionVertexId>
        {
            private readonly Vertex _from;
            private readonly Vertex _to;

            private SubdivisionVertexId(Vertex from, Vertex to)
            {
                _from = from;
                _to = to;
            }

            public static SubdivisionVertexId FromExistingVertex(Vertex existingVertex)
            {
                return new SubdivisionVertexId(existingVertex, existingVertex);
            }

            public static SubdivisionVertexId FromMidpointBetweenTwoVertices(Vertex a, Vertex b)
            {
                return a.Id < b.Id 
                    ? new SubdivisionVertexId(a, b) 
                    : new SubdivisionVertexId(b, a);
            }

            public Vector3 GetPosition()
            {
                return Equals(_from, _to) 
                    ? _from.Position 
                    : (_from.Position + _to.Position) / 2f;
            }
            
            public bool Equals(SubdivisionVertexId other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(_from, other._from) && Equals(_to, other._to);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((SubdivisionVertexId)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_from != null ? _from.GetHashCode() : 0) * 397) ^ (_to != null ? _to.GetHashCode() : 0);
                }
            }
        }
    }
}