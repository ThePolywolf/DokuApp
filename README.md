# DokuApp

Solves sudokus for you. Enter non-permenant and permenant numbers to the grid.


## Buttons
All buttons and their functions:
- **Solve**: Solves the sudoku as far as it can with its implemented strategies
- **Mark Corners**: Uses sudoku to mark cell corners and possibilities, but doesn't fill any cell values
- **Clear Numbers**: Deletes all non-permenant numbers out of the grid
- **Clear Corners**: Deletes all corner markings
- **Clear Grid**: Combination of *Clear Numbers* and *Clear Corners*
- **Total Clear Grid**: Completely resets the board, deleting all data


## Solution Techniques

Implemented solution strategies:
- Sudoku
- Naked / Hidden Singles
- Pointing Doubles / Triples
- Naked / Hidden Doubles
- Naked / Hidden Triples

When the strategies are not sufficient to solve a sudoku, it will go as far as it can and leave you with the remaining possibilities it is aware of in the empty cells.