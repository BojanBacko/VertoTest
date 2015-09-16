<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        Exception exc = Server.GetLastError();
        HttpException httpexc;

        if (Request.ServerVariables["Server_Name"].ToString() == "192.168.0.10")
        {
            if (exc.GetType() == typeof(HttpUnhandledException))
            {
                httpexc = (HttpException)exc;
                Response.Write(httpexc.GetHtmlErrorMessage());
            }
            else
            {
                httpexc = (HttpException)exc;
                if (httpexc.GetHttpCode().ToString() == "404")
                {
                    Server.Transfer("/CustomErrors.aspx?code=" + httpexc.GetHttpCode().ToString(), true);
                }
                else
                {
                    Response.Write(httpexc.GetHtmlErrorMessage());
                }
            }

            Response.End();
        }
        else
        {
            if (exc.GetType() == typeof(HttpUnhandledException))
            {
                httpexc = (HttpException)exc;
                Server.Transfer("/CustomErrors.aspx?code=" + httpexc.GetHttpCode().ToString(), true);
                Response.End();
            }
            else
            {
                httpexc = (HttpException)exc;
                Server.Transfer("/CustomErrors.aspx?code=" + httpexc.GetHttpCode(), true);
                Response.End();
            }
        }     
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
