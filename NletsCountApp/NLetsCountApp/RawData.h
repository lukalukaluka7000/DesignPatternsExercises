#pragma once
#include"Factory.h";

class RawData : public Factory
{
public:
	std::vector<std::vector<int>> data() override;
	void write() override;
};

