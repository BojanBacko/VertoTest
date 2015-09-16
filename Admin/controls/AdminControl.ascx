<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminControl.ascx.cs" Inherits="Admin_Controls_AdminControl" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/admin/controls/ImageControl.ascx" TagName="ImageControl" TagPrefix="verto" %>
<%@ Register Src="~/admin/controls/VideoControl.ascx" TagName="VideoControl" TagPrefix="verto" %>
<%@ Register Src="~/admin/controls/FileUploadControl.ascx" TagName="FileUploadControl" TagPrefix="verto" %>

<script type="text/javascript">
    //var inps = document.getElementsByClassName("del");
    //for (var i = 0; i < inps.length; ++i) {
    //    var inp = inps[i];
    //    var oldclick = inp.onclick;
    //    inp.onclick = function () {
    //        if (confirm("Are you certain you want delete the selected item?"))
    //        { return false; }
    //        else
    //        {
    //            oldclick();
    //        }
    //    }
    //}
</script>

<asp:SqlDataSource ID="GridViewSource" runat="server" ConnectionString='<%$ConnectionStrings:MSSQL%>'
    SelectCommand="SELECT * FROM {0}"
    DeleteCommand="DELETE FROM {0} WHERE {1} = @{2}">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsCheck" runat="server" DataSourceMode="DataReader" ConnectionString='<%$ConnectionStrings:MSSQL%>'>
</asp:SqlDataSource>

<asp:SqlDataSource ID="FormViewSource" runat="server" ConnectionString='<%$ConnectionStrings:MSSQL%>' 
    SelectCommand="SELECT * FROM {0} WHERE {1} = @id"
    UpdateCommand = "UPDATE {0} SET {1} WHERE {2} = @{2}"
    InsertCommand = "INSERT INTO {0} {1} VALUES {2} SELECT @returnid = SCOPE_IDENTITY()"
    CancelSelectOnNullParameter="false" OnInserted="FormViewSource_Inserted">
    <SelectParameters>
        <asp:ControlParameter ControlID="gvPages" PropertyName="SelectedValue" Name="id" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter Direction="Output" Name="returnid" Type="Int32"/>
    </InsertParameters>
</asp:SqlDataSource>

<asp:Literal runat="server" ID="litError" />

    
<asp:Button runat="server" OnClick="btnAdd_Click"  ID="btnAdd" Visible='<%#InsertEnabled %>' Text='Add New' CssClass="add" style="margin:10px 20px;" ToolTip="To add new page click here." />

 <asp:GridView runat="server" DataSource='<%#GridViewGrab()%>' OnRowDeleting="GridViewDeleting" ID="gvPages" HeaderStyle-CssClass="gvheader" OnRowDataBound="gvPages_RowDataBound" OnRowCommand="gvPages_Row_Command" DataKeyNames="id" 
        AutoGenerateColumns="false" CssClass="gv" AlternatingRowStyle-CssClass="alt" GridLines="None" OnSelectedIndexChanged="ChangeRow" PagerStyle-CssClass="paging" OnPageIndexChanging="gvPages_PageIndexChanging">
        <Columns>
            
        </Columns>
        <AlternatingRowStyle CssClass="gridviewtralt" />
    </asp:GridView>

    <asp:FormView runat="server" DataSourceID="FormViewSource" DefaultMode="Edit" RenderOuterTable="false" ID="fvControls" Visible="false">
        <EditItemTemplate>     
            <asp:PlaceHolder runat="server" ID="form2"></asp:PlaceHolder>
            <asp:Panel runat="server" ID="pnlButtons" CssClass="buttons" Visible='<%#ShowSaveButton %>'>
                <asp:Button ID="formSend" Text="Save" CssClass="save-button" ToolTip="To save changes click here." runat="server" OnClick="FormView_Update" ValidationGroup='<%#ValidationGroup %>' />   
                <asp:Button runat="server" Text="Cancel" OnClick="Cancel" ID="Cancel" CssClass="cancel-button" ToolTip="To cancel current operation click here." />
            </asp:Panel>
        </EditItemTemplate>
        <InsertItemTemplate>
            <asp:PlaceHolder runat="server" ID="form" OnLoad="CheckFull"></asp:PlaceHolder>
            <asp:Panel runat="server" ID="pnlButtons" CssClass="buttons" Visible='<%#ShowSaveButton %>'>
                <asp:Button ID="formSend2" Text="Save" CssClass="save-button" ToolTip="To save changes click here." runat="server" OnClick="Formview_Insert" ValidationGroup='<%#ValidationGroup %>' />
                <asp:Button runat="server" OnClick="Cancel" ID="Cancel" Text="Cancel" CssClass="cancel-button" ToolTip="To cancel current operation click here." />
            </asp:Panel>
        </InsertItemTemplate>
    </asp:FormView>
