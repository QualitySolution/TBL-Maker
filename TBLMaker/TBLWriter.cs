﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TBLMaker
{
    internal static class TBLWriter
    {
        public static void Write(TBLFile file)
        {
            for (int i = 0; i < file.ParsedTBLRecords.Count; i++)
            {
                WriteValue(
                    ref file.FileBytes, 
                    file.ParsedTBLRecords[i].FileName, 
                    TBLParser.FirstFileName + TBLParser.SectionSize * i, 
                    TBLParser.FirstFileParameter + TBLParser.SectionSize * i);
                WriteValue(
                    ref file.FileBytes,
                    file.ParsedTBLRecords[i].ParameterName,
                    TBLParser.FirstFileParameter + TBLParser.SectionSize * i,
                    TBLParser.FirstFileNumber + TBLParser.SectionSize * i);
                var first = Convert.ToInt32(file.ParsedTBLRecords[i].FileSize.Substring(0, 2), 16);
                var second = Convert.ToInt32(file.ParsedTBLRecords[i].FileSize.Substring(2, 2), 16);
                var third = Convert.ToInt32(file.ParsedTBLRecords[i].FileSize.Substring(4, 2), 16);
                file.FileBytes[TBLParser.FirstFileSize + TBLParser.SectionSize*i] = Convert.ToByte(first);
                file.FileBytes[TBLParser.FirstFileSize + TBLParser.SectionSize*i + 1] = Convert.ToByte(second);
                file.FileBytes[TBLParser.FirstFileSize + TBLParser.SectionSize*i + 2] = Convert.ToByte(third);
            }

            File.WriteAllBytes(file.Path, file.FileBytes);
        }

        private static void WriteValue(ref byte[] array, string value, int startIndex, int endIndex)
        {
            int i = startIndex;
            for (int j = 0; j < value.Length; j++)
            {
                array[i + j] = Convert.ToByte(value[j]);
            }
            for (i = startIndex + value.Length; i < endIndex; i++)
            {
                array[i] = 0;
            }
        }
    }
}