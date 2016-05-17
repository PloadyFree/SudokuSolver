﻿using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace SudokuSolver.Tests
{
    [TestFixture]
    public class GameField_Should : TestBase
    {
        private IGameField field;
        private Func<int, int, int> byRowNumerator;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            field = new GameField(5, 6);
            byRowNumerator = GetByRowEnumerator(field);
        }

        [Test]
        public void SaySizeCorrectly_WhenSizeIsCorrect()
        {
            var height = 5;
            var width = 6;
            field = new GameField(height, width);

            field.Height.Should().Be(height);
            field.Width.Should().Be(width);
        }

        [Test, Combinatorial]
        public void FailCreating_WhenSizeIsNotCorrect(
            [Values(-5, 0, 5)] int width,
            [Values(-6, 0, 6)] int height)
        {
            if (height > 0 && width > 0)
                return;
            Action create = () => new GameField(height, width);
            create.ShouldThrow<Exception>();
        }

        [Test]
        public void RememberCellValuesCorrectly()
        {
            field = field.Fill(byRowNumerator);

            foreach (var row in Enumerable.Range(0, field.Height))
                foreach (var column in Enumerable.Range(0, field.Width))
                    field.GetElementAt(row, column).Should().Be(byRowNumerator(row, column));
        }

        [Test]
        public void ReturnRealValuesOnGetRow()
        {
            field = field.Fill(byRowNumerator);

            foreach (var row in Enumerable.Range(0, field.Height))
            {
                var rowValues = field.GetRow(row).ToList();
                rowValues.Count.Should().Be(field.Width);
                foreach (var column in Enumerable.Range(0, field.Width))
                    rowValues[column].Should().Be(byRowNumerator(row, column));
            }
        }

        [Test]
        public void ReturnRealValuesOnGetColumn()
        {
            field = field.Fill(byRowNumerator);

            foreach (var column in Enumerable.Range(0, field.Width))
            {
                var columnValues = field.GetColumn(column).ToList();
                columnValues.Count.Should().Be(field.Height);
                foreach (var row in Enumerable.Range(0, field.Height))
                    columnValues[row].Should().Be(byRowNumerator(row, column));
            }
        }

        [Test]
        public void HaveZeroValues_ByDefault()
        {
            Enumerable.Range(0, field.Height)
                .SelectMany(row => field.GetRow(row))
                .ShouldAllBeEquivalentTo(0);
        }

        [Test]
        public void SayThatItsNotFilled_WhenJustCreated()
        {
            field.Filled.Should().BeFalse();
        }

        [Test]
        public void SayThatItsNotFilled_WhenThereAreSomeZeroCells()
        {
            field = field.Fill(byRowNumerator);
            field = field
                .SetElementAt(2, 3, 0)
                .SetElementAt(0, 0, 0);
            field.Filled.Should().BeFalse();
        }

        [Test]
        public void SayThatItsFilled_WhenThereAreNoZeroCells()
        {
            field = field.Fill(byRowNumerator);
            field = field.SetElementAt(0, 0, 1);
            field.Filled.Should().BeTrue();
        }

        [Test]
        public void ReturnGameFieldClass_OnSetElementAtMethod()
        {
            field.SetElementAt(0, 0, 123).GetType().Should().Be<GameField>();
        }
    }
}
