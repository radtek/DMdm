﻿using DormManage.BLL.FlexPlus;
using DormManage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DormManage.Web.UI.FlexPlus
{
    public partial class DormSuggestHandle : BasePage
    {
        private string key = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            key = Request.Params["id"].ToString();
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    InitData(key);
                }
            }
        }

        private void InitData(string key)
        {
            var bll = new FlexPlusBLL();
            var dt = bll.GetDormSuggestByID(key);
            if (DataTableHelper.IsEmptyDataTable(dt))
            {
                txtResponse.Disabled = false;
                btnSave.Visible = true;
                btnSave.Enabled = true;
                return;
            }

            var dr = dt.Rows[0];
            txtEmployeeNo.Text = dr["EmployeeNo"].ToString();
            txtCName.Text = dr["CName"].ToString();
            txtMobileNo.Text = dr["MobileNo"].ToString();
            var sDate = (dr["CreateDate"] == null) ? ""
                                                   : Convert.ToDateTime(dr["CreateDate"]).ToString("yyyy-MM-dd HH:mm");
            txtCreateDate.Text = sDate;
            txtSuggest.Value = dr["Suggest"].ToString();
            txtResponse.Value = dr["Response"].ToString();

            int nStatus = 0;
            nStatus = Convert.ToInt32(dr["Status"]);
            if (nStatus != 0)
            {
                txtResponse.Disabled = true;
                btnSave.Visible = false;
                btnSave.Enabled = false;
            }
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            var bll = new FlexPlusBLL();
            var sMsg = txtResponse.Value.Trim();
            //var sHandlerWorkdayNo = (base.UserInfo == null ? base.SystemAdminInfo.Account : base.UserInfo.EmployeeNo);
            var sEnName = (base.UserInfo == null ? base.SystemAdminInfo.Account : base.UserInfo.EName);
            bll.HandleSuggest(key, sEnName, sMsg);

            RunScript(this, "myscript", "saveComplete();");

        }
    }
}