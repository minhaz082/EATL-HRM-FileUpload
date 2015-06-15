using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EATL.BLL.Facade;
using System.IO;
using EATL.DAL;
using EATL.WebClient.HelperClass;
using EATL.SupportFramework;
using System.Collections;
using System.Runtime.Serialization;
using EATL.BLL;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace EATL.WebClient.CommonUI
{
    public partial class EmployeeDocumentArchiveList : System.Web.UI.Page
    {
        static string employeeDocArchiveSession = "EmployeeDocumentArchiveSession";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                long userID = Convert.ToInt64(Session["UserID"]);
                Session[employeeDocArchiveSession] = null;               
            }
        }

        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            using (HRMCommonFacade _facade = new HRMCommonFacade())
            {
                List<EmployeeDocumentArchive> EmployeePreviousDocument = _facade.GetEmployeeDocumentByEmpID(Convert.ToInt16(ddlEmployee.SelectedValue));
                lvEmpDocumentArchive.DataSource = EmployeePreviousDocument;
                lvEmpDocumentArchive.DataBind();                
            }
        }

        protected void lvEmpDocumentArchive_ItemCommand(object sender, ListViewCommandEventArgs e)
       {
            try
            {
                List<EmployeeDocumentArchive> employeeDocumentArchiveColl = (List<EmployeeDocumentArchive>)Session[employeeDocArchiveSession];
                List<EmployeeDocumentArchive> employeeDocumentArchiveDetail = new List<EmployeeDocumentArchive>();

                if (employeeDocumentArchiveColl == null)
                {
                    using (HRMCommonFacade facade = new HRMCommonFacade())
                    {
                        long EmployeeId = Convert.ToInt16(ddlEmployee.SelectedValue);
                        employeeDocumentArchiveColl = facade.GetEmployeeDocumentByEmpID(EmployeeId);
                    }
                }

                if (e.CommandName == "DeleteData")
                {
                    long EmployeeDocumentArchiveDetailID = Convert.ToInt64(e.CommandArgument);                    

                    EmployeeDocumentArchive employeeDocumentArchive = employeeDocumentArchiveColl.Where(detail => detail.IID == EmployeeDocumentArchiveDetailID).SingleOrDefault();
                    if (employeeDocumentArchive != null)
                    {
                        employeeDocumentArchiveColl.Remove(employeeDocumentArchive);
                        Session[employeeDocArchiveSession] = employeeDocumentArchiveColl;
                        lvEmpDocumentArchive.DataSource = employeeDocumentArchiveColl;
                        lvEmpDocumentArchive.DataBind();
                        //btnSave.Visible = true;
                        //lblSerial.Text = Convert.ToString((employeeDocumentArchiveColl.Count) + 1);
                    }
                }

                if (e.CommandName == "Download")
                {
                    long EmployeeDocumentArchiveDetailID = Convert.ToInt64(e.CommandArgument);                    

                    EmployeeDocumentArchive employeeDocumentArchive = employeeDocumentArchiveColl.Where(detail => detail.IID == EmployeeDocumentArchiveDetailID).SingleOrDefault();
                    if (employeeDocumentArchive != null)
                    {
                        string filePath = employeeDocumentArchive.FilePath.ToString();                        
                        string EmployeeDocumentFolderPath = ConfigurationManager.AppSettings["EmployeeDocumentFolderPath"];
                        string[] filePathsAll = Directory.GetFiles(Server.MapPath(EmployeeDocumentFolderPath));
                        if (filePathsAll.Contains(filePath))
                        {
                            lblMessage.Text = "";
                            Response.ContentType = "application/octect-stream";
                            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(filePath) + "\"");
                            Response.WriteFile(filePath);
                            Response.End();
                        }
                        else
                        {
                            lblMessage.Text = "File not found in the server";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error : " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        int lvRowCount = 0;
        protected void lvEmpDocumentArchive_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    ListViewDataItem currentItem = (ListViewDataItem)e.Item;
                    EmployeeDocumentArchive employeeDocumentArchive = (EmployeeDocumentArchive)((ListViewDataItem)(e.Item)).DataItem;

                    Label lblEmployeeID = (Label)currentItem.FindControl("lblEmployeeID");
                    Label lblEmployeeName = (Label)currentItem.FindControl("lblEmployeeName");
                    Label lblDocumentName = (Label)currentItem.FindControl("lblDocumentName");
                    Label lblFilePath = (Label)currentItem.FindControl("lblFilePath");
                    LinkButton lnkDownload = (LinkButton)currentItem.FindControl("lnkDownload");
                    LinkButton lnkModify = (LinkButton)currentItem.FindControl("lnkModify");

                    lvRowCount += 1;
                    Label lblSerialNolv = (Label)currentItem.FindControl("lblSerialNolv");
                    lblSerialNolv.Text = lvRowCount.ToString();

                    HRMCommonFacade facade = new HRMCommonFacade();
                    lblEmployeeID.Text = employeeDocumentArchive.EmployeeID.ToString();
                    lblEmployeeName.Text = facade.GetEmployeeByIID((long)employeeDocumentArchive.EmployeeID).Name;
                    lblDocumentName.Text = employeeDocumentArchive.DocumentName.ToString();
                    lblFilePath.Text = employeeDocumentArchive.FilePath.ToString();


                    lnkDownload.CommandArgument = employeeDocumentArchive.IID.ToString();
                    lnkDownload.CommandName = "Download";

                    lnkModify.CommandArgument = employeeDocumentArchive.IID.ToString();
                    lnkModify.CommandName = "DeleteData";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error : " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

}