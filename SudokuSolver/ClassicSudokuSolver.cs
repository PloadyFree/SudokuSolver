﻿using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class ClassicSudokuSolver : ISudokuSolver
    {
        public int BlockHeight { get; protected set; }
        public int BlockWidth { get; protected set; }
        public int MaxNumber => BlockHeight * BlockWidth;

        public ClassicSudokuSolver(int blockHeight, int blockWidth)
        {
            BlockHeight = blockHeight;
            BlockWidth = blockWidth;
        }

        public IEnumerable<SudokuGameField> GetAllSolutions(SudokuGameField startState)
        {
            return FindSolutions(startState);
        }

        public SudokuGameField GetSolution(SudokuGameField startState)
        {
            var solution = GetAllSolutions(startState).Take(1).ToList();
            return solution.Any() ? solution.First() : null;
        }

        private IEnumerable<SudokuGameField> FindSolutions(SudokuGameField field)
        {
            if (field.Filled)
            {
                yield return field;
                yield break;
            }

            foreach (var position in field.EnumerateCellPositions())
            {
                if (field.GetElementAt(position) != 0)
                    continue;

                var availableNumbers = GetAvailableNumbers(field, position);
                var solutions = availableNumbers
                    .Select(newNumber => (SudokuGameField) field.SetElementAt(position, newNumber))
                    .SelectMany(FindSolutions);

                foreach (var solution in solutions)
                    yield return solution;
                yield break;
            }
        }

        private IEnumerable<int> GetAvailableNumbers(SudokuGameField field, CellPosition position)
        {
            var numbersInBlock = GetBlock(field, position);
            var numbersInRow = field.GetRow(position.Row);
            var numbersInColumn = field.GetColumn(position.Column);

            return Enumerable.Range(1, MaxNumber)
                .Except(numbersInBlock)
                .Except(numbersInRow)
                .Except(numbersInColumn);
        }

        private IEnumerable<int> GetBlock(SudokuGameField field, CellPosition position)
        {
            var topLeftRow = position.Row / BlockHeight;
            var topLeftColumn = position.Column / BlockWidth;
            foreach (var row in Enumerable.Range(topLeftRow, BlockHeight))
                foreach (var column in Enumerable.Range(topLeftColumn, BlockWidth))
                {
                    var value = field.GetElementAt(row, column);
                    if (value != 0)
                        yield return value;
                }
        }
    }
}
