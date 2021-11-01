using FluentAssertions;
using Heds.Primitives;
using Heds.Tests.TestUtilities;
using UnityEngine;
using Xunit;

namespace Heds.Tests.Operations
{
    public class TranslateOperationTests
    {
        [Theory]
        [InlineData(1f, 0f, 0f)]
        [InlineData(0f, 1f, 0f)]
        [InlineData(0f, 0f, 1f)]
        public void TranslationMovesTheMesh(float x, float y, float z)
        {
            const float epsilon = 0.001f;
            
            var oldMesh = QuadCube.Create();
            var newMesh = oldMesh.Clone().Translate(new Vector3(x, y, z));

            var oldCenter = oldMesh.GetBoundingBox().center;
            var newCenter = newMesh.GetBoundingBox().center;

            newCenter.x.Should().BeApproximately(oldCenter.x + x, epsilon);
            newCenter.y.Should().BeApproximately(oldCenter.y + y, epsilon);
            newCenter.z.Should().BeApproximately(oldCenter.z + z, epsilon);
        }
        
        [Fact]
        public void NewMeshIsTopologicallyTheSame()
        {
            var oldMesh = QuadCube.Create()
                .Triangulate()
                .SubdivideTriangles()
                .SubdivideTriangles();
        
            var newMesh = oldMesh
                .Translate(new Vector3(1f, 0f, 0f));

            MeshTopologies.AssertEquivalent(oldMesh, newMesh);
        }
    }
}