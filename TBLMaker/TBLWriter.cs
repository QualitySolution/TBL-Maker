using System;
using System.IO;

namespace TBLMaker
{
    /// <summary>
    ///     Class for writing *.tbl file to disk.
    /// </summary>
    internal static class TBLWriter
    {
        /// <summary>
        ///     Writes file to disk.
        /// </summary>
        /// <param name="file">TBLFile to write to disk. Uses TBLFile.Path as destination.</param>
        public static void Write(TBLFile file)
        {
            for (var i = 0; i < file.ParsedTBLRecords.Count; i++)
            {
                //Writing file name
                WriteValue(
                    ref file.FileBytes,
                    file.ParsedTBLRecords[i].FileName,
                    TBLParser.FirstFileName + TBLParser.SectionSize*i,
                    TBLParser.FirstFileParameter + TBLParser.SectionSize*i);
                //Writing parameter name
                WriteValue(
                    ref file.FileBytes,
                    file.ParsedTBLRecords[i].ParameterName,
                    TBLParser.FirstFileParameter + TBLParser.SectionSize*i,
                    TBLParser.FirstFileNumber + TBLParser.SectionSize*i);
                //Writing file size by one byte.
                var first = Convert.ToInt32(file.ParsedTBLRecords[i].FileSize.Substring(0, 2), 16);
                var second = Convert.ToInt32(file.ParsedTBLRecords[i].FileSize.Substring(2, 2), 16);
                var third = Convert.ToInt32(file.ParsedTBLRecords[i].FileSize.Substring(4, 2), 16);
                file.FileBytes[TBLParser.FirstFileSize + TBLParser.SectionSize*i] = Convert.ToByte(first);
                file.FileBytes[TBLParser.FirstFileSize + TBLParser.SectionSize*i + 1] = Convert.ToByte(second);
                file.FileBytes[TBLParser.FirstFileSize + TBLParser.SectionSize*i + 2] = Convert.ToByte(third);
            }

            File.WriteAllBytes(file.Path, file.FileBytes);
        }

        /// <summary>
        ///     Writes specified value to byte[] array to specified position. All non-value will be zero-filled.
        /// </summary>
        /// <param name="array">Destination byte[] array</param>
        /// <param name="value">Value to write</param>
        /// <param name="startIndex">Start array index</param>
        /// <param name="endIndex">End array index. Writer will NOT include this byte.</param>
        private static void WriteValue(ref byte[] array, string value, int startIndex, int endIndex)
        {
            var i = startIndex;
            for (var j = 0; j < value.Length; j++)
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