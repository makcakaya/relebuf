﻿using System;
using System.Linq;
using Xunit;

namespace Relebuf.Tests
{
    public sealed class CircularBufferTests
    {
        [Fact]
        public void CanBuildValid()
        {
            new CircularBuffer<int>(100);
        }

        [Fact]
        public void BuildInvalidThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CircularBuffer<int>(0));
        }

        [Fact]
        public void InsertDoesNotOverwriteUnderCapacity()
        {
            uint capacity = 100;
            var buffer = new CircularBuffer<int>(capacity);
            for (var i = 0; i < capacity; i++)
            {
                buffer.Insert(i);
            }

            Assert.Equal(0, buffer.First());
        }

        [Fact]
        public void InsertOverwritesAboveCapacity()
        {
            uint capacity = 100;
            var buffer = new CircularBuffer<int>(capacity);
            for (var i = 0; i < capacity + 10; i++)
            {
                buffer.Insert(i);
            }

            Assert.Equal(10, buffer.ElementAt(0));
            Assert.Equal(11, buffer.ElementAt(1));
        }

        [Fact]
        public void DoesNotThrowWhenOverwritesMultipleTimes()
        {
            uint capacity = 100;
            var loopCount = 1000;
            var totalInserts = capacity * loopCount;

            var buffer = new CircularBuffer<int>(capacity);

            for (var i = 0; i < totalInserts; i++)
            {
                buffer.Insert(i);
            }

            Assert.Equal(totalInserts - 1, buffer.ElementAt((int)capacity - 1));
        }

        [Fact]
        public void ElementAtThrowsIfBeyondCapacity()
        {
            var buffalo = new CircularBuffer<int>(5);

            Assert.Throws<ArgumentOutOfRangeException>(() => buffalo.ElementAt(5));
        }

        [Fact]
        public void MeaninglessTest()
        {
            uint capacity = 100;
            var loopCount = 1000;
            var totalInsertCount = capacity * loopCount;
            var buffer = new CircularBuffer<int>(capacity);

            for (var i = 0; i < totalInsertCount; i++)
            {
                buffer.Insert(i);
                Assert.Equal(Math.Max(0, i - (capacity - 1)), buffer.First());
            }
        }
    }
}