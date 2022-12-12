package levelbounds

import (
	"math"
)

func CalculateExpBounds(levelCap int) []int {
	bounds := make([]int, levelCap-1)

	for i := range bounds {
		bounds[i] = calculateBound(i + 1)
	}

	return bounds
}

func calculateBound(level int) int {
	// Original D&D level up formula
	return 500 * (int(math.Pow(float64(level+1), 2)) - level + 1)
}
