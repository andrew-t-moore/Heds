using FluentAssertions;
using Xunit;

namespace Heds.Tests
{
    public class FaceTests
    {
        [Fact]
        public void CanFindFacesAdjacentToAFace()
        {
            new Mesh()
                .AddVertex(0f, 0f, 0f, out var v0)
                .AddVertex(1f, 0f, 0f, out var v1)
                .AddVertex(1f, 1f, 0f, out var v2)
                .AddVertex(0f, 1f, 0f, out var v3)
                .AddFace(new[] { v0, v1, v2 }, out var f0)
                .AddFace(new[] { v0, v2, v3 }, out var f1);

            f0.GetAdjacentFaces().Should().ContainSingle(f => ReferenceEquals(f, f1));
            f1.GetAdjacentFaces().Should().ContainSingle(f => ReferenceEquals(f, f0));
        }
    }
}