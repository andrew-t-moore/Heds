using FluentAssertions;

namespace Heds.Tests.TestUtilities
{
    public static class MeshTopologies
    {
        /// <summary>
        /// Check if two meshes are topologically the same.
        /// </summary>
        public static void AssertEquivalent(Mesh a, Mesh b)
        {
            a.Should().NotBeSameAs(b, because: "testing a mesh against itself indicates a problem in the test");
            // TODO: needs a lot of work.
            a.Vertices.Should().HaveCount(b.Vertices.Count);
            a.Faces.Should().HaveCount(b.Faces.Count);
            a.HalfEdges.Should().HaveCount(b.HalfEdges.Count);
        }
    }
}