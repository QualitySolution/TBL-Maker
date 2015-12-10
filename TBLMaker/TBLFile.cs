using System.Collections.Generic;

namespace TBLMaker
{
    /// <summary>
    /// Represents *.tbl file.
    /// </summary>
    class TBLFile
    {
        public string Path;
        public byte[] FileBytes;
        public List<TBLRecord> ParsedTBLRecords;

        /// <summary>
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="fileBytes">Readed bytes.</param>
        /// <param name="parsedRecords">Parsed file *.tbl records</param>
        public TBLFile(string path, byte[] fileBytes, List<TBLRecord> parsedRecords)
        {
            Path = path;
            FileBytes = fileBytes;
            ParsedTBLRecords = parsedRecords;
        }
    }
}
