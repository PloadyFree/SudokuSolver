﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SudokuSolver.Tests
{
    public abstract class TestBase
    {
        protected readonly Random rnd = new Random();
        protected static readonly Func<IGameField, int, int, int> Indexer
            = (field0, x, y) => y * field0.Width + x;

        [SetUp]
        public virtual void SetUp()
        {
        }

        protected static void Fill(ref IGameField field, Func<IGameField, int, int, int> getNumber)
        {
            foreach (var x in Enumerable.Range(0, field.Width))
                foreach (var y in Enumerable.Range(0, field.Height))
                    field = field.SetElementAt(x, y, getNumber(field, x, y));
        }

        protected static IGameField GameFieldFromLines(IEnumerable<string> lines)
        {
            var fieldData = lines
                .Select(line => line.Split(' ').Select(int.Parse).ToList())
                .ToList();
            var width = fieldData[0].Count;
            var height = fieldData.Count;
            IGameField field = new GameField(width, height);

            foreach (var x in Enumerable.Range(0, width))
                foreach (var y in Enumerable.Range(0, height))
                    field = field.SetElementAt(x, y, fieldData[y][x]);

            return field;
        }
    }
}
