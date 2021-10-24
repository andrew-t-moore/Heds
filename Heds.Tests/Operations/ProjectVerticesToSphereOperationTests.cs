using FluentAssertions;
using Heds.Primitives;
using Heds.Tests.TestUtilities;
using UnityEngine;
using Xunit;

namespace Heds.Tests.Operations
{
    public class ProjectVerticesToSphereOperationTests
    {
        [Fact]
        public void VerticesArePushedToSurfaceOfSphere()
        {
            const float radius = 5f;

            QuadCube.Create()
                .Triangulate()
                .SubdivideTriangles()
                .SubdivideTriangles()
                .ProjectVerticesToSphere(radius)
                .Vertices
                .Should()
                .OnlyContain(v => Mathf.Approximately(v.Position.magnitude, radius));
        }
        
        [Fact]
        public void NewMeshIsTopologicallyTheSame()
        {
            const float radius = 5f;

            var oldMesh = QuadCube.Create()
                .Triangulate()
                .SubdivideTriangles()
                .SubdivideTriangles();
        
            var newMesh = oldMesh
                .ProjectVerticesToSphere(radius);

            MeshTopologies.AssertEquivalent(oldMesh, newMesh);
        }
    }
}