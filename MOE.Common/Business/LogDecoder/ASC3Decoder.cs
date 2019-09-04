using System;
using System.Collections.Concurrent;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using MOE.Common.Properties;

namespace MOE.Common.Business.LogDecoder
{
    public class Asc3Decoder
    {
        // reference https://en.wikipedia.org/wiki/List_of_file_signatures
        // Found the two bytes for EOS by reading the file.
        public static byte[] _ZlibHeaderNoCompression = { 0x78, 0x01 };
        public static byte[] _ZlibHeaderDefaultCompression = { 0x78, 0x9C };
        public static byte[] _ZlibHeaderBestCompression = { 0x78, 0xDA };
        public static byte[] _GZipHeader = { 0x1f, 0x8b };
        public static byte[] _EOSHeader = { 0x18, 0x95 };

        public static void DecodeAsc3File(string fileName, string signalId,
            BlockingCollection<Data.MOE.Controller_Event_LogRow> rowBag, DateTime earliestAcceptableDate)
        {
            var encoding = Encoding.ASCII;

            // load the file into memory stream
            var fileStream = File.Open(fileName, FileMode.Open);
            var memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);
            fileStream.Close();

            // set the memory position to beggining
            memoryStream.Position = 0;

            // first let's check to see if the data is compressed
            if (IsCompressed(memoryStream))
            {
                // ok it is compressed, let's decompress it before we procced
                memoryStream = DecompessedStream(memoryStream);
            }

            using (var br = new BinaryReader(memoryStream, encoding))
            {
                var elTable = new Data.MOE.Controller_Event_LogDataTable();
                //var custUnique =
                //    new UniqueConstraint(new[]
                //    {
                //        elTable.Columns["SignalID"],
                //        elTable.Columns["Timestamp"],
                //        elTable.Columns["EventCode"],
                //        elTable.Columns["EventParam"]
                //    });

                //elTable.Constraints.Add(custUnique);

                if (br.BaseStream.Position + 21 < br.BaseStream.Length)
                {
                    //Find the start Date
                    var dateString = "";
                    for (var i = 1; i < 21; i++)
                    {
                        var c = br.ReadChar();
                        dateString += c;
                    }

                    var startTime = new DateTime();
                    if (DateTime.TryParse(dateString, out startTime) && br.BaseStream.Position < br.BaseStream.Length)
                    {
                        //find  line feed characters, that should take us to the end of the header.
                        // First line break is after Version
                        // Second LF is after FileName
                        // Third LF is after Interseciton number, which isn't used as far as I can tell
                        // Fourth LF is after IP address
                        // Fifth is after MAC Address
                        // Sixth is after "Controller data log beginning:," and then the date
                        // Seven is after "Phases in use:," and then the list of phases, seperated by commas

                        var i = 0;

                        while (i < 7 && br.BaseStream.Position < br.BaseStream.Length)
                        {
                            var c = br.ReadChar();
                            if (c == '\n')
                                i++;
                        }

                        // after that, we start reading until we reach the end 
                        while (br.BaseStream.Position + sizeof(byte) * 4 <= br.BaseStream.Length)
                        {
                            var eventTime = new DateTime();
                            var eventCode = new int();
                            var eventParam = new int();

                            for (var eventPart = 1; eventPart < 4; eventPart++)
                            {
                                //getting the EventCode
                                if (eventPart == 1)
                                    eventCode = Convert.ToInt32(br.ReadByte());

                                if (eventPart == 2)
                                    eventParam = Convert.ToInt32(br.ReadByte());

                                //getting the time offset
                                if (eventPart == 3)
                                {
                                    var rawoffset = new byte[2];
                                    rawoffset = br.ReadBytes(2);
                                    Array.Reverse(rawoffset);
                                    int offset = BitConverter.ToInt16(rawoffset, 0);
                                    var tenths = Convert.ToDouble(offset) / 10;
                                    eventTime = startTime.AddSeconds(tenths);
                                }
                            }

                            if (eventTime <= DateTime.Now && eventTime > earliestAcceptableDate)
                            {
                                try
                                {
                                    var eventrow = elTable.NewController_Event_LogRow();
                                    eventrow.Timestamp = eventTime;
                                    eventrow.SignalID = signalId;
                                    eventrow.EventCode = eventCode;
                                    eventrow.EventParam = eventParam;
                                    rowBag.Add(eventrow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                    //this is what we do when the datestring doesn't parse
                }
                //this is what we do when the datestring doesn't parse
            }
        }

        /// <summary>
        /// Determines if filestream passed in contains a hex signature of one of the known compression algorithms
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns>true or false if compression is detected</returns>
        public static bool IsCompressed(MemoryStream fileStream)
        {
            // read the magic header
            byte[] header = new byte[2];
            fileStream.Read(header, 0, 2);

            // let seek to back of file
            fileStream.Seek(0, SeekOrigin.Begin);

            // let's check for zlib compression
            if (AreEqual(header, _ZlibHeaderNoCompression) || AreEqual(header, _ZlibHeaderDefaultCompression) || AreEqual(header, _ZlibHeaderBestCompression))
            {
                return true;
            }
            // let's make sure it is not another compression 
            else if (AreEqual(header, _GZipHeader) || AreEqual(header, _EOSHeader))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Decompresses the passed in filestream using deflatestream
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns>decompressed filestream</returns>
        public static MemoryStream DecompessedStream(MemoryStream fileStream)
        {
            // read past the first two bytes of the zlib header
            fileStream.Seek(2, SeekOrigin.Begin);

            // decompress the file
            using (DeflateStream deflateStream = new DeflateStream(fileStream, CompressionMode.Decompress))
            {
                // copy decompressed data into return stream
                MemoryStream returnStream = new MemoryStream();
                deflateStream.CopyTo(returnStream);
                returnStream.Position = 0;

                return returnStream;
            }
        }


        /// <summary>
        /// Compares if two byte arrays are equal
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="b1"></param>
        /// <returns>returns true if they are equal and false if they are not equal</returns>
        private static bool AreEqual(byte[] a1, byte[] b1)
        {
            if (a1.Length != b1.Length)
            {
                return false;
            }

            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != b1[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}