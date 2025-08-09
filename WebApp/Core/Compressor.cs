using System.IO.Compression;

namespace WebApp.Core;

public class Compressor(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var response = context.Response;

        string? acceptEncoding = request.Headers.AcceptEncoding;
        if (string.IsNullOrEmpty(acceptEncoding))
        {
            await next(context);
            return;
        }

        var originalBodyStream = response.Body;
        Stream? compressionStream = null;

        if (acceptEncoding.Contains("br", StringComparison.OrdinalIgnoreCase))
        {
            response.Headers.ContentEncoding = "br";
            compressionStream = new BrotliStream(originalBodyStream, CompressionLevel.SmallestSize);
        }
        else if (acceptEncoding.Contains("gzip", StringComparison.OrdinalIgnoreCase))
        {
            response.Headers.ContentEncoding = "gzip";
            compressionStream = new GZipStream(originalBodyStream, CompressionLevel.SmallestSize);
        }
        else if (acceptEncoding.Contains("deflate", StringComparison.OrdinalIgnoreCase))
        {
            response.Headers.ContentEncoding = "deflate";
            compressionStream = new DeflateStream(originalBodyStream, CompressionLevel.SmallestSize);
        }

        if (compressionStream != null)
        {
            response.Body = compressionStream;
            try
            {
                await next(context);
            }
            finally
            {
                await compressionStream.DisposeAsync();
                response.Body = originalBodyStream;
            }
        }
        else
        {
            await next(context);
        }
    }
}