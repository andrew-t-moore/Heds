using FluentAssertions;
using Heds.Primitives;
using Heds.Selections;
using Xunit;

namespace Heds.Tests.Operations
{
    public class FilterFacesOperationTests
    {
        [Fact]
        public void RemovesOtherFaces()
        {
            var cube = QuadCube.Create();
            var selection = new FaceSelection(cube.Faces[0]);
            var mesh = cube.Filter(selection);

            mesh.Vertices.Should().HaveCount(4);
            mesh.HalfEdges.Should().HaveCount(4);
            mesh.Faces.Should().HaveCount(1);
        } 
    }
}