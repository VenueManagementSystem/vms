<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoggedOutMaster.Master" AutoEventWireup="true" CodeBehind="GenericView.aspx.cs" Inherits="VMS.Module.GenericView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="container">
            <div class="row" id="controls">
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <asp:GridView ID="gdvGeneric" runat="server" AutoGenerateColumns="true">
                    <EmptyDataTemplate>No record found !!!</EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="row" id="buttons">
        </div>
    </form>
</asp:Content>
