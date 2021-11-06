using System.Collections.Generic;
using System.Linq;

namespace Heds.Selections
{
    public class FaceSelection
    {
        public Face[] Faces { get; }

        public FaceSelection(IEnumerable<Face> faces) : this(faces.ToArray())
        {
        }
        
        public FaceSelection(params Face[] faces)
        {
            Faces = faces;
        }

        public FaceSelection Union(FaceSelection other)
        {
            return Union(other.Faces);
        }

        public FaceSelection Union(IEnumerable<Face> faces)
        {
            return new FaceSelection(Faces.Union(faces).Distinct());
        }
        
        public FaceSelection Grow()
        {
            var newFaces = Faces.SelectMany(f => f.GetAdjacentFaces());
            return Union(newFaces);
        }
    }
}