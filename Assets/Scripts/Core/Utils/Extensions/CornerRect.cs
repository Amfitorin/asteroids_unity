using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils.Extensions
{
    public struct CornerRect : IEnumerable<Vector2>, IEnumerator<Vector2>
    {
        private const int Length = 4;
        public readonly Rect Rect;
        private Vector2 bottomLeft;
        private Vector2 topLeft;
        private Vector2 topRight;
        private Vector2 bottomRight;

        private int _position;
        private Vector2 _current;

        public CornerRect(Rect rect) : this()
        {
            Rect = rect;
            _position = -1;
            ApplyRect(rect);
        }

        public CornerRect(Bounds bounds) : this()
        {
            var rect = new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);
            Rect = rect;
            _position = -1;
            ApplyRect(rect);
        }

        public Vector2 this[int index]
        {
            get
            {
                return index switch
                {
                    0 => bottomLeft,
                    1 => topLeft,
                    2 => topRight,
                    3 => bottomRight,
                    _ => throw new InvalidOperationException()
                };
            }
        }

        public void ApplyRect(Rect rect)
        {
            bottomLeft = rect.min;
            topLeft = rect.min + Vector2.up * rect.size.y;
            topRight = rect.max;
            bottomRight = rect.max + Vector2.down * rect.size.y;
        }

        public static implicit operator CornerRect(Bounds bounds)
        {
            return new CornerRect(bounds);
        }

        public static implicit operator CornerRect(Rect rect)
        {
            return new CornerRect(rect);
        }

        public bool MoveNext()
        {
            _position++;
            return _position < Length;
        }

        public void Reset()
        {
            _position = -1;
        }

        object IEnumerator.Current => Current;

        public Vector2 Current
        {
            get
            {
                if (_position is >= 0 and < Length)
                {
                    return this[_position];
                }

                throw new InvalidOperationException();
            }
        }

        public void Dispose()
        {
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}