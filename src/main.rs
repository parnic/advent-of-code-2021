use std::io::{self, BufRead};
use std::fs::File;
use std::path::Path;

fn main() {
    if let Ok(lines) = read_lines("inputs/01.txt") {
        part1(lines);
    }
    if let Ok(lines) = read_lines("inputs/01.txt") {
        part2(lines);
    }
}

fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
where P: AsRef<Path>, {
    let file = File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}

fn part1(lines: io::Lines<io::BufReader<File>>) {
    let mut last_depth: i64 = 0;
    let mut num_increased = -1;

    for line in lines {
        if let Ok(depth) = line {
            let di: i64 = depth.parse().unwrap();
            if di > last_depth {
                num_increased = num_increased + 1;
            }
            last_depth = di;
        }
    }

    println!("part1: increased: {:?}", num_increased);
}

fn part2(lines: io::Lines<io::BufReader<File>>) {
    let mut last_total: i64 = 0;
    let mut num_increased = -1;
    let mut num1;
    let mut num2 = -1;
    let mut num3 = -1;

    for line in lines {
        if let Ok(depth) = line {
            let di: i64 = depth.parse().unwrap();
            num1 = num2;
            num2 = num3;
            num3 = di;
            if num1 < 0 || num2 < 0 || num3 < 0 {
                continue
            }

            let total = num1 + num2 + num3;
            if total > last_total {
                num_increased = num_increased + 1;
            }
            last_total = total;
        }
    }

    println!("part2: increased: {:?}", num_increased);
}
