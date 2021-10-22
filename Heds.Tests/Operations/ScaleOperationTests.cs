using System;
using FluentAssertions;
using Heds.Primitives;
using Heds.Tests.TestUtilities;
using UnityEngine;
using Xunit;

namespace Heds.Tests.Operations
{
    public class ScaleOperationTests
    {
        [Fact]
        public void MeshCanBeScaledDownByASingleFloat()
        {
            RunScaleTest(mesh => mesh.Scale(0.5f), 0.5f, 0.5f, 0.5f);
        }
        
        [Fact]
        public void MeshCanBeScaledUpByASingleFloat()
        {
            RunScaleTest(mesh => mesh.Scale(2f), 2f, 2f, 2f);
        }

        [Fact]
        public void MeshCanBeScaledDownByThreeFloats()
        {
            RunScaleTest(mesh => mesh.Scale(0.5f, 0.3f, 0.2f), 0.5f, 0.3f, 0.2f);
        }

        [Fact]
        public void MeshCanBeScaledUpByThreeFloats()
        {
            RunScaleTest(mesh => mesh.Scale(2f, 3f, 5f), 2f, 3f, 5f);
        }

        [Fact]
        public void MeshCanBeScaledDownByAVector()
        {
            RunScaleTest(mesh => mesh.Scale(new Vector3(0.5f, 0.3f, 0.2f)), 0.5f, 0.3f, 0.2f);
        }

        [Fact]
        public void MeshCanBeScaledUpByAVector()
        {
            RunScaleTest(mesh => mesh.Scale(new Vector3(2f, 3f, 5f)), 2f, 3f, 5f);
        }

        private void RunScaleTest(
            Func<Mesh, Mesh> transform,
            float expectedXScaleFactor,
            float expectedYScaleFactor,
            float expectedZScaleFactor
        )
        {
            var originalMesh = QuadCube.Create();
            var scaledMesh = transform(originalMesh);

            var originalBoundingBox = originalMesh.GetBoundingBox();
            var scaledBoundingBox = scaledMesh.GetBoundingBox();

            const float epsilon = 0.001f;
            
            scaledBoundingBox.center.x.Should().BeApproximately(originalBoundingBox.center.x, epsilon);
            scaledBoundingBox.center.y.Should().BeApproximately(originalBoundingBox.center.y, epsilon);
            scaledBoundingBox.center.z.Should().BeApproximately(originalBoundingBox.center.z, epsilon);

            scaledBoundingBox.size.x.Should().BeApproximately(originalBoundingBox.size.x * expectedXScaleFactor, epsilon);
            scaledBoundingBox.size.y.Should().BeApproximately(originalBoundingBox.size.y * expectedYScaleFactor, epsilon);
            scaledBoundingBox.size.z.Should().BeApproximately(originalBoundingBox.size.z * expectedZScaleFactor, epsilon);
        }
        
        [Fact]
        public void NewMeshIsTopologicallyTheSame()
        {
            var oldMesh = QuadCube.Create()
                .SubdivideTriangles()
                .SubdivideTriangles();
        
            var newMesh = oldMesh
                .Scale(2f);

            MeshTopologies.AssertEquivalent(oldMesh, newMesh);
        }
    }
}