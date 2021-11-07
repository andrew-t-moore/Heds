using System.Collections.Generic;
using System.Linq;

namespace Heds.Utilities
{
    /// <summary>
    /// For convenience. Makes it easier to copy faces and vertices from one
    /// mesh into another in cases where there is a simple 1-to-1 mapping
    /// from mesh components in the old mesh onto mesh components in the new
    /// mesh.
    /// </summary>
    public class MeshMapHelper
    {
        public Mesh Mesh { get; } = new Mesh();

        private readonly Dictionary<Vertex, Vertex> _vertexMap = new Dictionary<Vertex, Vertex>();
        private readonly Dictionary<Face, Face> _faceMap = new Dictionary<Face, Face>();
        private readonly Mesh _oldMesh;

        public MeshMapHelper(Mesh oldMesh)
        {
            _oldMesh = oldMesh;
        }

        public Vertex GetOrCloneVertex(Vertex vertex)
        {
            if (_vertexMap.TryGetValue(vertex, out var existing))
            {
                return existing;
            }
            else
            {
                var clone = Mesh.AddVertex(vertex.Position);
                _vertexMap[vertex] = clone;
                return clone;
            }
        }

        public Face GetOrCloneFace(Face face)
        {
            if (_faceMap.TryGetValue(face, out var existing))
            {
                return existing;
            }
            else
            {
                var vertices = face.Vertices
                    .Select(GetOrCloneVertex)
                    .ToArray();
                var clone = Mesh.AddFace(vertices);
                _faceMap[face] = clone;
                return clone;
            }
        }
    }
}