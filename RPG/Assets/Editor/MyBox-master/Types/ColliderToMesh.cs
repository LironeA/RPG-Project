using System.Collections.Generic;
using UnityEngine;

namespace MyBox
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ColliderToMesh : MonoBehaviour
    {
        private void Start()
        {
            FillShape();
        }

        [ButtonMethod]
        public void FillShape()
        {
            var pc2 = gameObject.GetComponent<PolygonCollider2D>();
            var pointCount = pc2.GetTotalPointCount();

            var mf = GetComponent<MeshFilter>();
            var mesh = new Mesh();
            var points = pc2.points;
            var vertices = new Vector3[pointCount];
            for (var j = 0; j < pointCount; j++)
            {
                var actual = points[j];
                vertices[j] = new Vector3(actual.x, actual.y, 0);
            }

            var tr = new Triangulator(points);
            var triangles = tr.Triangulate();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mf.mesh = mesh;

            mf.sharedMesh.RecalculateBounds();
        }

        [ButtonMethod]
        public void ClearShape()
        {
            var mf = GetComponent<MeshFilter>();
            mf.mesh = null;
        }


        #region Triangulator Class

        private class Triangulator
        {
            private readonly List<Vector2> m_points;

            public Triangulator(Vector2[] points)
            {
                m_points = new List<Vector2>(points);
            }

            public int[] Triangulate()
            {
                var indices = new List<int>();

                var n = m_points.Count;
                if (n < 3)
                    return indices.ToArray();

                var V = new int[n];
                if (Area() > 0)
                    for (var v = 0; v < n; v++)
                        V[v] = v;
                else
                    for (var v = 0; v < n; v++)
                        V[v] = n - 1 - v;

                var nv = n;
                var count = 2 * nv;
                for (var v = nv - 1; nv > 2;)
                {
                    if (count-- <= 0)
                        return indices.ToArray();

                    var u = v;
                    if (nv <= u)
                        u = 0;
                    v = u + 1;
                    if (nv <= v)
                        v = 0;
                    var w = v + 1;
                    if (nv <= w)
                        w = 0;

                    if (Snip(u, v, w, nv, V))
                    {
                        int s, t;
                        var a = V[u];
                        var b = V[v];
                        var c = V[w];
                        indices.Add(a);
                        indices.Add(b);
                        indices.Add(c);

                        for (s = v, t = v + 1; t < nv; s++, t++)
                            V[s] = V[t];
                        nv--;
                        count = 2 * nv;
                    }
                }

                indices.Reverse();
                return indices.ToArray();
            }

            private float Area()
            {
                var n = m_points.Count;
                var A = 0.0f;
                for (int p = n - 1, q = 0; q < n; p = q++)
                {
                    var pval = m_points[p];
                    var qval = m_points[q];
                    A += pval.x * qval.y - qval.x * pval.y;
                }

                return A * 0.5f;
            }

            private bool Snip(int u, int v, int w, int n, int[] V)
            {
                int p;
                var A = m_points[V[u]];
                var B = m_points[V[v]];
                var C = m_points[V[w]];
                if (Mathf.Epsilon > (B.x - A.x) * (C.y - A.y) - (B.y - A.y) * (C.x - A.x))
                    return false;
                for (p = 0; p < n; p++)
                {
                    if (p == u || p == v || p == w)
                        continue;
                    var P = m_points[V[p]];
                    if (InsideTriangle(A, B, C, P))
                        return false;
                }

                return true;
            }

            private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
            {
                var ax = C.x - B.x;
                var ay = C.y - B.y;
                var bx = A.x - C.x;
                var @by = A.y - C.y;
                var cx = B.x - A.x;
                var cy = B.y - A.y;
                var apx = P.x - A.x;
                var apy = P.y - A.y;
                var bpx = P.x - B.x;
                var bpy = P.y - B.y;
                var cpx = P.x - C.x;
                var cpy = P.y - C.y;

                var aCROSSbp = ax * bpy - ay * bpx;
                var cCROSSap = cx * apy - cy * apx;
                var bCROSScp = bx * cpy - @by * cpx;

                return aCROSSbp >= 0.0f && bCROSScp >= 0.0f && cCROSSap >= 0.0f;
            }
        }

        #endregion
    }
}