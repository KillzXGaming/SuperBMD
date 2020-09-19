using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFormatReader.Common
{
    public static class WriterExtension
    {
        public static void WriteSignature(this EndianBinaryWriter writer, char[] signature)
        {
            if (writer.CurrentEndian == Endian.Little)
                Array.Reverse(signature);

            writer.Write(signature);
        }
    }
}
