// NLetsCountApp.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <map>
#include <vector>
#include <algorithm>
bool CombContainedInRow(std::vector<int> komb, std::vector<int> trenutniRedak) {

    std::vector<int> v = { 1, 2, 3, 4, 5, 6, 7 };
    int key = 6;

    for(const int& brojUKombi : komb) {

        if (!std::binary_search(trenutniRedak.begin(), trenutniRedak.end(), brojUKombi)) {
            return false;
        }

    }
    return true;
}

// Returns value of Binomial Coefficient C(n, k)
int binomialCoeff(int n, int k)
{
    // Base Cases
    if (k > n)
        return 0;
    if (k == 0 || k == n)
        return 1;

    // Recur
    return binomialCoeff(n - 1, k - 1)
        + binomialCoeff(n - 1, k);
}
std::vector<std::vector<int>> getDuplets(std::vector<int> redak) {

    std::vector<std::vector<int>> duplets;
    int dupletCount = binomialCoeff(redak.size(), 2);
    std::vector<int> toInsert(2,0);
    for (int i = 0; i < dupletCount; i++) {
        for (int j = i + 1; j < redak.size(); j++) {
            toInsert[0] = redak[i]; toInsert[1] = redak[j];
            duplets.push_back(toInsert);
        }
    }
    return duplets;
}
int main()
{
    //vector<int> data_1{ 10, 20, 30, 40 };
    //vis[data_1] = 1;
    std::vector<std::vector<int>> retci{ {1,2,5,7,8,9},{2,6,7,8,10,13}, {2,7,50,51,52,53} };

    std::map<std::vector<int>, int> rjecnik;

    for (int i = 0; i < retci.size(); i++) {
        // vector<vector<int>> vec{ { 1, 2, 3 }, 
                                 // { 4, 5, 6 }, 
                                 // { 7, 8, 9, 4 } }; 
        std::vector<std::vector<int>> kombe = getDuplets(retci[i]);

        for(const std::vector<int> komb : kombe) {

            for (int j = i + 1; j < retci.size(); j++) {

                if (CombContainedInRow(komb, retci[j])) {
                    if (rjecnik.find(komb) == rjecnik.end()) {
                        rjecnik[komb] = 2; // found for tha first time, but in both rows, therefore 2 repetirions
                    }
                    else {
                        rjecnik[komb] += 1;
                    }
                }

            }

        }
    }
}
