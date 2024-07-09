using angularwithasp.server.Data;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
namespace angularwithasp.server.Services
{
    public enum LogsEnum
    {
        StartTime = 0,
        PolygonResponse = 1,
        EmailsCount = 2
    }

    public class StockBackgroundService : BackgroundService
    {
        public static Dictionary<LogsEnum, string> Logs = new Dictionary<LogsEnum, string>
        {
            {LogsEnum.StartTime, DateTime.Now.ToString("MM-dd HH:mm:ss")},
            {LogsEnum.PolygonResponse, "Not yet"},
            {LogsEnum.EmailsCount, "0"}
        };

        IServiceScopeFactory _service;
        HttpClient client = new HttpClient();

        public StockBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _service = scopeFactory;
        }

        async Task<string[]> Addresses()
        {
            using var scope = _service.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<StockDbContext>();

            try
            {
                return (from item in context.Users
                        select item.Email).ToArray();
            }
            catch { return new string[] { }; }
        }

        async Task<string> HTML()
        {
            // For illustration only, the api returns only one daily reslut for free account!
            // Also not all days return results! have to choose a fixed day!
            // DateTime yesterday = DateTime.Today.ToUniversalTime().AddDays(-1); 
            DateTime yesterday = new DateTime(2024, 7, 7, 0, 0, 0).ToUniversalTime();
            string from = yesterday.AddDays(-1).ToString("yyyy-MM-dd");
            string to = yesterday.ToString("yyyy-MM-dd");

            string html =
                @"<h1>Apple Stock Price</h1>
                <table style='width:100%; border-collapse: collapse;'>
                    <tr>
                        <th style='border: 1px solid black'>Name</th>
                        <th style='border: 1px solid black'>Value</th>
                    </tr>
                    <tr>
                        <td style='border: 1px solid black'>Open</td>
                        <td style='border: 1px solid black'>vOpen</td>
                    </tr>
                    <tr>
                        <td style='border: 1px solid black'>Close</td>
                        <td style='border: 1px solid black'>vClose</td>
                    </tr>
                    <tr>
                        <td style='border: 1px solid black'>High</td>
                        <td style='border: 1px solid black'>vHigh</td>
                    </tr>
                    <tr>
                        <td style='border: 1px solid black'>Low</td>
                        <td style='border: 1px solid black'>vLow</td>
                    </tr>
                </table>";

            try
            {
                var response = await client.GetAsync($"https://api.polygon.io/v2/aggs/ticker/AAPL/range/1/day/{from}/{to}?apiKey=hk7FsLVmBv5fJP_b15cNEszPNF1TcVHr");
                string json = await response.Content.ReadAsStringAsync();

                Logs[LogsEnum.PolygonResponse] = json;

                Polygon polygon = JsonSerializer.Deserialize<Polygon>(json);
                Result result = polygon.results[0];

                // For illustration only, the api returns only one daily reslut for free account!
                return html
                    .Replace("vOpen", result.o.ToString())
                    .Replace("vClose", result.c.ToString())
                    .Replace("vHigh", result.h.ToString())
                    .Replace("vLow", result.l.ToString());
            }
            catch (Exception ex) { return string.Empty; }
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("arbweb.org@outlook.com", "eh3735hsy47Dh+-"),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("arbweb.org@outlook.com"),
                Subject = "Stock Market",
                IsBodyHtml = true,
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                string[] addresses = await Addresses();

                Logs[LogsEnum.EmailsCount] = addresses.Length.ToString();

                string html = await HTML();

                if (!string.IsNullOrEmpty(html))
                {
                    mailMessage.Body = html;

                    foreach (string recipient in addresses)
                    {
                        mailMessage.To.Clear();
                        mailMessage.To.Add(recipient);
                        try
                        {
                            smtpClient.Send(mailMessage);
                        }
                        catch { }
                    }
                }

                await Task.Delay(6 * 60 * 60 * 1000, stoppingToken);
            }
        }
    }
}