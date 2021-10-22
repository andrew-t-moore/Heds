using System.Linq;
using FluentAssertions;
using Heds.Primitives;
using UnityEngine;
using Xunit;

namespace Heds.Tests.Primitives
{
    public class QuadCubeTests
    {
        [Fact]
        public void CubeHas8Vertices()
        {
            QuadCube.Create().Vertices.Count.Should().Be(8);
        }

        [Fact]
        public void EachVertexHas3IncomingHalfEdges()
        {
            var vertices = QuadCube.Create().Vertices;
            vertices.Should().OnlyContain(v => v.IncomingHalfEdges.Count == 3);
        }

        [Fact]
        public void EachVertexHas3OutgoingHalfEdges()
        {
            var vertices = QuadCube.Create().Vertices;
            vertices.Should().OnlyContain(v => v.OutgoingHalfEdges.Count == 3);
        }

        [Fact]
        public void CubeIsCreatedAroundTheOrigin()
        {
            var vertices = QuadCube.Create().Vertices;
            var distancesFromOrigin = vertices
                .Select(v => v.Position.magnitude)
                .ToArray();

            var firstDistanceFromOrigin = distancesFromOrigin[0];

            distancesFromOrigin.Should()
                .OnlyContain(d => Mathf.Approximately(d, firstDistanceFromOrigin));
        }
        
        [Fact]
        public void CubeHas24HalfEdges()
        {
            var halfEdges = QuadCube.Create().HalfEdges;
            var lengths = halfEdges
                .Select(he => he.Length)
                .ToArray();

            var firstLength = lengths[0];

            lengths.Should()
                .OnlyContain(l => Mathf.Approximately(l, firstLength));
        }

        [Fact]
        public void AllHalfEdgesAreTheSameLength()
        {
            QuadCube.Create().HalfEdges.Count.Should().Be(24);
        }
        
        [Fact]
        public void CubeHas6Faces()
        {
            QuadCube.Create().Faces.Count.Should().Be(6);
        }

        [Fact]
        public void AllFacesHave4HalfEdges()
        {
            var faces = QuadCube.Create().Faces;
            faces.Should().OnlyContain(f => f.HalfEdges.Count == 4);
        }

        [Fact]
        public void AllFacesHave4AdjacentFaces()
        {
            var faces = QuadCube.Create().Faces;
            faces.Should().OnlyContain(f => f.GetAdjacentFaces().Count() == 4);
        }
    }
}