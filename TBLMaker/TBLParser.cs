using System;
using System.Collections.Generic;
using System.IO;

namespace TBLMaker
{
    internal static class TBLParser
    {
        internal const int Start = 310;
        internal const int FirstFileName = 310;
        internal const int FirstFileParameter = 360;
        internal const int FirstFileNumber = 400;
        internal const int FirstFileSize = 402;
        internal const int SectionSize = 96;


        public static TBLFile Parse(string filePath)
        {
            List<TBLRecord> recordsList = new List<TBLRecord>();

            byte[] fileBytes = File.ReadAllBytes(filePath);

            int i = 0;
            while ((Start + i * SectionSize) < fileBytes.Length)
            {
                var record = new TBLRecord();

                //Reading file name
                int j = FirstFileName + i * SectionSize;
                while (fileBytes[j] != 0) {
                    record.FileName += Convert.ToChar(fileBytes[j++]);
                }
                
                //Reading parameter name
                j = FirstFileParameter + i * SectionSize;
                while (fileBytes[j] != 0)
                {
                    record.ParameterName += Convert.ToChar(fileBytes[j++]);
                }

                //Reading file number. If it is first file - it's number is 0 and we cannot find a place for it;
                record.FileNumber = fileBytes[FirstFileNumber + i * SectionSize];
                
                //Reading file size;
                for (j = FirstFileSize + i*SectionSize; j < FirstFileSize + i*SectionSize + 3; j++)
                {
                    record.FileSize += Convert.ToString(fileBytes[j], 16).PadLeft(2, '0');
                }
                recordsList.Add(record);
                i++;
            }
            return new TBLFile(filePath, fileBytes, recordsList);
        }
    }
}
