#include "ExcelData.h"
#include "libxl.h";
#include<iostream>

#define FirstRowData 13
#define FirstColData 6
#define LastColData 12
std::vector<std::vector<int>> ExcelData::data()
{
    std::vector<std::vector<int>> excelData;
	libxl::Book* book = xlCreateBook();

	if (book->load(L"Debug\\testExcel.xls")) {
		libxl::Sheet* sheet = book->getSheet(1);
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
                            std::wcout << "(" << row << ", " << col << ") = ";
                            int d = sheet->readNum(row, col);
                            currentRow.push_back(d);
                            std::cout << d << " [number]" << std::endl;
                            break;
                        }
                        case libxl::CELLTYPE_STRING: break;
                        case libxl::CELLTYPE_BOOLEAN: break;
                        case libxl::CELLTYPE_BLANK:  break;
                        case libxl::CELLTYPE_ERROR:  break;
                    }
				}
                excelData.push_back(currentRow);
			}

		}
	}
    return excelData;
}
