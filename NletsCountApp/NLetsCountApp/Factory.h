#pragma once
#include <vector>
class Factory
{
public:
    // Factory Method
    static Factory* makeFactory(int choice);
    virtual std::vector<std::vector<int>> data() = 0;
};