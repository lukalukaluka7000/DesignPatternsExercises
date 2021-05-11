#include "Factory.h"
#include "RawData.h";
#include "ExcelData.h"
Factory* Factory::makeFactory(std::string fileName)
{
    if (fileName.find("error") != std::string::npos || fileName.empty())      
        return new RawData;
    else
        return new ExcelData(fileName);
}
