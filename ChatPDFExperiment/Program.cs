using ChatPDFExperiment.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
// Ver la ApiKey como usuario en https://api.chatpdf.com
const string apiKey = "sec_xxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
const string apiUrl = "https://api.chatpdf.com/v1/sources/add-url";
const string apiMessage = "https://api.chatpdf.com/v1/chats/message";

var app = builder.Build();


app.MapPost("obtenerPDFKey", async context =>
{
    
    var requestBody = new
    {
        url = "https://buenosaires.gob.ar/sites/default/files/2023-06/Reglamento%20Free%20Fire.pdf" //PDF url
    };

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await client.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            await context.Response.WriteAsync("API response: " + responseContent);
        }
        else
        {
            await context.Response.WriteAsync("API request failed with status code: " + response.StatusCode);
        }
    }
});

app.MapPost("mensajePDF", async context =>
{

    using (var reader = new StreamReader(context.Request.Body))
    {
        var requestBodyJson = await reader.ReadToEndAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        //Si necesitas hacer algo con el body recibido. Descomentarlo
        //var requestBody = JsonSerializer.Deserialize<ChatMessageRequest>(requestBodyJson, options);


        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);

            var content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiMessage, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                await context.Response.WriteAsync("API response: " + responseContent);
            }
            else
            {
                await context.Response.WriteAsync("API request failed with status code: " + response.StatusCode);
            }
        }
    }

});

app.Run();
