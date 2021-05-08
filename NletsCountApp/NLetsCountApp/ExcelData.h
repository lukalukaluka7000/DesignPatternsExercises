#pragma once
#include"Factory.h"
#include <libxl.h>
#include <string>
class ExcelData : public Factory
{
private:
	libxl::Book* _book = xlCreateBook();
	std::string _fileName;
	std::wstring _filePath;
	
public:
	std::vector<std::vector<int>> data() override;
	void write() override;
	ExcelData(std::string filePath);
};

