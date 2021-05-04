// NLetsCountApp.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <map>
#include <vector>
#include <algorithm>
#include "NLetsCountApp.h"
#include "Factory.h"
#include "IOManager.h"
bool CombContainedInRow(std::vector<int> komb, std::vector<int> trenutniRedak) {


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
//credits for function: user7781254 stack overflow
template<typename T> 
std::vector<std::vector<T>> combo(const std::vector<T> c, int k)
{
    std::vector<std::vector<T>> nLets;
    int n = c.size();
    int combo = (1 << k) - 1;       // k bit sets
    std::vector<T> toInsert;
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

void FillNlets(std::vector<std::vector<int>>& insertedCombinations, std::vector<int> redakToCompareTo, std::map<std::vector<int>, int>& currentNlet)
{
    for (const std::vector<int> komb : insertedCombinations) {

        if (CombContainedInRow(komb, redakToCompareTo)) {
            if (currentNlet.find(komb) == currentNlet.end()) {
                currentNlet[komb] = 1;
            }
            else {
                currentNlet[komb] += 1;
            }
        }

    }
}
int main()
{
    IOManager* mng = new IOManager();
    Factory* factory = Factory::makeFactory(mng->readFile());
    delete(mng);

    std::vector<std::vector<int>> retci = factory->data();
    
    std::map<std::vector<int>, int> duplets;
    std::map<std::vector<int>, int> triplets;
    std::map<std::vector<int>, int> quatrets;
    std::map<std::vector<int>, int> quintets;

    for (int i = 0; i < retci.size() ; i++) {
        // vector<vector<int>> vec{ { 1, 2, 3 }, 
                                 // { 4, 5, 6 }, 
                                 // { 7, 8, 9, 4 } }; 
        //std::vector<std::vector<int>> kombe = getDuplets(retci[i]);
        std::vector<std::vector<int>> kombe2 = combo(retci[i], 2);
        std::vector<std::vector<int>> kombe3 = combo(retci[i], 3);
        std::vector<std::vector<int>> kombe4 = combo(retci[i], 4);
        std::vector<std::vector<int>> kombe5 = combo(retci[i], 5);
        
        
        for (int j = i + 1; j < retci.size(); j++) {

            FillNlets(kombe2, retci[j], duplets);
            FillNlets(kombe3, retci[j], triplets);
            FillNlets(kombe4, retci[j], quatrets);
            FillNlets(kombe5, retci[j], quintets);
        }
        
    }
    FinishingTouches(duplets);
    FinishingTouches(triplets);
    FinishingTouches(quatrets);
    FinishingTouches(quintets);

    for (auto elem : retci)
    {
        for (int vecElem : elem) {
            std::cout << vecElem << " ";
        }
        std::cout << std::endl;
    }

    factory->setNlet(duplets);
    factory->setNlet(triplets);
    factory->setNlet(quatrets);
    factory->setNlet(quintets);

    factory->write();

    int d;
    std::cin >> d;
}

void FinishingTouches(std::map<std::vector<int>, int>& nlet)
{
    for (auto vec : nlet) {
        if (vec.second == 1)
            vec.second = 2; 
    }
    int index = 0;
    for (auto it = nlet.cbegin(); it != nlet.cend(); ++it, index++)
    {
        if (it->second == 1)
            nlet[it->first] = 2; // found only one pair, but in both rows, therefore 2 repetirions
    }
}


