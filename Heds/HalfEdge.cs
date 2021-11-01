using System;
using UnityEngine;

namespace Heds
{
    public class HalfEdge : IEquatable<HalfEdge>, IMeshComponent
    {
        public Mesh Mesh => From.Mesh;
        public int Id { get; }
        public HalfEdge Twin { get; private set; }
        public Vertex From { get; }
        public Vertex To { get; }
        public Face Face { get; private set; }
        public float Length => (To.Position - From.Position).magnitude;

        public HalfEdge(int id, Vertex from, Vertex to)
        {
            if (!ReferenceEquals(from.Mesh, to.Mesh))
                throw new InvalidOperationException("Cannot create a half-edge between two different graphs.");

            Id = id;
            From = from;
            To = to;
        }

        public Vector3 GetMidPoint()
        {
            return (From.Position + To.Position) / 2f;
        }
        
        public bool Equals(HalfEdge other)
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
            return Equals((HalfEdge)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public void SetFace(Face face)
        {
            Face = face;
        }

        public void SetTwin(HalfEdge twin)
        {
            Twin = twin;
        }
    }
}