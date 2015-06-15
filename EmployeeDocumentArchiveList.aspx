<%@ Page Title="" Language="C#" MasterPageFile="~/HRM.Master" AutoEventWireup="true" EnableEventValidation="false"
CodeBehind="EmployeeDocumentArchiveList.aspx.cs" Inherits="EATL.WebClient.CommonUI.EmployeeDocumentArchiveList" %>
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
                    ::--Employee's  Document Archive List--::</h3>
            </center>
        </div><br />
        <div>
            <table width="100%">
                <tr>
                    <td class="style1" align="right">
                        Department Name :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlDepartment" Width="250px" runat="server">
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
                        <asp:DropDownList ID="ddlDesignation" Width="250px" runat="server">
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
                        <asp:DropDownList ID="ddlEmployee" runat="server" Width="250px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:CascadingDropDown ID="ddlEmployee_CascadingDropDown" runat="server" Category="Employee"
                            Enabled="true" LoadingText="Please Wait" PromptText="Please Select Employee"
                            TargetControlID="ddlEmployee" ParentControlID="ddlDesignation" ServiceMethod="GetEmployeeByDesignation"
                            ServicePath="~/HRM_WebService.asmx" />
                    </td>
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
                        <asp:ListView ID="lvEmpDocumentArchive" runat="server" DataKeyNames="IID" 
                            OnItemCommand="lvEmpDocumentArchive_ItemCommand" OnItemDataBound="lvEmpDocumentArchive_ItemDataBound" >
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
                                        <th id="th9" runat="server" align="center" visible="false">
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
                                    <td align="center" valign="middle" visible="false">
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
                                    <td align="center" valign="middle" visible="false">
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
            </table>
        </div>
    </div>
</asp:Content>

