using System;
using System.Collections.Generic;
using System.Linq;
using Heds.Operations;
using Heds.Selections;
using Heds.Utilities;
using UnityEngine;

namespace Heds
{
    public class Mesh
    {
        public IReadOnlyList<Vertex> Vertices => _vertices;
        private readonly List<Vertex> _vertices = new List<Vertex>();
        private int _nextVertexId;

        public IReadOnlyList<HalfEdge> HalfEdges => _halfEdges;
        private readonly List<HalfEdge> _halfEdges = new List<HalfEdge>();
        private int _nextHalfEdgeId;

        public IReadOnlyList<Face> Faces => _faces;
        private readonly List<Face> _faces = new List<Face>();
        private int _nextFaceId;

        private int TakeId(ref int idCounter)
        {
            var newId = idCounter;
            idCounter++;
            return newId;
        }

        private int TakeVertexId() => TakeId(ref _nextVertexId);
        private int TakeHalfEdgeId() => TakeId(ref _nextHalfEdgeId);
        private int TakeFaceId() => TakeId(ref _nextFaceId);

        public Vertex AddVertex(Vector3 position)
        {
            var newId = TakeVertexId();
            var newVertex = new Vertex(this, newId, position);
        
            _vertices.Add(newVertex);
            return newVertex;
        }
        
        public Mesh AddVertex(Vector3 position, out Vertex newVertex)
        {
            newVertex = AddVertex(position);
            return this;
        }

        public Mesh AddVertex(float x, float y, float z, out Vertex newVertex)
        {
            return AddVertex(new Vector3(x, y, z), out newVertex);
        }

        public Vertex[] AddVertices(params Vector3[] positions)
        {
            var vertices = positions
                .Select(AddVertex)
                .ToArray();

            return vertices;
        }

        /// <summary>
        /// Detaches a vertex from this mesh. Will remove any incident half-edges
        /// and faces.
        /// </summary>
        public void DetachVertex(Vertex vertex)
        {
            vertex.Detach();
            _vertices.Remove(vertex);
        }

        public void DetachVertices(params Vertex[] vertices)
        {
            foreach (var vertex in vertices)
            {
                vertex.Detach();
            }
            
            _vertices.RemoveAll(vertices.Contains);
        }
        
        /// <summary>
        /// Detaches a face from this mesh.
        /// </summary>
        public void DetachFace(Face face, bool removeHalfEdges = false)
        {
            if (removeHalfEdges)
            {
                _halfEdges.RemoveAll(he => face.HalfEdges.Contains(he));
                foreach (var halfEdge in face.HalfEdges)
                {
                    halfEdge.Detach();
                }
            }
            else
            {
                face.Detach();
            }

            _faces.Remove(face);
        }

        public void DetachHalfEdge(HalfEdge halfEdge)
        {
            halfEdge.Detach();
            _halfEdges.Remove(halfEdge);
        }
        
        /// <summary>
        /// Removes all half-edges from this mesh. Will also remove
        /// all faces in the process.
        /// </summary>
        public void DetachHalfEdgesAndFaces()
        {
            foreach (var halfEdge in _halfEdges)
            {
                halfEdge.Detach();
            }
            
            _faces.Clear();
            _halfEdges.Clear();
        }
        
        /// <summary>
        /// Detaches all faces from this mesh.
        /// </summary>
        public void DetachAllFaces()
        {
            foreach (var face in _faces)
            {
                face.Detach();
            }
            _faces.Clear();
        }

        /// <summary>
        /// Detaches all vertices, half-edges and faces from this mesh.
        /// </summary>
        public void DetachAll()
        {
            foreach (var vertex in _vertices)
            {
                vertex.Detach();
            }
            
            _vertices.Clear();
            _halfEdges.Clear();
            _faces.Clear();
        }

        public HalfEdge AddHalfEdge(Vertex from, Vertex to)
        {
            var newId = TakeHalfEdgeId();

            var newHalfEdge = new HalfEdge(newId, from, to);
            _halfEdges.Add(newHalfEdge);
            
            from.AddOutgoingHalfEdge(newHalfEdge);
            to.AddIncomingHalfEdge(newHalfEdge);

            if (TryGetHalfEdge(to, from, out var existingTwin))
            {
                existingTwin.SetTwin(newHalfEdge);
                newHalfEdge.SetTwin(existingTwin);
            }

            return newHalfEdge;
        }
        
