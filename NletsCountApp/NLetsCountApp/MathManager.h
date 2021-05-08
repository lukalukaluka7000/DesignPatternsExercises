#pragma once
#include <vector>
#include <map>
class MathManager
{
public:
	static bool CombContainedInRow(std::vector<int> komb, std::vector<int> trenutniRedak);

	static void FindAtLeastOneCombination(std::vector<std::vector<int>>& insertedCombinations, std::vector<int> redakToCompareTo, std::map<std::vector<int>, int>& currentNlet);

	static void CurrentRowWithCombinationsUpdate(std::map<std::vector<int>, int>& nlet, std::vector<int> redak);

	static std::vector<std::vector<int>> combo(const std::vector<int> c, int k);
};

