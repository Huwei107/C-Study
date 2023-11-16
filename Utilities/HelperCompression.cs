using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
namespace FX.MainForms
{
    public class HelperCompression
    {
        /// <summary>
        /// 获取和设置压缩强度。
        /// </summary>
        public CompressionLevel Level;

        public HelperCompression()
        {
            Level = CompressionLevel.DefaultCompression;
        }

        public HelperCompression(CompressionLevel level)
        {
            Level = level;
        }

        /// <summary>
        /// 提供内部使用压缩字流的方法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static byte[] Compression(byte[] data, CompressionMode mode)
        {
            DeflateStream zip = null;
            MemoryStream ms = new MemoryStream();
            try
            {
                if (mode == CompressionMode.Compress)
                {
                    zip = new DeflateStream(ms, mode, true);
                    zip.Write(data, 0, data.Length);
                    zip.Close();
                    return ms.ToArray();
                }
                else
                {
                    ms.Write(data, 0, data.Length);
                    ms.Flush();
                    ms.Position = 0;
                    zip = new DeflateStream(ms, mode, true);
                    MemoryStream os = new MemoryStream();
                    int SIZE = 1024;
                    byte[] buf = new byte[SIZE];
                    int l = 0;
                    do
                    {
                        l = zip.Read(buf, 0, SIZE);
                        if (l == 0) l = zip.Read(buf, 0, SIZE);
                        os.Write(buf, 0, l);
                    } while (l != 0);
                    zip.Close();
                    return os.ToArray();
                }
            }
            catch
            {
                if (zip != null) zip.Close();
                if (ms != null) ms.Close();
                return null;
            }
            finally
            {
                if (zip != null) zip.Close();
                if (ms != null) ms.Close();
            }
        }


        /// <summary>
        /// 从已压缩的字节数组生成原始字节数组。
        /// </summary>
        /// <param name="bytesToDecompress">已压缩的字节数组。</param>
        /// <returns>返回原始字节数组。</returns>
        public byte[] DecompressToBytes(byte[] bytesToDecompress)
        {
            byte[] writeData = new byte[4096];
            Stream s2 = GetInputStream(new MemoryStream(bytesToDecompress));
            MemoryStream outStream = new MemoryStream();
            while (true)
            {
                int size = s2.Read(writeData, 0, writeData.Length);
                if (size > 0)
                {
                    outStream.Write(writeData, 0, size);
                }
                else
                {
                    break;
                }
            }
            s2.Close();
            byte[] outArr = outStream.ToArray();
            outStream.Close();
            return outArr;
        }
        #region Private Methods
        /// <summary>
        /// 根据压缩强度返回使用了不用压缩算法的 Deflate 对象。
        /// </summary>
        /// <param name="level">压缩强度。</param>
        /// <returns>返回使用了不用压缩算法的 Deflate 对象。</returns>
        private Deflater GetDeflater(CompressionLevel level)
        {
            switch (level)
            {
                case CompressionLevel.DefaultCompression:
                    return new Deflater(Deflater.DEFAULT_COMPRESSION);

                case CompressionLevel.BestCompression:
                    return new Deflater(Deflater.BEST_COMPRESSION);

                case CompressionLevel.BestSpeed:
                    return new Deflater(Deflater.BEST_SPEED);

                case CompressionLevel.NoCompression:
                    return new Deflater(Deflater.NO_COMPRESSION);

                default:
                    return new Deflater(Deflater.DEFAULT_COMPRESSION);
            }
        }

        /// <summary>
        /// 从给定的流生成压缩输出流。
        /// </summary>
        /// <param name="inputStream">原始流。</param>
        /// <returns>返回压缩输出流。</returns>
        private DeflaterOutputStream GetOutputStream(Stream inputStream)
        {
            return new DeflaterOutputStream(inputStream, GetDeflater(Level));
        }

        /// <summary>
        /// 从给定的流生成压缩输入流。
        /// </summary>
        /// <param name="inputStream">原始流。</param>
        /// <returns>返回压缩输入流。</returns>
        private InflaterInputStream GetInputStream(Stream inputStream)
        {
            return new InflaterInputStream(inputStream);
        }

        #endregion
    }

    /// <summary>
    /// 压缩强度。
    /// </summary>
    public enum CompressionLevel
    {
        /// <summary>
        /// 采用最好的压缩率。
        /// </summary>
        BestCompression,

        /// <summary>
        /// 采用默认的压缩率。
        /// </summary>
        DefaultCompression,

        /// <summary>
        /// 采用最快的压缩速度。
        /// </summary>
        BestSpeed,

        /// <summary>
        /// 不采用任何压缩。
        /// </summary>
        NoCompression
    }
}