        public Mesh AddHalfEdge(Vertex from, Vertex to, out HalfEdge newHalfEdge)
        {
            newHalfEdge = AddHalfEdge(from, to);
            return this;
        }

        public Mesh AddPairOfHalfEdges(Vertex from, Vertex to, out HalfEdge newHalfEdge, out HalfEdge newTwinHalfEdge)
        {
            AddHalfEdge(from, to, out newHalfEdge);
            AddHalfEdge(to, from, out newTwinHalfEdge);
            return this;
        }

        private bool TryGetHalfEdge(Vertex from, Vertex to, out HalfEdge halfEdge)
        {
            halfEdge = from.OutgoingHalfEdges.FirstOrDefault(he => he.To.Equals(to));
            return halfEdge != null;
        }

        private HalfEdge GetOrCreateHalfEdge(Vertex from, Vertex to)
        {
            if (TryGetHalfEdge(from, to, out var existing))
            {
                return existing;
            }
            else
            {
                AddHalfEdge(from, to, out var newHalfEdge);
                return newHalfEdge;
            }
        }

        public Face AddFace(HalfEdge[] halfEdges)
        {
            foreach (var halfEdge in halfEdges)
            {
                if (halfEdge.Face != null)
                {
                    throw new InvalidOperationException($"Cannot add a face to half-edge {halfEdge} - it is already attached to face {halfEdge.Face}");
                }
            }
            
            var newFace = new Face(TakeFaceId(), halfEdges);

            _faces.Add(newFace);

            foreach (var halfEdge in halfEdges)
            {
                halfEdge.SetFace(newFace);
            }
            
            return newFace;
        }
        
        public Mesh AddFace(HalfEdge[] halfEdges, out Face newFace)
        {
            newFace = AddFace(halfEdges);
            return this;
        }

        /// <summary>
        /// A convenience method that allows for easier creation of faces. Uses existing half-edges
        /// if they are present and creates new ones when required.
        /// </summary>
        public Face AddFace(Vertex[] vertices)
        {
            var halfEdges = vertices
                .Select((v, i) =>
                {
                    var from = v;
                    var to = vertices[(i + 1) % vertices.Length];

                    return GetOrCreateHalfEdge(from, to);
                })
                .ToArray();

            return AddFace(halfEdges);
        }

        /// <summary>
        /// A convenience method that allows for easier creation of faces. Uses existing half-edges
        /// if they are present and creates new ones when required.
        /// </summary>
        public Mesh AddFace(Vertex[] vertices, out Face newFace)
        {
            newFace = AddFace(vertices);
            return this;
        }
        
        /// <summary>
        /// Scales a mesh.
        /// </summary>
        public Mesh Scale(Vector3 scale)
        {
            new ScaleOperation(scale).Apply(this);
            return this;
        }
        
        /// <summary>
        /// Scales a mesh equally in all directions.
        /// </summary>
        public Mesh Scale(float scale)
        {
            new ScaleOperation(scale).Apply(this);
            return this;
        }
        
        /// <summary>
        /// Scales a mesh.
        /// </summary>
        public Mesh Scale(float scaleX, float scaleY, float scaleZ)
        {
            new ScaleOperation(scaleX, scaleY, scaleZ).Apply(this);
            return this;
        }
        
        /// <summary>
        /// Translates (moves) a mesh.
        /// </summary>
        public Mesh Translate(Vector3 translation)
        {
            new TranslateOperation(translation).Apply(this);
            return this;
        }
        
        /// <summary>
        /// Builds a new mesh where every vertex has been moved inwards or outwards
        /// until it falls on the surface of a sphere of the given radius. In other
        /// words, makes the mesh into a sphere.
        /// </summary>
        public Mesh ProjectVerticesToSphere(float radius)
        {
            new ProjectVerticesToSphereOperation(radius).Apply(this);
            return this;
        }
        
        /// <summary>
        /// Builds a new sphere where all vertices have been clamped within two radii
        /// (based on the origin) - an inner radius and an outer radius.
        /// </summary>
        public Mesh ClampVerticesWithinRadii(float innerRadius, float outerRadius)
        {
            new ClampVerticesWithinRadiiOperation(innerRadius, outerRadius).Apply(this);
            return this;
        }
        
