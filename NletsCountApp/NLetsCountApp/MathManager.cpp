#include "MathManager.h"
#include <algorithm>

bool MathManager::CombContainedInRow(std::vector<int> komb, std::vector<int> trenutniRedak) {

    for (const int& brojUKombi : komb) {

        if (!std::binary_search(trenutniRedak.begin(), trenutniRedak.end(), brojUKombi)) {
            return false;
        }

    }
    return true;
}

//credits for function: user7781254 stack overflow
std::vector<std::vector<int>> MathManager::combo(const std::vector<int> c, int k)
{
    std::vector<std::vector<int>> nLets;
    int n = c.size();
    int combo = (1 << k) - 1;       // k bit sets
    std::vector<int> toInsert;
    while (combo < 1 << n) {

        int N = c.size();
        for (int i = 0; i < N; ++i) {
            if ((combo >> i) & 1)
                toInsert.push_back(c[i]);
        }
        nLets.push_back(toInsert);

        int x = combo & -combo;
        int y = combo + x;
        int z = (combo & ~y);
        combo = z / x;
        combo >>= 1;
        combo |= y;
        toInsert.clear();
    }
    return nLets;
}

void MathManager::FindAtLeastOneCombination(std::vector<std::vector<int>>& insertedCombinations, std::vector<int> redakToCompareTo, std::map<std::vector<int>, int>& currentNlet)
{
    for (const std::vector<int> komb : insertedCombinations) {
        if (currentNlet.find(komb) != currentNlet.end()) continue;
        if (CombContainedInRow(komb, redakToCompareTo)) {
            if (currentNlet.find(komb) == currentNlet.end()) { //ako ga nema
                currentNlet[komb] = 0;
            }
        }
    }
}
void MathManager::CurrentRowWithCombinationsUpdate(std::map<std::vector<int>, int>& nlet, std::vector<int> redak)
{
    for (std::map<std::vector<int>, int>::iterator it = nlet.begin(); it != nlet.end(); it++) {
        if (CombContainedInRow(it->first, redak)) {
            nlet[it->first] += 1;
        }
    }
}