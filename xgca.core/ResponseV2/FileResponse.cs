using System.Collections.Generic;

namespace otm.core.ResponseV2
{
    public class FileResponse
    {
        public byte[] Bytes { get; }
        public string FileName { get; }
        public List<ErrorField> Errors { get; }

        public FileResponse(byte[] bytes, string fileName)
        {
            Bytes = bytes;
            FileName = fileName;
        }

        public FileResponse(List<ErrorField> errors)
        {
            Errors = errors;
        }
    }
}