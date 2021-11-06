using System;
using UnityEngine;

namespace Heds
{
    public class HalfEdge : IEquatable<HalfEdge>, IMeshComponent
    {
        public Mesh Mesh => From.Mesh;
        public bool IsDetached { get; }
        public int Id { get; }
        public HalfEdge Twin { get; private set; }
        public Vertex From { get; private set; }
        public Vertex To { get; private set; }
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

        internal void Detach()
        {
            if (IsDetached)
                return;

            From.OnHalfEdgeDetach(this);
            To.OnHalfEdgeDetach(this);
            Face?.Detach();
            Twin?.SetTwin(null);
            
            Twin = null;
            From = null;
            To = null;
        }

        /// <summary>
        /// Called when the face incident on this half-edge is being detached.
        /// </summary>
        internal void OnFaceDetach(Face face)
        {
            if (IsDetached)
                return;
            
            if (face.Equals(Face))
            {
                Face = null;
            }
            else
            {
                throw new InvalidOperationException($"Can't remove the face {face} from half-edge {this} - the face is not incident on this half-edge.");
            }
        }
    }
}