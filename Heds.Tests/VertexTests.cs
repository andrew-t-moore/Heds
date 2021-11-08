using System.Linq;
using FluentAssertions;
using UnityEngine;
using Xunit;

namespace Heds.Tests
{
    public class VertexTests
    {
        [Fact]
        public void AVertexIsInitiallyAttached()
        {
            new Mesh().AddVertex(Vector3.zero, out var v);
            v.IsDetached.Should().BeFalse();
        }
        
        [Fact]
        public void AVertexIsMarkedAsDetachedOnceDetached()
        {
            var mesh = new Mesh().AddVertex(Vector3.zero, out var v);

            v.IsDetached.Should().BeFalse();
            
            mesh.DetachVertex(v);

            v.IsDetached.Should().BeTrue();
        }

        [Fact]
        public void AVertexIsEqualToItself()
        {
            new Mesh().AddVertex(Vector3.zero, out var v);

            v.Should().Be(v);
        }
        
        [Fact]
        public void AVertexIsNotEqualToAnotherVertex()
        {
            new Mesh()
                .AddVertex(Vector3.zero, out var v0)
                .AddVertex(Vector3.back, out var v1);

            v0.Should().NotBe(v1);
        }

        [Fact]
        public void AVertexIsInitialisedWithNoHalfEdges()
        {
            new Mesh().AddVertex(Vector3.zero, out var v);

            v.IncomingHalfEdges.Should().BeEmpty();
            v.OutgoingHalfEdges.Should().BeEmpty();
        }
        
        [Fact]
        public void CanRetrieveIncidentHalfEdgesFromAVertex()
        {
            new Mesh()
                .AddVertex(Vector3.zero, out var v0)
                .AddVertex(Vector3.up, out var v1)
                .AddVertex(Vector3.right, out var v2)
                .AddHalfEdge(v0, v1, out var he0)
                .AddHalfEdge(v1, v2, out var he1)
                .AddHalfEdge(v2, v0, out var he2);

            v0.OutgoingHalfEdges.Should().ContainInOrder(he0);
            v0.IncomingHalfEdges.Should().ContainInOrder(he2);
            
            v1.OutgoingHalfEdges.Should().ContainInOrder(he1);
            v1.IncomingHalfEdges.Should().ContainInOrder(he0);
            
            v2.OutgoingHalfEdges.Should().ContainInOrder(he2);
            v2.IncomingHalfEdges.Should().ContainInOrder(he1);
        }
        
        [Fact]
        public void CanRetrieveIncidentFacesFromAVertex()
        {
            new Mesh()
                .AddVertex(Vector3.zero, out var v0)
                .AddVertex(Vector3.up, out var v1)
                .AddVertex(Vector3.right, out var v2)
                .AddVertex(Vector3.down, out var v3)
                .AddVertex(Vector3.left, out var v4)
                .AddFace(new[] { v0, v1, v2 }, out var f0)
                .AddFace(new[] { v0, v2, v3 }, out var f1)
                .AddFace(new[] { v0, v3, v4 }, out var f2)
                .AddFace(new[] { v0, v4, v1 }, out var f3);

            v0.GetIncidentFaces()
                .ToArray()
                .Should()
                .Contain(new[] { f0, f1, f2, f3 });
        }
    }
}