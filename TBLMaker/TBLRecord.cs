namespace TBLMaker
{
    /// <summary>
    ///     Represents one record in *.tbl file.
    /// </summary>
    public class TBLRecord
    {
        /// <summary>
        ///     *.bin file name
        /// </summary>
        public string FileName;

        /// <summary>
        ///     Number of record in *.tbl file.
        /// </summary>
        public int FileNumber;

        /// <summary>
        ///     *.bin file size. Should be 6 byte hex value writed in 2-byte reversed order.
        ///     E.g. file size 000F3C should be writed as 3C0F00.
        /// </summary>
        public string FileSize;

        /// <summary>
        ///     Name of parameter controlled by *.bin.
        /// </summary>
        public string ParameterName;

        /// <summary>
        /// Full *.bin file path.
        /// </summary>
        public string FilePath;

        public TBLRecord()
        {
            FileNumber = 0;
            ParameterName = string.Empty;
            FileName = string.Empty;
            FileSize = string.Empty;
        }

        /// <summary>
        ///     *.bin file record representation
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