<%@ Page Title="" Language="C#" MasterPageFile="~/HRM.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="EmployeeDocumentArchiveUpload.aspx.cs"
    Inherits="EATL.WebClient.CommonUI.EmployeeDocumentArchiveUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-family: Verdana;
            font-size: 11px;
            color: #000000; /*#570202;*/;
            font-weight: bold;
            text-align: left;
            padding-right: 15px;
            vertical-align : top;
            width: 23%;
        }
        .style2
        {
            width: 23%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="midAdmin">
        <link href="../Styles/AjaxClass.css" rel="stylesheet" type="text/css" />
        <div>
            <center>
                <h3>
                    ::--Employee's  Document Archive--::</h3>
            </center>
        </div><br />
        <div>
            <table width="100%">
                <tr>
                    <td class="style1" align="right">
                        Department Name :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlDepartment" Width="200px" runat="server">
                        </asp:DropDownList>
                        <cc1:CascadingDropDown ID="ddlDepartment_CascadingDropDown" runat="server" Category="Department"
                            Enabled="true" LoadingText="Please Wait" PromptText="Please Select Department"
                            TargetControlID="ddlDepartment" ServiceMethod="GetDepartment" ServicePath="~/HRM_WebService.asmx" />
                    </td>
                </tr>
                <tr>
                    <td class="style1" align="right">
                        Designation Name :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlDesignation" Width="200px" runat="server">
                        </asp:DropDownList>
                        <cc1:CascadingDropDown ID="ddlDesignation_CascadingDropDown" runat="server" Category="Designation"
                            Enabled="true" LoadingText="Please Wait" PromptText="Please Select Designation"
                            TargetControlID="ddlDesignation" ParentControlID="ddlDepartment" ServiceMethod="GetDesignatioByDepartment"
                            ServicePath="~/HRM_WebService.asmx" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Employee Name :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlEmployee" runat="server" Width="200px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:CascadingDropDown ID="ddlEmployee_CascadingDropDown" runat="server" Category="Employee"
                            Enabled="true" LoadingText="Please Wait" PromptText="Please Select Employee"
                            TargetControlID="ddlEmployee" ParentControlID="ddlDesignation" ServiceMethod="GetEmployeeByDesignation"
                            ServicePath="~/HRM_WebService.asmx" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Serial :
                    </td>
                    <td align="left">
                        <asp:Label ID="lblSerial" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Document Name :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtDocumentName" runat="server" Width="250px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                    </td>
                    <td align="left">
                        <asp:FileUpload ID="UploadedEmployeeFile" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                    </td>
                    <td align="left">
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"
                            Width="100px" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                            CausesValidation="False" Width="100px" />                        
                        <asp:HiddenField runat="server" ID="hdEmployeeDocumentArchiveID" />
                        <asp:HiddenField runat="server" ID="hdEmployeeDocumentArchiveDetailID" />                       
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td align="left">
                        <asp:Label ID="lblMessage" runat="server" Text="" CssClass="LabelTD"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:ListView ID="lvEmpDocumentArchive" runat="server" DataKeyNames="IID" OnItemDataBound="lvEmpDocumentArchive_ItemDataBound"
                            OnItemCommand="lvEmpDocumentArchive_ItemCommand">
                            <LayoutTemplate>
                                <table border="0" cellpadding="0" cellspacing="1" width="100%" style="border-style: none">
                                    <tr class="dGridHeaderClass" id="tr1" runat="server">
                                        <th id="th6" runat="server" align="center" width="5%">
                                            SL #
                                        </th>
                                        <th id="th5" runat="server" align="center">
                                            Employee Name
                                        </th>
                                        <th id="th1" runat="server" align="center">
                                            Document Name
                                        </th>
                                        <th id="th2" runat="server" align="center">
                                            File Path
                                        </th>
                                        <th id="th3" runat="server" align="center">
                                            View
                                        </th>
                                        <th id="th9" runat="server" align="center">
                                            Change
                                        </th>
                                    </tr>
                                    <tbody id="itemPlaceholder" runat="server">
                                    </tbody>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr class="dGridRowClass" id="trBody" runat="server">
                                    <td align="center">
                                        <asp:Label ID="lblSerialNolv" runat="server"></asp:Label>
                                        <asp:Label ID="lblEmployeeID" Visible="false" runat="server"></asp:Label>
                                    </td>
                                    <td align="center" valign="middle">
                                        <asp:Label ID="lblEmployeeName" runat="server" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td align="center" valign="middle">
                                        <asp:Label ID="lblDocumentName" runat="server" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblFilePath" runat="server" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download"></asp:LinkButton>
                                    </td>
                                    <td align="center" valign="middle">
                                        <asp:LinkButton ID="lnkModify" CausesValidation="false" Text="Delete" runat="server"></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="dGridAltRowClass" id="trBody" runat="server">
                                    <td align="center">
                                        <asp:Label ID="lblSerialNolv" runat="server"></asp:Label>
                                        <asp:Label ID="lblEmployeeID" Visible="false" runat="server"></asp:Label>
                                        <asp:Label ID="lblLeaveID" Visible="false" runat="server"></asp:Label>
                                    </td>
                                    <td align="center" valign="middle">
                                        <asp:Label ID="lblEmployeeName" runat="server" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td align="center" valign="middle">
                                        <asp:Label ID="lblDocumentName" runat="server" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblFilePath" runat="server" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download"></asp:LinkButton>
                                    </td>
                                    <td align="center" valign="middle">
                                        <asp:LinkButton ID="lnkModify" CausesValidation="false" Text="Delete" runat="server"></asp:LinkButton>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            No Data...
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="100px" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
