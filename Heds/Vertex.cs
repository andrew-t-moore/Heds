using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Heds
{
    public class Vertex : IEquatable<Vertex>, IMeshComponent
    {
        public Mesh Mesh { get; }
        public bool IsDetached { get; private set; }
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

        internal void Detach()
        {
            if (IsDetached)
                return;
            
            IsDetached = true;

            foreach (var halfEdge in IncomingHalfEdges)
            {
                halfEdge.Detach();
            }
            
            foreach (var halfEdge in OutgoingHalfEdges)
            {
                halfEdge.Detach();
            }
            
            _incomingHalfEdges.Clear();
            _outgoingHalfEdges.Clear();
        }

        internal void OnHalfEdgeDetach(HalfEdge halfEdge)
        {
            if (IsDetached)
                return;
            
            if (halfEdge.From.Equals(this))
            {
                _outgoingHalfEdges.Remove(halfEdge);
            }
            else if (halfEdge.To.Equals(this))
            {
                _incomingHalfEdges.Remove(halfEdge);
            }
            else
            {
                throw new InvalidOperationException($"Can't remove the half-edge {halfEdge} from vertex {this} - the half-edge is not incident on this vertex.");
            }
        }
    }
}