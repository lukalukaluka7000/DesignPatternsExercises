#pragma once
#include"Factory.h"
class ExcelData : public Factory
{
public:
	std::vector<std::vector<int>> data() override;
	void write() override;
};

