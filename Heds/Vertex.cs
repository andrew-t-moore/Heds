using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Heds
{
    public class Vertex : IEquatable<Vertex>, IMeshComponent
    {
        public Mesh Mesh { get; }
        public int Id { get; }
        public Vector3 Position { get; set; }

        private readonly List<HalfEdge> _incomingHalfEdges = new List<HalfEdge>();
        private readonly List<HalfEdge> _outgoingHalfEdges = new List<HalfEdge>();
        
        public IReadOnlyList<HalfEdge> IncomingHalfEdges => _incomingHalfEdges;
        public IReadOnlyList<HalfEdge> OutgoingHalfEdges => _outgoingHalfEdges;

        public Vertex(Mesh mesh, int id, Vector3 position)
        {
            Mesh = mesh;
            Id = id;
            Position = position;
        }

        /// <summary>
        /// Returns the faces that are incident to this vertex.
        /// </summary>
        public IEnumerable<Face> GetIncidentFaces()
        {
            return IncomingHalfEdges
                .Select(he => he?.Twin?.Face)
                .Where(f => f != null)
                .Distinct();
        }
        
        public bool Equals(Vertex other)
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
            return Equals((Vertex)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        internal void AddIncomingHalfEdge(HalfEdge halfEdge)
        {
            _incomingHalfEdges.Add(halfEdge);
        }

        internal void AddOutgoingHalfEdge(HalfEdge halfEdge)
        {
            _outgoingHalfEdges.Add(halfEdge);
        }

        internal void RemoveIncomingHalfEdge(HalfEdge halfEdge)
        {
            _incomingHalfEdges.Remove(halfEdge);
        }
        
        internal void RemoveOutgoingHalfEdge(HalfEdge halfEdge)
        {
            _outgoingHalfEdges.Remove(halfEdge);
        }

        internal void ClearHalfEdges()
        {
            _incomingHalfEdges.Clear();
            _outgoingHalfEdges.Clear();
        }
    }
}