using DAL;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Admin.Security
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization == null)
                {
                    actionContext.Response = actionContext.Request
                        .CreateResponse(HttpStatusCode.Unauthorized);
                }
                else
                {
                    string authenticationToken = actionContext.Request.Headers
                                                .Authorization.Parameter;
                    string decodedAuthenticationToken = Encoding.UTF8.GetString(
                        Convert.FromBase64String(authenticationToken));
                    string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                    string username = usernamePasswordArray[0];
                    string password = usernamePasswordArray[1];
                    UtentiRepository repository = new UtentiRepository();

                    if (repository.LoginWs(username, password))
                    {
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                    }
                    else
                    {
                       
                    }
                }
            }
            catch
            {
                actionContext.Response = actionContext.Request
                           .CreateResponse(HttpStatusCode.Unauthorized);
            }
            
        }
    }
}