#include "Factory.h"
#include "RawData.h";
#include "ExcelData.h"
Factory* Factory::makeFactory(int choice)
{
    if (choice == 1)
        return new RawData;
    else if (choice == 2)
        return new ExcelData;
}
