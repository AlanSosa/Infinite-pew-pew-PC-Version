using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ThridWolrdShooterGame.Extensions
{
    /// <summary>
    /// Batch-draws a series of lines defined by 3d points (and more). Much faster than drawing one-by-one.
    /// </summary>
    /// <typeparam name="VertexType">The vertex type to use (such as VertexPositionColor)</typeparam>
    public class LineBatch<VertexType> where VertexType : struct, IVertexType
    {
        private GraphicsDevice _graphicsDevice;
        private Effect _effect;
        private String _technique;
        private List<VertexType> _vertices;
        private List<int> _indices;
        private int _count;
        private Boolean _state;

        /// <summary>
        /// Creates a new instance of LineBatch.
        /// </summary>
        /// <param name="graphicsDevice">A graphics device with which to draw.</param>
        /// <param name="effect">The effect to use to draw lines.</param>
        /// <param name="technique">The technique to use to draw lines.</param>
        public LineBatch(GraphicsDevice graphicsDevice, Effect effect, String technique)
        {
            _graphicsDevice = graphicsDevice;
            _effect = effect;
            _technique = technique;
            _state = false;

            _vertices = new List<VertexType>();
            _indices = new List<int>();
            _count = 0;
        }

        /// <summary>
        /// Begin drawing lines. Must be called before Draw(). Call End() when you're done.
        /// </summary>
        public void Begin()
        {
            if (_state == true) throw new Exception("Must call End() before calling Begin() again.");
            _state = true;
        }

        /// <summary>
        /// Draw a line from start to end. Must be called between Begin() and End().
        /// </summary>
        /// <param name="start">Start</param>
        /// <param name="end">End</param>
        public void Draw(VertexType start, VertexType end)
        {
            if (_state == false) throw new Exception("Must call Begin() before calling Draw().");
            _vertices.Add(start);
            _vertices.Add(end);
            _indices.Add(_count++);
            _indices.Add(_count++);
        }

        /// <summary>
        /// Draw a line-strip. Must be called between Begin() and End().
        /// </summary>
        /// <param name="verts">The list of vertices to draw.</param>
        public void Draw(IList<VertexType> verts)
        {
            if (_state == false) throw new Exception("Must call Begin() before calling Draw().");
            if (verts.Count < 2) throw new Exception("Must add at least two points.");

            int offset = _vertices.Count;

            _vertices.AddRange(verts);
            for (int i = 0; i < verts.Count - 1; ++i)
            {
                _indices.Add(i + offset);
                _indices.Add(i + offset + 1);
            }
            _count += verts.Count;
        }

        /// <summary>
        /// Draw an indexed list of lines. Must be called between Begin() and End().
        /// </summary>
        /// <param name="verts">The list of vertices to draw.</param>
        /// <param name="indices">The list of indices into the list of vertices.</param>
        public void Draw(IList<VertexType> verts, IList<int> indices)
        {
            if (_state == false) throw new Exception("Must call Begin() before calling Draw().");
            if (verts.Count < 2 || indices.Count < 2) throw new Exception("Must add at least two points.");

            int offset = _vertices.Count;

            _vertices.AddRange(verts);
            for (int i = 0; i < indices.Count; ++i)
            {
                _indices.Add(indices[i] + offset);
            }
            _count += _vertices.Count;
        }


        /// <summary>
        /// Finalizes the line batch and draws it to the GraphicsDevice. Must be called after Begin().
        /// </summary>
        public void End()
        {
            if (_state == false) throw new Exception("Must call Begin() before calling End().");
            _state = false;
            _effect.CurrentTechnique = _effect.Techniques[_technique];

            foreach (var p in _effect.CurrentTechnique.Passes)
            {
                p.Apply();
                _graphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList,
                    _vertices.ToArray(), 0, _vertices.Count,
                    _indices.ToArray(), 0, _indices.Count / 2);
            }
            _vertices.Clear();
            _indices.Clear();
            _count = 0;
        }

        /// <summary>
        /// Aborts any Begin() and Draw() calls, resetting the LineBatch to its initial state. Does not care when it is called.
        /// </summary>
        public void Abort()
        {
            _state = false;
            _vertices.Clear();
            _indices.Clear();
            _count = 0;
        }
    }
}