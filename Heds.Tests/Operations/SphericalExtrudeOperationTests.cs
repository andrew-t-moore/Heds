using System.Linq;
using FluentAssertions;
using Heds.Primitives;
using Heds.Selections;
using Xunit;

namespace Heds.Tests.Operations
{
    public class SphericalExtrudeOperationTests
    {
        [Fact]
        public void ASingleFaceCanBeExtruded()
        {
            var mesh = QuadCube.Create();
            var selection = new FaceSelection(mesh.Faces[0]);

            mesh.SphericalExtrude(selection, 0.1f);

            mesh.Vertices.Should().HaveCount(12);
            mesh.HalfEdges.Should().HaveCount(40);
            mesh.Faces.Should().HaveCount(10);
        }

        [Fact]
        public void MultipleFacesCanBeExtruded()
        {
            var mesh = QuadCube.Create();
            var selection = new FaceSelection(mesh.Faces[0], mesh.Faces[1]);

            mesh.SphericalExtrude(selection, 0.1f);

            var shitEdges = mesh.HalfEdges
                .Where(he => he.Face == null)
                .ToArray();
            
            mesh.Vertices.Should().HaveCount(14);
            mesh.HalfEdges.Should().HaveCount(48);
            mesh.Faces.Should().HaveCount(12);
        }
    }
}