        /// <summary>
        /// Builds a new mesh where each triangle has been subdivided into multiple triangles.
        /// For each level of subdivision, the number of triangles is multiplied by 4.
        /// </summary>
        public Mesh SubdivideTriangles(int numLevels = 1)
        {
            new SubdivideTrianglesOperation(numLevels).Apply(this);
            return this;
        }
        
        // /// <summary>
        // /// Filters a mesh down to only the requested faces.
        // /// </summary>
        // public Mesh Filter(FaceSelection selection)
        // {
        //     return new FilterFacesOperation(selection).Apply(this);
        // }
        //
        // /// <summary>
        // /// Filters a mesh down to only the requested faces.
        // /// </summary>
        // public Mesh Filter(params Face[] faces)
        // {
        //     return Filter(new FaceSelection(faces));
        // }
        
        /// <summary>
        /// Rebuilds the faces of a mesh so that all the faces are triangles.
        /// </summary>
        public Mesh Triangulate()
        {
            new TriangulateOperation().Apply(this);
            return this;
        }
        
        /// <summary>
        /// Creates the geometric dual of a mesh, i.e. a mesh where each
        /// face is replaced by a vertex and each vertex is replaced with
        /// a face.
        /// </summary>
        public Mesh GeometricDual()
        {
            new GeometricDualOperation().Apply(this);
            return this;
        }

        /// <summary>
        /// Extrudes some faces towards or away from the origin point.
        /// </summary>
        public Mesh SphericalExtrude(FaceSelection selection, float distance)
        {
            new SphericalExtrudeOperation(selection, distance).Apply(this);
            return this;
        }
        
        /// <summary>
        /// Returns the smallest axis-aligned bounding box that encompasses this mesh.
        /// </summary>
        public Bounds GetBoundingBox()
        {
            if (!Vertices.Any())
                return new Bounds();

            var minX = float.MaxValue;
            var maxX = float.MinValue;
            var minY = float.MaxValue;
            var maxY = float.MinValue;
            var minZ = float.MaxValue;
            var maxZ = float.MinValue;

            foreach (var vertex in Vertices)
            {
                var position = vertex.Position;
                minX = Mathf.Min(minX, position.x);
                maxX = Mathf.Max(maxX, position.x);
                minY = Mathf.Min(minY, position.y);
                maxY = Mathf.Max(maxY, position.y);
                minZ = Mathf.Min(minZ, position.z);
                maxZ = Mathf.Max(maxZ, position.z);
            }

            var size = new Vector3(
                maxX - minX,
                maxY - minY,
                maxZ - minZ
            );

            var center = new Vector3(
                (minX + maxX) / 2f,
                (minY + maxY) / 2f,
                (minZ + maxZ) / 2f
            );
            
            return new Bounds(center, size);
        }

        /// <summary>
        /// Creates a deep clone of this mesh. All new (but equivalent) faces, half-edges
        /// and vertices will be created.
        /// </summary>
        public Mesh Clone()
        {
            var newMesh = new Mesh();

            var vertexMap = _vertices
                .ToDictionary(
                    v => v,
                    v => newMesh.AddVertex(v.Position)
                );

            var halfEdgeMap = _halfEdges
                .ToDictionary(
                    he => he,
                    he => newMesh.AddHalfEdge(
                        vertexMap[he.From],
                        vertexMap[he.To]
                    )
                );

            foreach (var oldFace in _faces)
            {
                var halfEdges = oldFace.HalfEdges
                    .Select(he => halfEdgeMap[he])
                    .ToArray();
                
                newMesh.AddFace(halfEdges);
            }

            return newMesh;
        }
        
        /// <summary>
        /// Builds a Unity mesh from this instance.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public UnityEngine.Mesh ToUnityMesh()
        {
            if (Faces.Any(f => !f.IsTriangle))
            {
                throw new InvalidOperationException(
                    $"One or more faces in the mesh is not a triangle. Consider using the {nameof(Triangulate)} method to ensure that faces are triangles."
                );
            }
            
            var vertices = _vertices
                .Select(v => v.Position)
                .ToArray();

            var vertexIndices = _vertices
                .ToDictionary((v, i) => i);

            var triangles = _faces
                .SelectMany(f => f.Vertices.Select(v => vertexIndices[v]))
                .ToArray();
            
            var mesh = new UnityEngine.Mesh
            {
                name = "Generated Mesh",
                vertices = vertices,
                triangles = triangles
            };
            
            return mesh;
        }
    }
}