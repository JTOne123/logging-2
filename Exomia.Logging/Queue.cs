#region License

// Copyright (c) 2018-2019, exomia
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

#endregion

using System;
using System.Runtime.CompilerServices;

namespace Exomia.Logging
{
    /// <summary>
    ///     A queue. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    sealed class Queue<T>
    {
        /// <summary>
        ///     The default capacity.
        /// </summary>
        private const int DEFAULT_CAPACITY = 8;
        /// <summary>
        ///     The maximum capacity.
        /// </summary>
        private const int MAX_CAPACITY     = 0X7FEFFFFF;

        /// <summary>
        ///     The array.
        /// </summary>
        private T[] _array;
        /// <summary>
        ///     The head.
        /// </summary>
        private int _head;
        /// <summary>
        ///     The tail.
        /// </summary>
        private int _tail;
        /// <summary>
        ///     Number of.
        /// </summary>
        private int _count;

        /// <summary>
        ///     Gets the number of. 
        /// </summary>
        /// <value>
        ///     The count.
        /// </value>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        ///     Initializes a new instance of the &lt;see cref="Queue&lt;T&gt;"/&gt; class.
        /// </summary>
        /// <param name="capacity"> The capacity. </param>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
        ///                                                the required range. </exception>
        public Queue(int capacity)
        {
            if (capacity < 0) { throw new ArgumentOutOfRangeException(nameof(capacity)); }

            _array = new T[capacity];
            _head  = 0;
            _tail  = 0;
            _count = 0;
        }

        /// <summary>
        ///     Initializes a new instance of the &lt;see cref="Queue&lt;T&gt;"/&gt; class.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        public Queue(Queue<T> other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            _array = new T[other._array.Length];

            if (other._count > 0)
            {
                if (other._head < other._tail)
                {
                    Array.Copy(other._array, other._head, _array, 0, other._count);
                }
                else
                {
                    Array.Copy(other._array, _head, _array, 0, other._array.Length - other._head);
                    Array.Copy(other._array, 0, _array, other._array.Length        - other._head, other._tail);
                }
            }

            _count = other._count;
            _head  = 0;
            _tail  = _count == _array.Length ? 0 : _count;
        }

        /// <summary>
        ///     Adds an object onto the end of this queue.
        /// </summary>
        /// <param name="item"> The item. </param>
        public void Enqueue(T item)
        {
            EnsureCapacity(_count + 1);

            _array[_tail] = item;
            _tail         = (_tail + 1) % _array.Length;
            _count++;
        }

        /// <summary>
        ///     Removes the head object from this queue.
        /// </summary>
        /// <returns>
        ///     The head object from this queue.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"> Thrown when the index is outside the required
        ///                                             range. </exception>
        public T Dequeue()
        {
            if (_count == 0) { throw new IndexOutOfRangeException(); }

            T removed = _array[_head];
            _array[_head] = default;
            _head         = (_head + 1) % _array.Length;
            _count--;

            return removed;
        }

        /// <summary>
        ///     Clears this object to its blank/initial state.
        /// </summary>
        public void Clear()
        {
            if (_head < _tail)
            {
                Array.Clear(_array, _head, _count);
            }
            else
            {
                Array.Clear(_array, _head, _array.Length - _head);
                Array.Clear(_array, 0, _tail);
            }

            _head  = 0;
            _tail  = 0;
            _count = 0;
        }

        /// <summary>
        ///     Clears this object to its blank/initial state.
        /// </summary>
        /// <param name="other"> The other. </param>
        public void Clear(Queue<T> other)
        {
            if (_array.Length < other._count)
            {
                _array = new T[other._count];
            }
            else
            {
                Array.Clear(_array, other._count, _array.Length - other._count);
            }

            if (other._count > 0)
            {
                if (other._head < other._tail)
                {
                    Array.Copy(other._array, other._head, _array, 0, other._count);
                }
                else
                {
                    Array.Copy(other._array, _head, _array, 0, other._array.Length - other._head);
                    Array.Copy(other._array, 0, _array, other._array.Length        - other._head, other._tail);
                }
            }

            _count = other._count;
            _head  = 0;
            _tail  = _count == _array.Length ? 0 : _count;
        }

        /// <summary>
        ///     Ensures that capacity.
        /// </summary>
        /// <param name="min"> The minimum. </param>
        /// <exception cref="OutOfMemoryException"> Thrown when a low memory situation occurs. </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacity(int min)
        {
            if (_array.Length < min)
            {
                int newCapacity = _array.Length == 0 ? DEFAULT_CAPACITY : _array.Length * 2;
                if (newCapacity > MAX_CAPACITY) { newCapacity = MAX_CAPACITY; }
                if (newCapacity < min) { throw new OutOfMemoryException(); }

                T[] buffer = new T[newCapacity];
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, buffer, 0, _count);
                }
                else
                {
                    Array.Copy(_array, _head, buffer, 0, _array.Length - _head);
                    Array.Copy(_array, 0, buffer, _array.Length        - _head, _tail);
                }
                _array = buffer;
                _head  = 0;
                _tail  = _count == newCapacity ? 0 : _count;
            }
        }
    }
}