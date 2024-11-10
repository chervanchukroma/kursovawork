using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Pomelo.EntityFrameworkCore.MySql;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // ������������ ���������� �� MySQL
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql("server=127.0.0.1;database=RegisterCourseWork;user=Roma;password=password",
            ServerVersion.AutoDetect("server=127.0.0.1;database=RegisterCourseWork;user=Roma;password=password"))
        );

        // ������������ �������������� ����� OpenID Connect
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddOpenIdConnect(options =>
        {
            options.Authority = "https://accounts.google.com";  // URL ������ OpenID ����������
            options.ClientId = "1072840364360-5gq2hn4hqv48vi450huql5vrg6dog654.apps.googleusercontent.com";  // ID �볺���
            options.ClientSecret = "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";  // ������ �볺���
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.Scope.Add("openid");
            options.Scope.Add("profile");
        });

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseAuthentication();  // ������ ��������������
        app.UseAuthorization();   // ������ �����������

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
