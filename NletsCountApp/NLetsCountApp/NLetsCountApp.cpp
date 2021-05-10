// NLetsCountApp.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <vector>
#include<ctime>
#include "Factory.h"
#include "IOManager.h"
#include "MathManager.h"

int main()
{
    time_t begin, end;
    time(&begin);
    IOManager* mng = new IOManager();
    Factory* factory = Factory::makeFactory(mng->readFile());
    delete(mng);

    std::vector<std::vector<int>> retci = factory->data();
    
    std::vector<std::vector<int>> combos2, combos3, combos4, combos5;

    for (int i = 0; i < retci.size() ; i++) {

        combos2 = MathManager::combo(retci[i], 2);
        combos3 = MathManager::combo(retci[i], 3);
        combos4 = MathManager::combo(retci[i], 4);
        combos5 = MathManager::combo(retci[i], 5);
        
        for (int j = i + 1; j < retci.size(); j++) {
            MathManager::FindAtLeastOneCombination(combos2, retci[j], factory->duplets);
            MathManager::FindAtLeastOneCombination(combos3, retci[j], factory->triplets);
            MathManager::FindAtLeastOneCombination(combos4, retci[j], factory->quadriples);
            MathManager::FindAtLeastOneCombination(combos5, retci[j], factory->quintets);
        }

        MathManager::CurrentRowWithCombinationsUpdate(factory->duplets, retci[i]);
        MathManager::CurrentRowWithCombinationsUpdate(factory->triplets, retci[i]);
        MathManager::CurrentRowWithCombinationsUpdate(factory->quadriples, retci[i]);
        MathManager::CurrentRowWithCombinationsUpdate(factory->quintets, retci[i]);
        
        if (i % 4 == 0) 
            std::cout << double(i) / double(retci.size()) * 100.0f << "% ..." ;
    }

    factory->write();

    time(&end);
    double diff = difftime(end, begin);
    printf("Vrimena uzelo: %lf minuta\n", diff / 60.0f);
    for (auto elem : retci)
    {
        for (int vecElem : elem) {
            std::cout << vecElem << " ";
        }
        std::cout << std::endl;
    }
    printf("Vrimena uzelo: %lf minuta\n", diff / 60.0f);
    int d;
    std::cin >> d;
}