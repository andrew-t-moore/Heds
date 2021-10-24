using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Heds
{
    public class Face : IEquatable<Face>
    {
        public Mesh Mesh => HalfEdges[0].Mesh;
        public int Id { get; }
        public IReadOnlyList<HalfEdge> HalfEdges => _halfEdges;
        private readonly HalfEdge[] _halfEdges;

        public IEnumerable<Vertex> Vertices => HalfEdges.Select(he => he.From);

        public Face(int id, params HalfEdge[] halfEdges)
        {
            if (halfEdges.Length < 3)
                throw new InvalidOperationException($"Can't create a face with {halfEdges.Length} edges. Need at least 3 edges.");
        
            Id = id;
            _halfEdges = halfEdges;
        }

        public bool IsTriangle => _halfEdges.Length == 3;

        public Vector3 GetMidpoint()
        {
            return Vertices
                .Select(v => v.Position)
                .Aggregate((a, b) => a + b) / HalfEdges.Count;
        }
        
        public IEnumerable<Face> GetAdjacentFaces()
        {
            return _halfEdges
                .Select(he => he?.Twin?.Face)
                .Where(f => f != null)
                .Distinct();
        }

        public bool IsAdjacentTo(Face otherFace)
        {
            return GetAdjacentFaces()
                .Contains(otherFace);
        }

        public bool IsIncidentOn(Vertex vertex)
        {
            return _halfEdges.Any(e => e.From.Equals(vertex));
        }

        public Vector3 GetSurfaceNormal()
        {
            var a = _halfEdges[0].From.Position;
            var b = _halfEdges[1].From.Position;
            var c = _halfEdges[2].From.Position;

            return new Plane(a, b, c).normal;
        }
        
        public bool Equals(Face other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Face)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}