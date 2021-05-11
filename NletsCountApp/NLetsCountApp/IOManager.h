#pragma once
#include <string>
#include <map>

class IOManager
{
public:
	std::string readConfig();
private:
	enum string_code 
	{
		Raw,
		Excel
	};
	string_code hashit(std::string const& inString);
	std::string getValueFromHashMap(std::string option);
	
	std::map<std::string, std::string> _confData;
	std::string _filePath;
};

