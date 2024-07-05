using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace TimberPrint;

public class BlueprintCompressor
{
    public static string CompressToBase64(Blueprint blueprint)
    {
        byte[] blueprintBytes = Encoding.UTF8.GetBytes(blueprint.ConvertToString());
        
        // Compress the original binary data
        using (var compressedStream = new MemoryStream())
        {
            using (var gzipStream = new GZipStream(compressedStream, CompressionLevel.Optimal))
            {
                gzipStream.Write(blueprintBytes, 0, blueprintBytes.Length);
            }

            // Convert the compressed data to Base64
            return Convert.ToBase64String(compressedStream.ToArray());
        }
    }
    
    public static Blueprint DecodeBlueprintString(string blueprintString)
    {
        // Convert Base64 string back to binary data
        byte[] compressedData = Convert.FromBase64String(blueprintString);

        // Decompress the data
        using (var compressedStream = new MemoryStream(compressedData))
        {
            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            {
                using (var decompressedStream = new MemoryStream())
                {
                    gzipStream.CopyTo(decompressedStream);

                    var blueprint = new Blueprint(Encoding.UTF8.GetString(decompressedStream.ToArray()));
                    return blueprint;
                }
            }
        }
    }
}