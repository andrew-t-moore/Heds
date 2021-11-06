using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Heds.Tests.Operations
{
    public class SubdivideTrianglesOperationTests
    {
        private Mesh BuildMeshWithOneTriangularFace()
        {
            return new Mesh()
                .AddVertex(-1f, 0f, 1f, out var v0)
                .AddVertex(-1f, 0f, -1f, out var v1)
                .AddVertex(1f, 0f, -1f, out var v2)
                .AddFace(new[] { v0, v1, v2 }, out _);
        }

        private Mesh BuildMeshWithTwoTriangularFaces()
        {
            return new Mesh()
                .AddVertex(-1f, 0f, 1f, out var v0)
                .AddVertex(-1f, 0f, -1f, out var v1)
                .AddVertex(1f, 0f, -1f, out var v2)
                .AddVertex(1f, 0f, 1f, out var v3)
                .AddFace(new[] { v0, v1, v2 }, out _)
                .AddFace(new[] { v0, v2, v3 }, out _);
        }
        
        [Fact]
        public void ASingleFaceShouldSubdivideInto4Faces()
        {
            var oldMesh = BuildMeshWithOneTriangularFace(); 
            var newMesh = oldMesh.SubdivideTriangles();

            newMesh.Faces.Should().HaveCount(4);
        }

        [Fact]
        public void ThrowsExceptionOnNonTriangularFace()
        {
            var mesh = new Mesh()
                .AddVertex(-1f, 0f, 1f, out var v0)
                .AddVertex(-1f, 0f, -1f, out var v1)
                .AddVertex(1f, 0f, -1f, out var v2)
                .AddVertex(1f, 0f, 1f, out var v3)
                .AddFace(new[] { v0, v1, v2, v3 }, out _);

            mesh.Invoking(m => m.SubdivideTriangles())
                .Should()
                .ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void TwoFacesShouldSubdivideInto8Faces()
        {
            var mesh = BuildMeshWithTwoTriangularFaces()
                .SubdivideTriangles();

            var verticesByIncomingEdgeCount = mesh.Vertices
                .GroupBy(v => v.IncomingHalfEdges.Count)
                .ToDictionary(grp => grp.Key, grp => grp.Count());

            verticesByIncomingEdgeCount[1].Should().Be(2);
            verticesByIncomingEdgeCount[2].Should().Be(2);
            verticesByIncomingEdgeCount[3].Should().Be(4);
            verticesByIncomingEdgeCount[6].Should().Be(1);
            
            var verticesByOutgoingEdgeCount = mesh.Vertices
                .GroupBy(v => v.OutgoingHalfEdges.Count)
                .ToDictionary(grp => grp.Key, grp => grp.Count());

            verticesByOutgoingEdgeCount[1].Should().Be(2);
            verticesByOutgoingEdgeCount[2].Should().Be(2);
            verticesByOutgoingEdgeCount[3].Should().Be(4);
            verticesByOutgoingEdgeCount[6].Should().Be(1);
            
            mesh.Vertices.Should().HaveCount(9);
            mesh.Faces.Should().HaveCount(8);
            mesh.HalfEdges.Should().HaveCount(24);
        } 
    }
}