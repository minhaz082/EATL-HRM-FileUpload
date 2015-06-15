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
    public partial class EmployeeDocumentArchiveUpload : System.Web.UI.Page
    {
        static string employeeDocArchiveSession = "EmployeeDocumentArchiveSession";
        int serial = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                long userID = Convert.ToInt64(Session["UserID"]);                
                lblSerial.Text = serial.ToString();
                Session[employeeDocArchiveSession] = null;                
                btnSave.Visible = false;                
            }
        }                

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (txtDocumentName.Text != "")
            {
                if (UploadedEmployeeFile.PostedFile.FileName != "")
                {
                    lblMessage.Text = "";
                    string filename = Path.GetFileName(UploadedEmployeeFile.PostedFile.FileName);
                    string EmployeeDocumentFolderPath = ConfigurationManager.AppSettings["EmployeeDocumentFolderPath"];
                    string FilePath = Server.MapPath(EmployeeDocumentFolderPath +"/"+ filename);

                    //UploadedEmployeeFile.SaveAs(Server.MapPath("EmployeesDocument/" + filename));
                    // "Files/"= folder name where document are saved in solution
                    UploadedEmployeeFile.SaveAs(FilePath);     // "Files/"= folder name where document are saved in solution
                    List<EmployeeDocumentArchive> empDocumentArchiveDetailCollection = LoadFileUploadListView((int)EnumCollection.OperationName.AddNewData);
                    lvEmpDocumentArchive.DataSource = empDocumentArchiveDetailCollection;
                    lvEmpDocumentArchive.DataBind();

                    txtDocumentName.Text = "";
                    btnUpload.Visible = true;
                    btnSave.Visible = true;
                    lblSerial.Text = Convert.ToString((empDocumentArchiveDetailCollection.Count) + 1);                    
                }
                else
                {
                    lblMessage.Text = "Please Select a Document for upload";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }                
            }
            else
            {
                lblMessage.Text = "Please Enter Your Document Name";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
            
        }        
        
        private List<EmployeeDocumentArchive> LoadFileUploadListView(int operationNameID)
        {
            using (HRMCommonFacade _facade = new HRMCommonFacade())
            {
                List<EmployeeDocumentArchive> empDocumentArchiveDetailCollection = (List<EmployeeDocumentArchive>)Session[employeeDocArchiveSession];
                List<EmployeeDocumentArchive> EmployeeWiseDocumentArchiveDetail = _facade.GetEmployeeDocumentByEmpID(Convert.ToInt16(ddlEmployee.SelectedValue));

                try
                {
                    EmployeeDocumentArchive employeeDocumentArchive = new EmployeeDocumentArchive();

                    string filename = Path.GetFileName(UploadedEmployeeFile.PostedFile.FileName);
                    // ifteraz
                    string EmployeeDocumentFolderPath = ConfigurationManager.AppSettings["EmployeeDocumentFolderPath"];
                    string FilePath = Server.MapPath(EmployeeDocumentFolderPath + filename);
                    //ifteraz
                    //string filePath = "EmployeesDocument/" + filename;
                    
                    employeeDocumentArchive.IID = 0;
                    employeeDocumentArchive.EmployeeID = Convert.ToInt16(ddlEmployee.SelectedValue);
                    employeeDocumentArchive.DocumentName = txtDocumentName.Text.Trim();
                    employeeDocumentArchive.SerialNo = Convert.ToInt16(lblSerial.Text);
                    employeeDocumentArchive.FilePath = FilePath.Trim();
                    employeeDocumentArchive.CreateBy = Convert.ToInt64(Session["UserID"]);
                    employeeDocumentArchive.CreateDate = DateTime.Now;
                    employeeDocumentArchive.UpdateBy = Convert.ToInt64(Session["UserID"]);
                    employeeDocumentArchive.Updatedate = DateTime.Now;
                    employeeDocumentArchive.IsRemove = true;

                    if (empDocumentArchiveDetailCollection != null)
                    {
                        empDocumentArchiveDetailCollection.Add(employeeDocumentArchive);
                        Session[employeeDocArchiveSession] = empDocumentArchiveDetailCollection;
                        return empDocumentArchiveDetailCollection;
                    }
                    else
                    {
                        EmployeeWiseDocumentArchiveDetail.Add(employeeDocumentArchive);
                        Session[employeeDocArchiveSession] = EmployeeWiseDocumentArchiveDetail;
                        return EmployeeWiseDocumentArchiveDetail;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
       
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtDocumentName.Text = "";
            btnUpload.Visible = true;
            btnSave.Visible = true;            
            lblMessage.Text = string.Empty;
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
                    hdEmployeeDocumentArchiveDetailID.Value = EmployeeDocumentArchiveDetailID.ToString();

                    EmployeeDocumentArchive employeeDocumentArchive = employeeDocumentArchiveColl.Where(detail => detail.IID == EmployeeDocumentArchiveDetailID).SingleOrDefault();
                    if (employeeDocumentArchive != null)
                    {
                        employeeDocumentArchiveColl.Remove(employeeDocumentArchive);
                        Session[employeeDocArchiveSession] = employeeDocumentArchiveColl;
                        lvEmpDocumentArchive.DataSource = employeeDocumentArchiveColl;
                        lvEmpDocumentArchive.DataBind();
                        btnSave.Visible = true;
                        lblSerial.Text = Convert.ToString((employeeDocumentArchiveColl.Count) + 1);
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
                            lblMessage.Text = "File not found Please Upload the file again";
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (HRMCommonFacade facade = new HRMCommonFacade())
                {
                    EmployeeDocumentArchive documentArchive = new EmployeeDocumentArchive();
                    List<EmployeeDocumentArchive> documentArchiveList = new List<EmployeeDocumentArchive>();

                    documentArchiveList = facade.GetEmployeeDocumentByEmpID(Convert.ToInt64(ddlEmployee.SelectedValue));
                    if (documentArchiveList.Count > 0)
                    {
                        DeleteExistingData(Convert.ToInt64(ddlEmployee.SelectedValue));
                    }

                    List<EmployeeDocumentArchive> employeeDocumentArchiveDetailData = new List<EmployeeDocumentArchive>();
                    employeeDocumentArchiveDetailData = (List<EmployeeDocumentArchive>)Session[employeeDocArchiveSession];

                    foreach (ListViewItem item in lvEmpDocumentArchive.Items)
                    {
                        EmployeeDocumentArchive empDocumentArchive = new EmployeeDocumentArchive();
                        empDocumentArchive.IID = 0;
                        empDocumentArchive.EmployeeID = Convert.ToInt64(ddlEmployee.SelectedValue);
                        empDocumentArchive.SerialNo = Convert.ToInt32(((Label)item.FindControl("lblSerialNolv")).Text);
                        empDocumentArchive.DocumentName = ((Label)item.FindControl("lblDocumentName")).Text;
                        empDocumentArchive.FilePath = ((Label)item.FindControl("lblFilePath")).Text;
                        empDocumentArchive.CreateBy = Convert.ToInt64(Session["UserID"]);
                        empDocumentArchive.CreateDate = DateTime.Now;
                        empDocumentArchive.UpdateBy = Convert.ToInt64(Session["UserID"]);
                        empDocumentArchive.Updatedate = DateTime.Now;
                        empDocumentArchive.IsRemove = false;
                        facade.AddEmployeeDocumentArchive(empDocumentArchive);
                       
                    }

                    Session[employeeDocArchiveSession] = null;
                    lblMessage.Text = "Data Saved Successfully";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    ClearAllDataField();                   
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error : " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void DeleteExistingData(long employeeID)
        {
            int count = 0;
            using (DataHelper dHelper = new DataHelper())
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(dHelper.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("DeletePreviousDocumentByEmployeeID"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@EmployeeID", employeeID));

                            SqlParameter returnValue = new SqlParameter("@Return_Value", DbType.Int32);
                            returnValue.Direction = ParameterDirection.ReturnValue;

                            cmd.Parameters.Add(returnValue);
                            conn.Open();
                            cmd.Connection = conn;

                            cmd.ExecuteNonQuery();
                            count = Int32.Parse(cmd.Parameters["@Return_Value"].Value.ToString());                            
                            conn.Close();                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
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
                lblSerial.Text = Convert.ToString((EmployeePreviousDocument.Count) + 1);   
            }
        }

        private void ClearAllDataField()
        {
            ddlDepartment.SelectedIndex = 0;
            ddlDesignation.SelectedIndex = 0;
            ddlEmployee.SelectedIndex = 0;
            txtDocumentName.Text = "";
            btnUpload.Visible = false;
            btnSave.Visible = false;
        }

               
    }
}