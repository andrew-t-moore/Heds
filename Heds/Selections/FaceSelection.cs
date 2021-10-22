using System.Linq;

namespace Heds.Selections
{
    public class FaceSelection
    {
        public Face[] Faces { get; }

        public FaceSelection(params Face[] faces)
        {
            Faces = faces;
        }

        public FaceSelection Grow()
        {
            var newFaces = Faces
                .Union(Faces.SelectMany(f => f.GetAdjacentFaces()))
                .Distinct()
                .ToArray();

            return new FaceSelection(newFaces);
        }
    }
}