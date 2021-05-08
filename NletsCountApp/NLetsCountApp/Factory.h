#pragma once
#include <vector>
#include <map>
class Factory
{
public:
    std::map<std::vector<int>, int> duplets;
    std::map<std::vector<int>, int> triplets;
    std::map<std::vector<int>, int> quadriples;
    std::map<std::vector<int>, int> quintets;
public:
    // Factory Method
    static Factory* makeFactory(int choice);
    virtual std::vector<std::vector<int>> data() = 0;
    virtual void write() = 0;
};