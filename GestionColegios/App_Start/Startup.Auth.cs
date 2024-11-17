using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using GestionColegios.Models;
using Microsoft.Owin.Security;

namespace GestionColegios
{
    public partial class Startup
    {
        // Para obtener más información sobre cómo configurar la autenticación, visite https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure el contexto de base de datos, el administrador de usuarios y el administrador de inicios de sesión para usar una única instancia por solicitud
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Permitir que la aplicación use una cookie para almacenar información para el usuario que inicia sesión
            // y una cookie para almacenar temporalmente información sobre un usuario que inicia sesión con un proveedor de inicio de sesión de terceros
            // Configurar cookie de inicio de sesión
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                    validateInterval: TimeSpan.FromMinutes(2),  // Valida el token cada 2 minutos
                    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                },
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(60), // Tiempo de expiración de la cookie
                SlidingExpiration = true,
            });

            // Permite que la aplicación use una cookie de inicio de sesión externa
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Permite que la aplicación almacene temporalmente la información del usuario cuando se verifica el segundo factor en el proceso de autenticación de dos factores.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Permite que la aplicación recuerde el segundo factor de verificación de inicio de sesión, como el teléfono o correo electrónico.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Middleware para actualizar el SecurityStamp mientras el usuario está activo
            app.Use(async (context, next) =>
            {
                if (context.Authentication.User.Identity.IsAuthenticated)
                {
                    var user = context.Authentication.User;
                    var signInManager = context.Get<ApplicationSignInManager>();
                    var userManager = context.Get<ApplicationUserManager>();

                    var applicationUser = await userManager.FindByIdAsync(user.Identity.GetUserId());
                    if (applicationUser != null)
                    {
                        // Regenera la identidad y vuelve a iniciar sesión para renovar el SecurityStamp
                        var identity = await userManager.CreateIdentityAsync(applicationUser, DefaultAuthenticationTypes.ApplicationCookie);
                        context.Authentication.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);
                    }
                }

                await next.Invoke();
            });

            // Quitar los comentarios de las siguientes líneas para habilitar el inicio de sesión con proveedores de inicio de sesión de terceros
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

    }
}