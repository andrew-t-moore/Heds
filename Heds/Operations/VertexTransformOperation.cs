using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Heds.Operations
{
    public interface IOperation
    {
        Mesh Apply(Mesh oldMesh);
    }
    
    public abstract class VertexTransformOperation : IOperation
    {
        public abstract Vector3 Transform(Vector3 vertexPosition);
        
        public Mesh Apply(Mesh oldMesh)
        {
            var newMesh = new Mesh();
            var vertexMap = new Dictionary<Vertex, Vertex>();

            foreach (var oldVertex in oldMesh.Vertices)
            {
                vertexMap[oldVertex] = newMesh.AddVertex(
                    Transform(oldVertex.Position)
                );
            }

            var halfEdgeMap = new Dictionary<HalfEdge, HalfEdge>();

            foreach (var oldHalfEdge in oldMesh.HalfEdges)
            {
                var v1 = vertexMap[oldHalfEdge.From];
                var v2 = vertexMap[oldHalfEdge.To];

                halfEdgeMap[oldHalfEdge] = newMesh.AddHalfEdge(v1, v2);
            }
            
            foreach (var oldFace in oldMesh.Faces)
            {
                var edgesInNewFace = oldFace.HalfEdges
                    .Select(e => halfEdgeMap[e])
                    .ToArray();
            
                newMesh.AddFace(edgesInNewFace);
            }

            return newMesh;
        }
    }
}