using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
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
        public void HalfEdgesAreLinkedAutomaticallyToTheirTwins()
        {
            new Mesh()
                .AddVertex(0f, 0f, 0f, out var v0)
                .AddVertex(1f, 0f, 0f, out var v1)
                .AddHalfEdge(v0, v1, out var he0)
                .AddHalfEdge(v1, v0, out var he1);

            he0.Twin.Should().BeSameAs(he1);
            he1.Twin.Should().BeSameAs(he0);
        }

        [Fact]
        public void HalfEdgesAddedAsAPairAreLinkedAutomatically()
        {
            new Mesh()
                .AddVertex(0f, 0f, 0f, out var v0)
                .AddVertex(1f, 0f, 0f, out var v1)
                .AddPairOfHalfEdges(v0, v1, out var v0v1, out var v1v0);

            v0v1.Twin.Should().BeSameAs(v1v0);
            v1v0.Twin.Should().BeSameAs(v0v1);
        }

        [Fact]
        public void VerticesAreLinkedToHalfEdges()
        {
            new Mesh()
                .AddVertex(0f, 0f, 0f, out var v0)
                .AddVertex(1f, 0f, 0f, out var v1)
                .AddPairOfHalfEdges(v0, v1, out var v0v1, out var v1v0);

                v0.OutgoingHalfEdges.Should().ContainSingle(he => ReferenceEquals(he, v0v1));
                v0.IncomingHalfEdges.Should().ContainSingle(he => ReferenceEquals(he, v1v0));
                
                v1.OutgoingHalfEdges.Should().ContainSingle(he => ReferenceEquals(he, v1v0));
                v1.IncomingHalfEdges.Should().ContainSingle(he => ReferenceEquals(he, v0v1));
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
        public void HalfEdgesAreLinkedToFacesAutomatically()
        {
            new Mesh()
                .AddVertex(0f, 0f, 0f, out var v0)
                .AddVertex(1f, 0f, 0f, out var v1)
                .AddVertex(1f, 1f, 0f, out var v2)
                .AddHalfEdge(v0, v1, out var v0v1)
                .AddHalfEdge(v1, v2, out var v1v2)
                .AddHalfEdge(v2, v0, out var v2v0)
                .AddFace(new[] { v0v1, v1v2, v2v0 }, out var f0);

            v0v1.Face.Should().BeSameAs(f0);
            v1v2.Face.Should().BeSameAs(f0);
            v2v0.Face.Should().BeSameAs(f0);
        }

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