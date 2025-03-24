using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Middleware
{
    public class JsonRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public JsonRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Vérifier si c'est une requête JSON
            if (context.Request.ContentType != null && 
                context.Request.ContentType.Contains("application/json") && 
                context.Request.Method != "GET" &&
                context.Request.ContentLength.HasValue)
            {
                // Sauvegarder le corps de la requête
                context.Request.EnableBuffering();
                
                // Lire le corps de la requête de manière sécurisée
                using var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true);
                
                var requestBody = await reader.ReadToEndAsync();
                
                // Corriger les nombres avec des zéros en tête
                var correctedBody = FixLeadingZeros(requestBody);

                // Remplacer le corps de la requête
                var correctedBytes = Encoding.UTF8.GetBytes(correctedBody);
                context.Request.Body = new MemoryStream(correctedBytes);
                context.Request.ContentLength = correctedBytes.Length;
                context.Request.Body.Position = 0;
            }

            await _next(context);
        }

        private string FixLeadingZeros(string json)
        {
            // Regex pour trouver les nombres avec des zéros en tête dans les valeurs JSON
            var pattern = "\"(\\w+)\"\\s*:\\s*0+(\\d+)";
            return Regex.Replace(json, pattern, "\"$1\": $2");
        }
    }
}

