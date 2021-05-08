#include "Factory.h"
#include "RawData.h";
#include "ExcelData.h"
Factory* Factory::makeFactory(int choice)
{
    if (choice == 1)
        return new RawData;
    else if (choice == 2)
        return new ExcelData("testExcelv0.xls");
}

void Factory::setNlet(std::map<std::vector<int>, int> Nlet)
{
    if (!Nlet.empty()) {
        switch (Nlet.begin()->first.size()) {
            case 2:
                _duplets = Nlet;
                break;
            case 3:
                _triplets = Nlet;
                break;
            case 4:
                _quadriples = Nlet;
                break;
            case 5:
                _quantiples = Nlet;
                break;
            }
    }
    
}
