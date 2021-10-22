using System.Collections.Generic;
using System.Linq;

namespace Heds.Selections
{
    public class VertexSelection
    {
        public IReadOnlyList<Vertex> Vertices { get; }

        public VertexSelection(IEnumerable<Vertex> vertices)
        {
            Vertices = vertices.ToArray();
        }

        public VertexSelection(params Vertex[] vertices)
        {
            Vertices = vertices;
        }

        /// <summary>
        /// Adds some vertices to the selection.
        /// </summary>
        public VertexSelection Add(params Vertex[] vertices)
        {
            return new VertexSelection(
                Vertices.Union(vertices)
            );
        }
        
        /// <summary>
        /// Adds a vertex to the selection.
        /// </summary>
        public static VertexSelection operator +(VertexSelection selection, Vertex vertex)
        {
            return selection.Add(vertex);
        }
    }
}