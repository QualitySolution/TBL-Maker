using System;

namespace TBLMaker
{
    public class TBLRecord
    {
        public int FileNumber;
        public string ParameterName;
        public string FileName;
        public string FileSize;

        public TBLRecord()
        {
            FileNumber = 0;
            ParameterName = String.Empty;
            FileName = String.Empty;
            FileSize = String.Empty;
        }
        
        /// <summary>
        /// *.bin file record representation
        /// </summary>
        /// <param name="fileNumber">Number of record in *.tbl file</param>
        /// <param name="parameterName">File parameter name</param>
        /// <param name="fileName">*.bin file name</param>
        /// <param name="fileSize">*.bin file size</param>
        public TBLRecord(int fileNumber, string parameterName, string fileName, string fileSize)
        {
            FileNumber = fileNumber;
            ParameterName = parameterName;
            FileName = fileName;
            FileSize = fileSize;
        }
    }
}
