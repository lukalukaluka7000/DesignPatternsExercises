#pragma once
#include"Factory.h"
#include <libxl.h>
class ExcelData : public Factory
{
private:
	libxl::Book* _book = xlCreateBook();
public:
	std::vector<std::vector<int>> data() override;
	void write() override;
};

