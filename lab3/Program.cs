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

                string strResponse = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>����������:</H1>";
                strResponse += "<BR> ������: " + context.Request.Host;
                strResponse += "<BR> ����: " + context.Request.PathBase;
                strResponse += "<BR> ��������: " + context.Request.Protocol;
                strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";

                await context.Response.WriteAsync(strResponse);
            });
        });

        app.Map("/investor", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedBankDb cachedBankDb = context.RequestServices.GetService<CachedBankDb>();

                IEnumerable<Investor> investors = cachedBankDb.GetInvestor("client");

                string HtmlString = "<HTML><HEAD><TITLE>��������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                HtmlString += "<BODY><H1>������ ��������</H1><TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>���</TH>";
                HtmlString += "<TH>�������</TH>";
                HtmlString += "<TH>��������</TH>";
                HtmlString += "<TH>�����</TH>";
                HtmlString += "<TH>����� ��������</TH>";
                HtmlString += "<TH>����� ��������</TH>";
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
                HtmlString += "<BR><A href='/'>�������</A></BR>";
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
                                  "<label>������� id:</label>";



                if (context.Request.Cookies.TryGetValue("id", out var input_value))
                {
                    formHtml += $"<input type='number' name='id' value='{input_value}'><br><br>" +
                               "<input type='submit' value='�����'>" +
                               "</form>";
                }
                else
                {
                    formHtml += "<input type='number' name='id'><br><br>" +
                                "<input type='submit' value='�����'>" +
                                "</form>";
                }


                if (context.Request.Method == "POST")
                {
                    var id = int.Parse(context.Request.Form["id"]);

                    context.Response.Cookies.Append("id", id.ToString());

                    IEnumerable<Investor> byId = investors.Where(s => s.Id > id);

                    string HtmlString = "<HTML><HEAD><TITLE>��������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>������ ��������</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>��������</TH>";
                    HtmlString += "<TH>�����</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
                else
                {
                    string HtmlString = "<HTML><HEAD><TITLE>��������</TITLE></HEAD>" +
                     "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>������ ��������</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>��������</TH>";
                    HtmlString += "<TH>�����</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
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
                                    "<label>���:</label>";


                if (context.Session.Keys.Contains("name"))
                {
                    string name = context.Session.GetString("name");

                    formHtml += $"<input type='text' name='name' value='{name}'><br><br>" +
                                "<input type='submit' value='�����'>" +
                                 "</form>";
                }
                else
                {
                    formHtml += "<input type='text' name='name'><br><br>" +
                                "<input type='submit' value='�����'>" +
                                 "</form>";
                }

                if (context.Request.Method == "POST")
                {
                    string name = context.Request.Form["name"];

                    context.Session.SetString("name", name);

                    IEnumerable<Investor> investorByName = investor.Where(s => s.Name == name);

                    string HtmlString = "<HTML><HEAD><TITLE>��������</TITLE></HEAD>" +
                     "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>������ ��������</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>��������</TH>";
                    HtmlString += "<TH>�����</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
                else
                {

                    string HtmlString = "<HTML><HEAD><TITLE>��������</TITLE></HEAD>" +
                     "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>������ ��������</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>��������</TH>";
                    HtmlString += "<TH>�����</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
            });
        });

        app.Run((context) =>
        {

            CachedBankDb cachedBankDb = context.RequestServices.GetService<CachedBankDb>();

            cachedBankDb.GetInvestor("investor");


            string HtmlString = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
            "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<BODY><H1>�������</H1>";
            HtmlString += "<H2>������ �������� � ��� �������</H2>";
            HtmlString += "<BR><A href='/'>�������</A></BR>";
            HtmlString += "<BR><A href='/info'>����������</A></BR>";
            HtmlString += "<BR><A href='/investor'>������ ����������</A></BR>";
            HtmlString += "<BR><A href='/searchId'>������ ���������������</A></BR>";
            HtmlString += "<BR><A href='/searchName'>����� �� ���</A></BR>";
            HtmlString += "</BODY></HTML>";

            return context.Response.WriteAsync(HtmlString);

        });

        app.Run();
    }
}