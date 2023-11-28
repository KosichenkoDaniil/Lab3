using lab3;
using lab3.Services;
using lab3.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        var services = builder.Services;

        string connection = builder.Configuration.GetConnectionString("MSSQL");
        services.AddDbContext<BankDeposits1Context>(options => options.UseSqlServer(connection));



        services.AddMemoryCache();

        services.AddDistributedMemoryCache();
        services.AddScoped<CachedBankDb>();
        services.AddSession();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();

        var app = builder.Build();
        app.UseSession();


        app.Map("/info", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {

                string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>Информация:</H1>";
                strResponse += "<BR> Сервер: " + context.Request.Host;
                strResponse += "<BR> Путь: " + context.Request.PathBase;
                strResponse += "<BR> Протокол: " + context.Request.Protocol;
                strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";

                await context.Response.WriteAsync(strResponse);
            });
        });

        app.Map("/investor", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedBankDb cachedBankDb = context.RequestServices.GetService<CachedBankDb>();

                IEnumerable<Investor> investors = cachedBankDb.GetInvestor("client");

                string HtmlString = "<HTML><HEAD><TITLE>Подписки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                HtmlString += "<BODY><H1>Список клиентов</H1><TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>Имя</TH>";
                HtmlString += "<TH>Фамилия</TH>";
                HtmlString += "<TH>Отчество</TH>";
                HtmlString += "<TH>Адрес</TH>";
                HtmlString += "<TH>Номер телефона</TH>";
                HtmlString += "<TH>Номер паспорта</TH>";
                HtmlString += "</TR>";
                foreach (var investor in investors)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + investor.Id + "</TD>";
                    HtmlString += "<TD>" + investor.Name + "</TD>";
                    HtmlString += "<TD>" + investor.Surname + "</TD>";
                    HtmlString += "<TD>" + investor.Middlename + "</TD>";
                    HtmlString += "<TD>" + investor.Address + "</TD>";
                    HtmlString += "<TD>" + investor.Phonenumber + "</TD>";
                    HtmlString += "<TD>" + investor.PassportId + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>Главная</A></BR>";
                HtmlString += "</BODY></HTML>";

                await context.Response.WriteAsync(HtmlString);
            });
        });

        app.Map("/searchId", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedBankDb cachedBankDb = context.RequestServices.GetService<CachedBankDb>();
                IEnumerable<Investor> investors = cachedBankDb.GetInvestor("investor");

                string formHtml = "<form method='post' action='/searchId'>" +
                                  "<label>Введите id:</label>";



                if (context.Request.Cookies.TryGetValue("id", out var input_value))
                {
                    formHtml += $"<input type='number' name='id' value='{input_value}'><br><br>" +
                               "<input type='submit' value='Поиск'>" +
                               "</form>";
                }
                else
                {
                    formHtml += "<input type='number' name='id'><br><br>" +
                                "<input type='submit' value='Поиск'>" +
                                "</form>";
                }


                if (context.Request.Method == "POST")
                {
                    var id = int.Parse(context.Request.Form["id"]);

                    context.Response.Cookies.Append("id", id.ToString());

                    IEnumerable<Investor> byId = investors.Where(s => s.Id > id);

                    string HtmlString = "<HTML><HEAD><TITLE>Подписки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>Список клиентов</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>Имя</TH>";
                    HtmlString += "<TH>Фамилия</TH>";
                    HtmlString += "<TH>Отчество</TH>";
                    HtmlString += "<TH>Адрес</TH>";
                    HtmlString += "<TH>Номер телефона</TH>";
                    HtmlString += "<TH>Номер паспорта</TH>";
                    HtmlString += "</TR>";
                    foreach (var investor in byId)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + investor.Id + "</TD>";
                        HtmlString += "<TD>" + investor.Name + "</TD>";
                        HtmlString += "<TD>" + investor.Surname + "</TD>";
                        HtmlString += "<TD>" + investor.Middlename + "</TD>";
                        HtmlString += "<TD>" + investor.Address + "</TD>";
                        HtmlString += "<TD>" + investor.Phonenumber + "</TD>";
                        HtmlString += "<TD>" + investor.PassportId + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
                else
                {
                    string HtmlString = "<HTML><HEAD><TITLE>Подписки</TITLE></HEAD>" +
                     "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>Список клиентов</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>Имя</TH>";
                    HtmlString += "<TH>Фамилия</TH>";
                    HtmlString += "<TH>Отчество</TH>";
                    HtmlString += "<TH>Адрес</TH>";
                    HtmlString += "<TH>Номер телефона</TH>";
                    HtmlString += "<TH>Номер паспорта</TH>";
                    HtmlString += "</TR>";
                    foreach (var investor in investors)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + investor.Id + "</TD>";
                        HtmlString += "<TD>" + investor.Name + "</TD>";
                        HtmlString += "<TD>" + investor.Surname + "</TD>";
                        HtmlString += "<TD>" + investor.Middlename + "</TD>";
                        HtmlString += "<TD>" + investor.Address + "</TD>";
                        HtmlString += "<TD>" + investor.Phonenumber + "</TD>";
                        HtmlString += "<TD>" + investor.PassportId + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";


                    await context.Response.WriteAsync(HtmlString);
                }
            });
        });


        app.Map("/searchName", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedBankDb cachedBankDb = context.RequestServices.GetService<CachedBankDb>();
                IEnumerable<Investor> investor = cachedBankDb.GetInvestor("investors");

                string formHtml = "<form method='post' action='/searchName'>" +
                                    "<label>Имя:</label>";


                if (context.Session.Keys.Contains("name"))
                {
                    string name = context.Session.GetString("name");

                    formHtml += $"<input type='text' name='name' value='{name}'><br><br>" +
                                "<input type='submit' value='Поиск'>" +
                                 "</form>";
                }
                else
                {
                    formHtml += "<input type='text' name='name'><br><br>" +
                                "<input type='submit' value='Поиск'>" +
                                 "</form>";
                }

                if (context.Request.Method == "POST")
                {
                    string name = context.Request.Form["name"];

                    context.Session.SetString("name", name);

                    IEnumerable<Investor> investorByName = investor.Where(s => s.Name == name);

                    string HtmlString = "<HTML><HEAD><TITLE>Подписки</TITLE></HEAD>" +
                     "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>Список клиентов</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>Имя</TH>";
                    HtmlString += "<TH>Фамилия</TH>";
                    HtmlString += "<TH>Отчество</TH>";
                    HtmlString += "<TH>Адрес</TH>";
                    HtmlString += "<TH>Номер телефона</TH>";
                    HtmlString += "<TH>Номер паспорта</TH>";
                    HtmlString += "</TR>";
                    foreach (var Investor in investorByName)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + Investor.Id + "</TD>";
                        HtmlString += "<TD>" + Investor.Name + "</TD>";
                        HtmlString += "<TD>" + Investor.Surname + "</TD>";
                        HtmlString += "<TD>" + Investor.Middlename + "</TD>";
                        HtmlString += "<TD>" + Investor.Address + "</TD>";
                        HtmlString += "<TD>" + Investor.Phonenumber + "</TD>";
                        HtmlString += "<TD>" + Investor.PassportId + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
                else
                {

                    string HtmlString = "<HTML><HEAD><TITLE>Подписки</TITLE></HEAD>" +
                     "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>Список клиентов</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>Имя</TH>";
                    HtmlString += "<TH>Фамилия</TH>";
                    HtmlString += "<TH>Отчество</TH>";
                    HtmlString += "<TH>Адрес</TH>";
                    HtmlString += "<TH>Номер телефона</TH>";
                    HtmlString += "<TH>Номер паспорта</TH>";
                    HtmlString += "</TR>";
                    foreach (var Investor in investor)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + Investor.Id + "</TD>";
                        HtmlString += "<TD>" + Investor.Name + "</TD>";
                        HtmlString += "<TD>" + Investor.Surname + "</TD>";
                        HtmlString += "<TD>" + Investor.Middlename + "</TD>";
                        HtmlString += "<TD>" + Investor.Address + "</TD>";
                        HtmlString += "<TD>" + Investor.Phonenumber + "</TD>";
                        HtmlString += "<TD>" + Investor.PassportId + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
            });
        });

        app.Run((context) =>
        {

            CachedBankDb cachedBankDb = context.RequestServices.GetService<CachedBankDb>();

            cachedBankDb.GetInvestor("investor");


            string HtmlString = "<HTML><HEAD><TITLE>Емкости</TITLE></HEAD>" +
            "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<BODY><H1>Главная</H1>";
            HtmlString += "<H2>Данные записаны в кэш сервера</H2>";
            HtmlString += "<BR><A href='/'>Главная</A></BR>";
            HtmlString += "<BR><A href='/info'>Информация</A></BR>";
            HtmlString += "<BR><A href='/investor'>Список инвесторов</A></BR>";
            HtmlString += "<BR><A href='/searchId'>Список отсортированный</A></BR>";
            HtmlString += "<BR><A href='/searchName'>Поиск по ФИО</A></BR>";
            HtmlString += "</BODY></HTML>";

            return context.Response.WriteAsync(HtmlString);

        });

        app.Run();
    }
}