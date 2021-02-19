using System;
using Cyborg.Core;
using Cyborg.Utilities;
using Microsoft.Xna.Framework;

namespace Cyborg.Components
{
    public class BodyComponent
    {
        public BodyComponent(Vector2 position, Point size = default, Edge edges = Edge.None)
        {
            Position = position;
            Size = size;
            Edges = edges;
        }

        public Vector2 Position { get; set; }
        public Point Size { get; }
        public Edge Edges { get; }
    }

    [Flags]
    public enum Edge
    {
        None = 0,
        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8
    }

    public interface IBody : IEntity
    {
        BodyComponent Body { get; }
    }
}