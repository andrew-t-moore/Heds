# Heds
A Half-Edge data structure for Unity. You can learn about Half Edge data structures [here](https://cs184.eecs.berkeley.edu/sp19/article/15/the-half-edge-data-structure).

This library allows you to build up meshes vertex by vertex, face by face. It's not a replacement for 3D modelling tools - rather it gives you the ability to create and manipulate shapes using a fairly simple object model.

# Compatibility
This library is compatible with Unity `2020.3.21f1`. It doesn't hit much of the Unity API surface area, so I expect to to be reasonably future proof.

# Examples
If you want to generate a sphere, you could do something like this:

```csharp
var unityMesh = Icosahedron.Create()
  .Subdivide()
  .Subdivide()
  .GeometricDual()
  .Triangulate()
  .ProjectToSphere(sphereRadius)
  .ToUnityMesh();
```

That's all there is to it. There are loads more operations defined on the `Mesh` class if you want them, and you can easily define your own.

Suppose you want something like the above, but you need two meshes:

- A logical mesh, where faces are hexagons and pentagons, and you can use the object model to easily navigate the mesh
- A physical mesh, which is like the logical mesh but the faces have been broken down into triangles so unity can render it

You could do that like this:

```csharp
var logicalMesh = Icosahedron.Create()
  .Subdivide()
  .Subdivide()
  .GeometricDual()
  .ProjecToSphere(sphereRadius);

var unityMesh = logicalMesh
  .Clone()
  .Triangulate()
  .ToUnityMesh();
```

Or if you want to generate a sphere based on a cube instead of an icosahedron:

```csharp
var unityMesh = QuadCube.Create()
  .Triangulate()
  .Subdivide()
  .Subdivide()
  .ProjectToSphere(sphere)
  .ToUnityMesh();
```

# Custom shapes
If you want to generate your own shape instead of starting from a common shape, you can do that too:

```csharp
new Mesh()
    .AddVertex(0f, 0f, 0f, out var v0)
    .AddVertex(1f, 0f, 0f, out var v1)
    .AddVertex(1f, 1f, 0f, out var v2)
    .AddVertex(0f, 1f, 0f, out var v3)
    .AddFace(new[] { v0, v1, v2 }, out var f0)
    .AddFace(new[] { v0, v2, v3 }, out var f1);
```

This is what the built-in primitives are doing under the hood.

- See [Icosahedron.cs](Heds/Primitives/Icosahedron.cs)
- See [QuadCube.cs](Heds/Primitives/QuadCube.cs)

# Navigating meshes
Once you've built a mesh, you can navigate and modify the mesh easily. Some examples:

```csharp
myMesh.Faces[0].GetAdjacentFaces();
```

```csharp
myMesh.Vertices[0].IncomingHalfEdges;
```

```csharp
myMesh.HalfEdges[0].Twin.Face;
```
