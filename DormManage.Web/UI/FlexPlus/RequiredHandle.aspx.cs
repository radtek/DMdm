﻿using DormManage.BLL.FlexPlus;
using DormManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DormManage.Web.UI.FlexPlus
{
    public partial class RequiredHandle : BasePage
    {
        private string mKind = string.Empty;
        private string mKey = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            mKind = Request.Params["kind"].ToString();
            mKey = Request.Params["key"].ToString();
            if (!IsPostBack)
            {
                BindSelect();
                if (!string.IsNullOrEmpty(mKey))
                {
                    InitData(mKey);
                }
            }
        }

        private void InitData(string mKey)
        {
            //load old data
            var bll = new FlexPlusBLL();
            var kind = (0 == string.Compare("RepairDorm", mKind, true)) ? 0 : 1;
            var sAppr = "1";
            var sResp = string.Empty;
            if(bll.IsHandledRequire(kind, mKey, out sAppr, out sResp))
            {
                ddlHandle.SelectedValue = sAppr;
                txtReply.Value = sResp;

                txtReply.Disabled = true;
                ddlHandle.Enabled = false;
                btnSave.Visible = false;
                btnSave.Enabled = false;
            }
            else
            {
                txtReply.Disabled = false;
                btnSave.Visible = true;
            }
        }

        private void BindSelect()
        {
            var lst = new List<TNameVal>();
            lst.Add(new TNameVal("批准", "1"));
            lst.Add(new TNameVal("拒绝", "2"));
            ddlHandle.DataSource = lst;
            ddlHandle.DataValueField = "Value";
            ddlHandle.DataTextField = "Name";
            ddlHandle.DataBind();
            ddlHandle.SelectedIndex = 0;
        }

        protected void ddlHandle_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == mKey || 0 == mKey.Length)
            {
                return;
            }

            var bll = new FlexPlusBLL();
            var sHandle = ddlHandle.SelectedValue;
            var sMsg = txtReply.Value.Trim();
            //var sHandlerWorkdayNo = (base.UserInfo == null ? base.SystemAdminInfo.Account : base.UserInfo.EmployeeNo);
            var sEnName = (base.UserInfo == null ? base.SystemAdminInfo.Account : base.UserInfo.EName);
            var kind = (0 == string.Compare("RepairDorm", mKind, true)) ? 0 : 1;
            bll.HandleRequired(kind, mKey, sEnName, sHandle, sMsg);

            var sCmd = string.Format("saveComplete({0});", kind);
            RunScript(this, "myscript", sCmd);
        }
    }
}