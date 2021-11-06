using System;
using System.Collections.Generic;
using System.Linq;

namespace Heds.Operations
{
    public class SubdivideTrianglesOperation : IOperation
    {
        private readonly int _numLevels;

        public SubdivideTrianglesOperation(int numLevels)
        {
            _numLevels = numLevels;
        }
        
        public void Apply(Mesh mesh)
        {
            if (mesh.Faces.Any(f => !f.IsTriangle))
                throw new InvalidOperationException($"Can't subdivide a mesh that contains non-triangular faces.");

            for (var i = 0; i < _numLevels; i++)
            {
                ApplyInternal(mesh);
            }
        }

        private void ApplyInternal(Mesh mesh)
        {
            var midPointVertexLookup = new Dictionary<EdgeMidpoint, Vertex>();

            Vertex GetVertexAtMidPoint(HalfEdge he)
            {
                var key = new EdgeMidpoint(he.From, he.To);
                if (midPointVertexLookup.TryGetValue(key, out var existingVertex))
                {
                    return existingVertex;
                }

                var newVertex = mesh.AddVertex(he.GetMidPoint());
                midPointVertexLookup[key] = newVertex;
                return newVertex;
            }

            // The loop below modifies while subdividing, so 
            // make a copy of the faces first.
            var facesToSubdivide = mesh.Faces.ToArray();
            
            foreach (var face in facesToSubdivide)
            {
                mesh.AddFace(new[]
                {
                    face.HalfEdges[0].From,
                    GetVertexAtMidPoint(face.HalfEdges[0]),
                    GetVertexAtMidPoint(face.HalfEdges[2])
                });
                
                mesh.AddFace(new[]
                {
                    GetVertexAtMidPoint(face.HalfEdges[0]),
                    face.HalfEdges[1].From,
                    GetVertexAtMidPoint(face.HalfEdges[1])
                });
                
                mesh.AddFace(new[]
                {
                    GetVertexAtMidPoint(face.HalfEdges[2]),
                    GetVertexAtMidPoint(face.HalfEdges[0]),
                    GetVertexAtMidPoint(face.HalfEdges[1])
                });

                mesh.AddFace(new[]
                {
                    GetVertexAtMidPoint(face.HalfEdges[2]),
                    GetVertexAtMidPoint(face.HalfEdges[1]),
                    face.HalfEdges[2].From
                });

                mesh.DetachFace(face, true);
            }
        }
        
        /// <summary>
        /// Models the midpoint of an edge. 
        /// </summary>
        private class EdgeMidpoint : IEquatable<EdgeMidpoint>
        {
            private readonly Vertex _from;
            private readonly Vertex _to;

            public EdgeMidpoint(Vertex from, Vertex to)
            {
                if (from.Id < to.Id)
                {
                    _from = from;
                    _to = to;
                }
                else
                {
                    _from = to;
                    _to = from;
                }
            }

            public bool Equals(EdgeMidpoint other)
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
                return Equals((EdgeMidpoint)obj);
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