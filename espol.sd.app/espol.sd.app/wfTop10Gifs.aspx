<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfTop10Gifs.aspx.cs" Inherits="espol.sd.app.wfTop10Gifs" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ESPOL :: Sistemas Distribuidos - Top 10 Gifs</title>
    <style type="text/css">
        .ViewStyle {
            font-family: Tahoma;
            font-size: 10pt;
            color: #666666;
        }

        .HeaderStyle {
            background: #284775;
            Color: white;
            font-weight: bold;
            font-size: 11pt;
            vertical-align: top;
        }

        .FieldHeaderStyle {
            background: #284775;
            Color: white;
            font-weight: bold;
            font-size: 11pt;
            vertical-align: top;
        }

        .AlternatingRowStyle {
            background: #F7F6F3;
            Color: #284775;
            font-size: 10pt;
        }

        .RowStyle {
            background: white;
            Color: #284775;
            font-size: 10pt;
        }

        .SelectedRowStyle {
            background: #F4FA85;
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblTitulo" runat="server" Text="TOP 10 Gifs"></asp:Label>            
            <hr />
            <br />
            <asp:Button ID="btnRefresh" runat="server" Text="Actualizar" OnClick="btnRefresh_Click" />
            <asp:RadioButtonList ID="rtbnlstOrigen" runat="server" CssClass="RowStyle">
                <asp:ListItem Value="1" Selected="True">Base de Datos</asp:ListItem>
                <asp:ListItem Value="2">Caché</asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <asp:Label ID="lblMensaje" runat="server" CssClass="RowStyle"></asp:Label>
            <br />
            <br />
            <asp:GridView ID="grdGifs" runat="server" CssClass="ViewStyle" AutoGenerateColumns="False" Width="100%">
                <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                <HeaderStyle CssClass="HeaderStyle" />
                <RowStyle CssClass="RowStyle" />
                <Columns>
                    <asp:TemplateField HeaderText="Datos">
                        <ItemStyle Width="500px" HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text="Id Archivo" Width="150px" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                            <br />
                            <asp:Label ID="Label2" runat="server" Text="Nombre Archivo" Width="150px" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblNombreArchivo" runat="server" Text='<%# Eval("nombrearchivo") %>'></asp:Label>
                            <br />
                            <asp:Label ID="Label3" runat="server" Text="Numero de Accesos" Width="150px" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblNumAccesos" runat="server" Text='<%# Eval("num_accesos") %>'></asp:Label>
                            <br />
                            <asp:Label ID="Label4" runat="server" Text="Ranking" Width="150px" Font-Bold="true"></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imagen [gif]">
                        <ItemTemplate>
                            <asp:Image ID="img" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataRowStyle CssClass="RowStyle" Height="100px" HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    [No se encontraron Registros]
                </EmptyDataTemplate>
            </asp:GridView>

        </div>        
    </form>
</body>
</html>
