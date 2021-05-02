#pragma once
#include <string>
#include <map>
class IOManager
{
public:
	int readFile(std::string filePath = ".\\config.txt");
private:
	enum string_code 
	{
		enumRaw,
		enumExcel
	};
	string_code hashit(std::string const& inString);
	std::string getValueFromHashMap(std::string option);
	
	std::map<std::string, std::string> _confData;
	std::string _filePath;
};

