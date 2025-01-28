namespace MVC_DB_Mit_Bild
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Konfiguriert die Anwendung als MVC Anwendung
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            //app.MapGet("/", () => "Hello World!");
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{Id?}");

            app.Run();
        }
    }
}
