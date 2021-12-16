package src

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

func Day01() {
	fileBytes, fileErr := ioutil.ReadFile("inputs/01.txt")
	if fileErr != nil {
		panic(fileErr)
	}

	fileStr := string(fileBytes)
	fileLines := strings.Split(strings.ReplaceAll(fileStr, "\r", ""), "\n")
	fileInts := make([]int, len(fileLines))
	var err error
	for idx, line := range fileLines {
		fileInts[idx], err = strconv.Atoi(line)
		if err != nil {
			panic(err)
		}
	}
	part1(fileInts)
	part2(fileInts)
}

func part1(depths []int) {
	lastDepth := 0
	numIncreased := -1

	for _, depth := range depths {
		if depth > lastDepth {
			numIncreased++
		}

		lastDepth = depth
	}

	fmt.Println("<+black>> part1: increased:", numIncreased)
}

func part2(depths []int) {
	lastTotal := 0
	numIncreased := -1

	num1 := -1
	num2 := -1
	num3 := -1

	for _, depth := range depths {
		num1 = num2
		num2 = num3
		num3 = depth
		if num1 < 0 || num2 < 0 || num3 < 0 {
			continue
		}

		total := num1 + num2 + num3
		if total > lastTotal {
			numIncreased++
		}
		lastTotal = total
	}

	fmt.Println("<+black>> part2: increased:", numIncreased)
}
