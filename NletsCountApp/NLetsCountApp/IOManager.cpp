#include "IOManager.h"
#include <fstream>
#include <string>
#include <iostream>
#include <sstream>
#include <cassert>
#define ConfigName "config.txt"
#define ConfigFetchMode "FetchDataMode"
#define ConfigFileName "FileName"
std::string IOManager::readConfig()
{
	std::string _filePath = ConfigName;
	std::ifstream file;
	file.open(_filePath);
	if (!file.is_open()) {
		std::cout << "Ne mogu pronaci config datoteku: " << _filePath <<  std::endl;
		exit(-1);
	}
	std::string line = "";
	while (std::getline(file, line))
	{
		std::size_t comment = line.find('#', 0);
		if (comment == std::string::npos)
		{
			//Removes Spaces
			std::string::size_type space = line.find(' ');
			while (std::string::npos != space)
			{
				line.erase(space, 1);
				space = line.find(' ');
			}

			std::istringstream is_line(line);
			std::string key;
			if (std::getline(is_line, key, '='))
			{
				std::string value;
				if (std::getline(is_line, value))
				{
					_confData.emplace(key, value);
				}

			}
		}
	}

	//refacgtor this if ther would be more shit in config for now this is acceptable for me
	std::string mode =	getValueFromHashMap(ConfigFetchMode);
	if (hashit(mode) == Raw) return "error";
	else 
		return getValueFromHashMap(ConfigFileName);
}

IOManager::string_code IOManager::hashit(std::string const& inString) {
	if (inString == "raw" || inString == "Raw" ||inString == "RAW") 
		return Raw;
	if (inString == "excel" || inString == "Excel" || inString == "EXCEL") 
		return Excel;
}
std::string IOManager::getValueFromHashMap(std::string option)
{
	std::map<std::string, std::string>::iterator it = _confData.find(option);
	if (it == _confData.end()) {
		std::cout << "Key: " + option + " not found in " + _filePath << std::endl;
		return "";
	}
	return it->second;
}
