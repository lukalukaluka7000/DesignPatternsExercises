#include "ExcelData.h"
#include "libxl.h";
#include<iostream>
#include <string>
#include<fstream>

#define FirstRowData 13
#define FirstColData 6
#define LastColData 12

#define WriteStartRow 2
#define WriteStartCol 2

ExcelData::ExcelData(std::string fileName)
{
    _fileName = fileName;
    std::string fp = "Debug\\" + _fileName;
    _filePath = std::wstring(fp.begin(), fp.end());
}

std::vector<std::vector<int>> ExcelData::data()
{
    std::ifstream infile(_filePath);
    if (!infile.good()) {
        std::cout << "Ne mogu pronaci excel za otvoriti..." << std::endl;
        exit(-1);
    }

    std::vector<std::vector<int>> excelData;
    
	if (_book->load(_filePath.c_str())) {
        
		libxl::Sheet* sheet = _book->getSheet(1);

		if (sheet) {

			for (int row = FirstRowData; row < sheet->lastRow(); ++row) {
                std::vector<int> currentRow;
				for (int col = FirstColData; col < LastColData; ++col)
				{
                    libxl::CellType cellType = sheet->cellType(row, col);
                    switch (cellType)
                    {
                        case libxl::CELLTYPE_EMPTY: break;
                        case libxl::CELLTYPE_NUMBER:
                        {
                            //std::cout << "(" << row << ", " << col << ") = ";
                            double d = sheet->readNum(row, col);
                            currentRow.push_back(int(d));
                            //std::cout << d << " [number]" << std::endl;
                            break;
                        }
                        case libxl::CELLTYPE_STRING: break;
                        case libxl::CELLTYPE_BOOLEAN: break;
                        case libxl::CELLTYPE_BLANK:  break;
                        case libxl::CELLTYPE_ERROR:  break;
                    }
				}
                if (currentRow.empty())
                    break;
                excelData.push_back(currentRow);
			}

		}
	}

    return excelData;
}

template <typename A, typename B>
std::multimap<B, A> flip_map(std::map<A, B>& src) {

    std::multimap<B, A> dst;

    for (typename std::map<A, B>::const_iterator it = src.begin(); it != src.end(); ++it)
        dst.insert(std::pair<B, A>(it->second, it->first));

    return dst;
}
void ExcelData::write() {
    int currentRow = WriteStartRow;
    int currentCol = WriteStartCol;

    if (_book->load(_filePath.c_str())) {
        
        libxl::Sheet* sheet = _book->getSheet(2); // duplets
        if (sheet) {

            std::multimap<int, std::vector<int>> reverseTest = flip_map(duplets);
            /*Contents of flipped map in descending order:*/
            for (std::multimap<int, std::vector<int>>::const_reverse_iterator elem = reverseTest.rbegin(); elem != reverseTest.rend(); ++elem)
            {
                currentCol = WriteStartCol;
                for (int vecElem : elem->second) {
                    sheet->writeNum(currentRow, currentCol++, vecElem);
                }
                sheet->writeNum(currentRow, currentCol, elem->first);
                currentRow++;
            }
            currentRow = WriteStartRow;
        }

        sheet = _book->getSheet(3); // triplets
        if (sheet) {

            std::multimap<int, std::vector<int>> reverseTest = flip_map(triplets);
            for (std::multimap<int, std::vector<int>>::const_reverse_iterator elem = reverseTest.rbegin(); elem != reverseTest.rend(); ++elem)
            {
                currentCol = WriteStartCol;
                for (int vecElem : elem->second) {
                    sheet->writeNum(currentRow, currentCol++, vecElem);
                }
                sheet->writeNum(currentRow, currentCol, elem->first);
                currentRow++;
            }
            currentRow = WriteStartRow;
        }

        sheet = _book->getSheet(4); // quatro
        if (sheet) {

            std::multimap<int, std::vector<int>> reverseTest = flip_map(quadriples);
            for (std::multimap<int, std::vector<int>>::const_reverse_iterator elem = reverseTest.rbegin(); elem != reverseTest.rend(); ++elem)
            {
                currentCol = WriteStartCol;
                for (int vecElem : elem->second) {
                    sheet->writeNum(currentRow, currentCol++, vecElem);
                }
                sheet->writeNum(currentRow, currentCol, elem->first);
                currentRow++;
            }
            currentRow = WriteStartRow;
        }

        sheet = _book->getSheet(5); // quinto
        if (sheet) {

            std::multimap<int, std::vector<int>> reverseTest = flip_map(quintets);
            for (std::multimap<int, std::vector<int>>::const_reverse_iterator elem = reverseTest.rbegin(); elem != reverseTest.rend(); ++elem)
            {
                currentCol = WriteStartCol;
                for (int vecElem : elem->second) {
                    sheet->writeNum(currentRow, currentCol++, vecElem);
                }
                sheet->writeNum(currentRow, currentCol, elem->first);
                currentRow++;
            }
            currentRow = WriteStartRow;
        }
    }
    _book->save(_filePath.c_str());
    _book->release();

    std::cout << std::endl;
    for (auto elem : duplets)
    {
        for (int vecElem : elem.first) {
            std::cout << vecElem << " ";
        }
        std::cout << "    ----> " << elem.second << " puta";
        std::cout << std::endl;
    }
    std::cout << std::endl;
    for (auto elem : triplets)
    {
        for (int vecElem : elem.first) {
            std::cout << vecElem << " ";
        }
        std::cout << "    ----> " << elem.second << " puta";
        std::cout << std::endl;
    }
    std::cout << std::endl;
    for (auto elem : quadriples)
    {
        for (int vecElem : elem.first) {
            std::cout << vecElem << " ";
        }
        std::cout << "    ----> " << elem.second << " puta";
        std::cout << std::endl;
    }
    std::cout << std::endl;
    for (auto elem : quintets)
    {
        for (int vecElem : elem.first) {
            std::cout << vecElem << " ";
        }
        std::cout << "    ----> " << elem.second << " puta";
        std::cout << std::endl;
    }
}