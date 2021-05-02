#include "RawData.h"

std::vector<std::vector<int>> RawData::data()
{
	//vector<int> data_1{ 10, 20, 30, 40 };
	//vis[data_1] = 1;
	//std::vector<std::vector<int>> retci{ {1,2,5,7,8,9},{2,6,7,8,10,13}, {2,7,50,51,52,53} };
	//std::vector<std::vector<int>> retci{ {1,2,5,7,8,9},{1,2,5,8,10,13}, {1,2,3,4,5,53} };
	std::vector<std::vector<int>> simulateExcel{ {1,2,5,7,8,9},{1,2,5,8,10,13}, {1,2,3,4,5,53} };
	return simulateExcel;
}
