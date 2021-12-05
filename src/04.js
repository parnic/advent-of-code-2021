const fs = require('fs');

var numbers = Array();
var boards = Array();

const lines = fs.readFileSync('inputs/04.txt').toString().split('\n');
let phase = 0;
let row = 0;
lines.forEach(line => {
    if (line.length == 0) {
        boards.push(Array());
        phase++;
        row = 0;
    } else if (phase == 0) {
        line.split(',').forEach(entry => {
            numbers.push(parseInt(entry));
        });
    } else {
        boards[phase-1].push(Array());
        line.split(' ').forEach(num => {
            if (num.length > 0) {
                boards[phase-1][row].push(parseInt(num));
            }
        });
        row++;
    }
});

function part1() {
    let marked = Array(boards.length);
    let unmarked = Array(boards.length);
    for (let board in boards) {
        unmarked[board] = boards[board].flat();
    }

    for (let numIdx in numbers) {
        let num = numbers[numIdx];
        for (let boardIdx in boards) {
            let board = boards[boardIdx];
            for (let row in board) {
                for (let col in board[row]) {
                    if (board[row][col] == num) {
                        if (!marked[boardIdx]) {
                            marked[boardIdx] = {row: {0:0,1:0,2:0,3:0,4:0}, col: {0:0,1:0,2:0,3:0,4:0}};
                        }
                        marked[boardIdx].row[row]++;
                        marked[boardIdx].col[col]++;
                        unmarked[boardIdx].splice(unmarked[boardIdx].indexOf(num), 1);

                        if (isWinner(marked[boardIdx])) {
                            let sum = unmarked[boardIdx].reduce((sum, a) => sum + a, 0);
                            console.log(`winner: board ${boardIdx} on turn ${numIdx}, unmarked sum: ${sum}, result: ${sum * num}`);
                            return;
                        }
                    }
                }
            }
        };
    };
}

function isWinner(marked) {
    for (let row in marked.row) {
        if (marked.row[row] == 5) {
            return true;
        }
    }
    for (let col in marked.col) {
        if (marked.col[col] == 5) {
            return true;
        }
    }

    return false;
}

function part2() {
    let marked = Array(boards.length);
    let unmarked = Array(boards.length);
    for (let board in boards) {
        unmarked[board] = boards[board].flat();
    }
    let won = Array();

    for (let numIdx in numbers) {
        let num = numbers[numIdx];
        for (let boardIdx in boards) {
            let board = boards[boardIdx];
            for (let row in board) {
                for (let col in board[row]) {
                    if (board[row][col] == num) {
                        if (!marked[boardIdx]) {
                            marked[boardIdx] = {row: {0:0,1:0,2:0,3:0,4:0}, col: {0:0,1:0,2:0,3:0,4:0}};
                        }
                        marked[boardIdx].row[row]++;
                        marked[boardIdx].col[col]++;
                        unmarked[boardIdx].splice(unmarked[boardIdx].indexOf(num), 1);

                        if (isWinner(marked[boardIdx])) {
                            won[boardIdx] = true;
                            let numWon = 0;
                            for (let wonIdx in won) {
                                if (won[wonIdx]) {
                                    numWon++;
                                }
                            }
                            if (numWon < boards.length) {
                                continue;
                            }

                            let sum = unmarked[boardIdx].reduce((sum, a) => sum + a, 0);
                            console.log(`last winner: board ${boardIdx} on turn ${numIdx}, unmarked sum: ${sum}, result: ${sum * num}`);
                            return;
                        }
                    }
                }
            }
        };
    };
}

part1();
part2();
