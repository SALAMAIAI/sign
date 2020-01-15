<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../Pages/ApplicationMaster.Master" CodeBehind="viewAppIdikhar.aspx.cs" Inherits="WebApplication2.application.viewAppIdikhar" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form runat="server"  >
  <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
             <rsweb:ReportViewer ID="reportViewer1" runat="server" Height="23px" Visible="False">
            </rsweb:ReportViewer>
         <rsweb:ReportViewer ID="reportViewer2" runat="server" Height="23px" Visible="False">
            </rsweb:ReportViewer>
      </form>
</asp:Content>
