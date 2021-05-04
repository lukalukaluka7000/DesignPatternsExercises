#pragma once
#include <vector>
#include <map>
class Factory
{
protected:
    std::map<std::vector<int>, int> _duplets;
    std::map<std::vector<int>, int> _triplets;
    std::map<std::vector<int>, int> _quadriples;
    std::map<std::vector<int>, int> _quantiples;
public:
    // Factory Method
    static Factory* makeFactory(int choice);
    virtual std::vector<std::vector<int>> data() = 0;
    virtual void write() = 0;
    void setNlet(std::map<std::vector<int>, int> Nlet);
};