using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using FluentAssertions;
using Heds.Primitives;
using Xunit;

namespace Heds.Tests
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class MeshTests
    {
        [Fact]
        public void CallingAddVertexReturnsTheSameMeshInstance()
        {
            var mesh1 = new Mesh();
            var mesh2 = mesh1.AddVertex(0f, 0f, 0f, out _);

            mesh1.Should().BeSameAs(mesh2);
        }

        [Fact]
        public void CallingAddHalfEdgeReturnsTheSameMeshInstance()
        {
            var mesh1 = new Mesh()
                .AddVertex(0f, 0f, 0f, out var v0)
                .AddVertex(1f, 0f, 0f, out var v1);

            var mesh2 = mesh1.AddHalfEdge(v0, v1, out _);

            mesh1.Should().BeSameAs(mesh2);
        }

        [Fact]
        public void CallingAddFaceReturnsTheSameMeshInstance()
        {
            var mesh1 = new Mesh()
                .AddVertex(0f, 0f, 0f, out var v0)
                .AddVertex(1f, 0f, 0f, out var v1)
                .AddVertex(1f, 1f, 0f, out var v2)
                .AddHalfEdge(v0, v1, out var v0v1)
                .AddHalfEdge(v1, v2, out var v1v2)
                .AddHalfEdge(v2, v0, out var v2v0);

            var mesh2 = mesh1.AddFace(new[] { v0v1, v1v2, v2v0 }, out _);
            mesh1.Should().BeSameAs(mesh2);
        }

        [Fact]
        public void CanSplitAMeshIntoMultipleMeshes()
        {
            var mesh = QuadCube.Create();
            var meshes = mesh.SplitMesh(f => f.Id);

            meshes.Should().HaveCount(6);
            meshes.Should().OnlyContain(kvp => kvp.Value.Faces.Count == 1);
            meshes.Should().OnlyContain(kvp => kvp.Value.Vertices.Count == 4);
            meshes.Should().OnlyContain(kvp => kvp.Value.HalfEdges.Count == 4);
        }

        [Fact]
        public void SplittingAMeshDoesNotChangeTheOriginalMesh()
        {
            var mesh = QuadCube.Create();
            mesh.SplitMesh(f => f.Id);

            mesh.Vertices.Should().HaveCount(8);
            mesh.HalfEdges.Should().HaveCount(24);
            mesh.Faces.Should().HaveCount(6);
        }
    }
}