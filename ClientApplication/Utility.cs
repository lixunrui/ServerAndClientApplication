using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace ClientApplication
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct HeaderMsg
    {
        internal int messageID;
        internal string messageFrom;
        internal string messageTO;
        internal int messageSize;
    }

    internal static class Utility
    {
        internal static byte[] GetBytesFromStruct(HeaderMsg header)
        {
            //byte[] arr = null;

            //try
            //{
            //    int size = Marshal.SizeOf(header);
            //    arr = new byte[size];
            //    IntPtr ptr = Marshal.AllocHGlobal(size);
            //    Marshal.StructureToPtr(header, ptr, true);
            //    Marshal.Copy(ptr, arr, 0, size);
            //    Marshal.FreeHGlobal(ptr);
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //return arr;

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            writer.Write(header.messageID);
            writer.Write(header.messageFrom);
            writer.Write(header.messageTO);
            writer.Write(header.messageSize);

            return stream.ToArray();
        }

        internal static HeaderMsg GetStructFromBytes(byte[] data)
        {
            //HeaderMsg header = new HeaderMsg();
            //int size = Marshal.SizeOf(header);
            //IntPtr ptr = Marshal.AllocHGlobal(size);
            //Marshal.Copy(data, 0, ptr, size);
            //header = (HeaderMsg)Marshal.PtrToStructure(ptr, data.GetType());
            //Marshal.FreeHGlobal(ptr);
            //return header;

            var reader = new BinaryReader(new MemoryStream(data));

            HeaderMsg header = new HeaderMsg();

            header.messageID = reader.ReadInt32();
            header.messageFrom = reader.ReadString();
            header.messageTO = reader.ReadString();
            header.messageSize = reader.ReadInt32();

            return header;
        }
    }
}
