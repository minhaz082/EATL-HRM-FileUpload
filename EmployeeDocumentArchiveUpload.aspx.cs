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

namespace EATL.WebClient.CommonUI
{
    public partial class EmployeeDocumentArchiveUpload : System.Web.UI.Page
    {
        static string employeeArchiveDetailCollSe = "employeeArchiveDetailCollSession";
        int serial = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                long userID = Convert.ToInt64(Session["UserID"]);
                long employeeIID = Convert.ToInt64(Session["EmployeeID"]);
                lblSerial.Text = serial.ToString();                
                Session[employeeArchiveDetailCollSe] = null;                
                btnSave.Visible = false;
            }
        }                

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (txtDocumentName.Text != "")
            {
                if (fileUploadEmpDocuments.PostedFile.FileName != "")
                {
                    lblMessage.Text = "";
                    string filename = Path.GetFileName(fileUploadEmpDocuments.PostedFile.FileName);
                    fileUploadEmpDocuments.SaveAs(Server.MapPath("Files/" + filename));
                    List<EmployeeDocumentArchive> empDocumentArchiveDetailColl = CreateEmployeeDocumentArchiveDetail((int)EnumCollection.OperationName.AddNewData);
                    lvEmpDocumentArchive.DataSource = empDocumentArchiveDetailColl;
                    lvEmpDocumentArchive.DataBind();

                    txtDocumentName.Text = "";
                    btnUpload.Visible = true;
                    btnSave.Visible = true;
                    lblSerial.Text = Convert.ToString((empDocumentArchiveDetailColl.Count) + 1);                    
                }
                else
                {
                    lblMessage.Text = "Please select a Document for upload";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }                
            }
            else
            {
                lblMessage.Text = "Please Enter Document Name";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
            
        }        
        
        private List<EmployeeDocumentArchive> CreateEmployeeDocumentArchiveDetail(int operationNameID)
        {
            using (HRMCommonFacade _facade = new HRMCommonFacade())
            {
                List<EmployeeDocumentArchive> empDocumentArchiveDetailColl = (List<EmployeeDocumentArchive>)Session[employeeArchiveDetailCollSe];                
                List<EmployeeDocumentArchive> empDocumentArchiveDetailList = _facade.GetEmployeeDocumentByEmpID(Convert.ToInt16(ddlEmployee.SelectedValue));

                try
                {
                    EmployeeDocumentArchive employeeDocumentArchive = new EmployeeDocumentArchive();
                   
                    string filename = Path.GetFileName(fileUploadEmpDocuments.PostedFile.FileName);
                    string filePath = "Files/" + filename;
                    employeeDocumentArchive.IID = 0;
                    employeeDocumentArchive.EmployeeID = Convert.ToInt16(ddlEmployee.SelectedValue);
                    employeeDocumentArchive.DocumentName = txtDocumentName.Text.Trim();
                    employeeDocumentArchive.SerialNo = Convert.ToInt16(lblSerial.Text);
                    employeeDocumentArchive.FilePath = filePath.Trim();
                    employeeDocumentArchive.CreateBy = Convert.ToInt64(Session["UserID"]);
                    employeeDocumentArchive.CreateDate = DateTime.Now;
                    employeeDocumentArchive.UpdateBy = Convert.ToInt64(Session["UserID"]);
                    employeeDocumentArchive.Updatedate = DateTime.Now;
                    employeeDocumentArchive.IsRemove = true;
                    
                    if (empDocumentArchiveDetailColl != null)
                    {
                        empDocumentArchiveDetailColl.Add(employeeDocumentArchive);
                        Session[employeeArchiveDetailCollSe] = empDocumentArchiveDetailColl;
                        return empDocumentArchiveDetailColl;
                    }
                    else
                    {
                        empDocumentArchiveDetailList.Add(employeeDocumentArchive);
                        Session[employeeArchiveDetailCollSe] = empDocumentArchiveDetailList;
                        return empDocumentArchiveDetailList;
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
                if (e.CommandName == "DeleteData")
                { 
                    long EmployeeDocumentArchiveDetailID = Convert.ToInt64(e.CommandArgument);
                    hdEmployeeDocumentArchiveDetailID.Value = EmployeeDocumentArchiveDetailID.ToString();

                    List<EmployeeDocumentArchive> employeeDocumentArchiveColl = (List<EmployeeDocumentArchive>)Session[employeeArchiveDetailCollSe];
                    List<EmployeeDocumentArchive> employeeDocumentArchiveDetail = new List<EmployeeDocumentArchive>();
                    
                    if (employeeDocumentArchiveColl == null )
                    {
                        using (HRMCommonFacade facade = new HRMCommonFacade())
                        {
                            long EmployeeId = Convert.ToInt16(ddlEmployee.SelectedValue);                            
                            employeeDocumentArchiveColl = facade.GetEmployeeDocumentByEmpID(EmployeeId);
                        }                        
                    }

                    EmployeeDocumentArchive employeeDocumentArchive = employeeDocumentArchiveColl.Where(detail => detail.IID == EmployeeDocumentArchiveDetailID).SingleOrDefault();
                    if (employeeDocumentArchive != null)
                    {                       
                        employeeDocumentArchiveColl.Remove(employeeDocumentArchive);
                        Session[employeeArchiveDetailCollSe] = employeeDocumentArchiveColl;
                        lvEmpDocumentArchive.DataSource = employeeDocumentArchiveColl;
                        lvEmpDocumentArchive.DataBind();
                        btnSave.Visible = true;
                        lblSerial.Text = Convert.ToString((employeeDocumentArchiveColl.Count) + 1);
                    }
                    else
                    {
                        lblMessage.Text = " Please try again.";
                    }
                }

                else if (e.CommandName == "Download")
                {
                    long EmployeeDocumentArchiveDetailID = Convert.ToInt64(e.CommandArgument);
                    hdEmployeeDocumentArchiveDetailID.Value = EmployeeDocumentArchiveDetailID.ToString();

                    List<EmployeeDocumentArchive> employeeDocumentArchiveColl = (List<EmployeeDocumentArchive>)Session[employeeArchiveDetailCollSe];
                    List<EmployeeDocumentArchive> employeeDocumentArchiveDetail = new List<EmployeeDocumentArchive>();

                    if (employeeDocumentArchiveColl == null)
                    {
                        using (HRMCommonFacade facade = new HRMCommonFacade())
                        {
                            long EmployeeId = Convert.ToInt16(ddlEmployee.SelectedValue);
                            employeeDocumentArchiveColl = facade.GetEmployeeDocumentByEmpID(EmployeeId);
                        }
                    }

                    EmployeeDocumentArchive employeeDocumentArchive = employeeDocumentArchiveColl.Where(detail => detail.IID == EmployeeDocumentArchiveDetailID).SingleOrDefault();
                    if (employeeDocumentArchive != null)
                    {
                        string filePath = employeeDocumentArchive.FilePath.ToString();
                        Response.ContentType = "image/jpg";
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
                        Response.TransmitFile(Server.MapPath(filePath));
                        Response.End();
                        lblMessage.Text = "";
                    }
                }
                else
                {
                    lblMessage.Text = "Please Try Again";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }               
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error : " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void FillEmployeeDocumentArchiveDetail(EmployeeDocumentArchive employeeDocumentArchive)
        {
            try
            {                
                txtDocumentName.Text = employeeDocumentArchive.DocumentName;
                lblSerial.Text = employeeDocumentArchive.SerialNo.ToString();
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

                    List<EmployeeDocumentArchive> employeeDocumentArchiveDetailColl1 = new List<EmployeeDocumentArchive>();                  
                    employeeDocumentArchiveDetailColl1 = (List<EmployeeDocumentArchive>)Session[employeeArchiveDetailCollSe];

                    foreach (ListViewItem item in lvEmpDocumentArchive.Items)
                    {
                        EmployeeDocumentArchive ent = new EmployeeDocumentArchive();
                        ent.IID = 0;
                        ent.EmployeeID = Convert.ToInt64(ddlEmployee.SelectedValue);
                        ent.SerialNo = Convert.ToInt32(((Label)item.FindControl("lblSerialNolv")).Text);
                        ent.DocumentName = ((Label)item.FindControl("lblDocumentName")).Text;
                        ent.FilePath = ((Label)item.FindControl("lblFilePath")).Text;
                        ent.CreateBy = Convert.ToInt64(Session["UserID"]);
                        ent.CreateDate = DateTime.Now;
                        ent.UpdateBy = Convert.ToInt64(Session["UserID"]);
                        ent.Updatedate = DateTime.Now;
                        ent.IsRemove = false;
                        facade.AddEmployeeDocumentArchive(ent);
                       
                    }

                    Session[employeeArchiveDetailCollSe] = null;
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