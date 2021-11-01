using FluentAssertions;
using Heds.Primitives;
using Xunit;

namespace Heds.Tests.Operations
{
    public class GeometricDualOperationTests
    {
        [Fact]
        public void Test()
        {
            var mesh = QuadCube.Create()
                .GeometricDual();

            mesh.Vertices.Should().HaveCount(6);
            mesh.Faces.Should().HaveCount(8);
            mesh.HalfEdges.Should().HaveCount(24);
        }
    }
}