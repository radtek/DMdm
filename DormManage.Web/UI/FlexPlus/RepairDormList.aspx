﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepairDormList.aspx.cs" Inherits="DormManage.Web.UI.FlexPlus.RepairDormList" %>

<%@ Register Assembly="Ctl" Namespace="ExtendGridView" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>宿舍报修</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link rel="stylesheet" href="../../Styles/main.css" />
    <link rel="stylesheet" href="../../Styles/style.css" />
    <link href="../../Styles/reset.css" rel="stylesheet" />
    <link href="../../Styles/showLoading.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.showLoading.js" type="text/javascript"></script>
    <script src="../../Scripts/DatePicker/WdatePicker.js"></script>
    <script src="../../Scripts/ligerUI1.1.9/js/ligerui.all.js"></script>
    <link href="../../Scripts/ligerUI1.1.9/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    
    <style>
        .l-dialog-content {
            overflow: initial;
        }
    </style>
    
    <script type="text/javascript">
        function GetSelItem() {
            var recordId = "";
            var $chkSelect = $("input[name='chkSelect']:checked");
            if ($chkSelect.length == 0) {
                return null;
            }
            return $chkSelect.toArray();
        }

        function DoApply() {
            var arrSel = GetSelItem();
            if (!arrSel || !arrSel.length) {
                alert("请选择要处理的记录.");
                return;
            }
            var keys = "";
            for (var i = 0; i < arrSel.length; i++) {
                keys += $(arrSel[i]).val() + ",";
            }
            keys=keys.slice(0, -1);
            var sUrl = 'RequiredHandle.aspx?kind=RepairDorm&key=' + escape(keys);
            $.ligerDialog.open({
                title: "审核宿舍报修",
                top:30,                
                width: 580,
                height: 380,
                isResize: true,
                url: sUrl
            });
            return false;
        }
        function cancel() {
            $.ligerDialog.close();
        }
        function ViewRow(obj, id) {
            //var id = $(obj).attr("name");
            $.ligerDialog.open({
                title: "查看宿舍报修",
                top:15,
                left:80,
                width: 400,
                height: 550,
                isResize: true,
                url: 'RepairDormView.aspx?id=' + id
            });
        }


        function ViewRepairPicture(batchID){            
            if (!batchID || batchID <= 0) { return; }
            $.ligerDialog.open({
                title: "宿舍报修相关照片",
                top: 15,
                width: 800,
                height: 560,
                isResize: true,
                url: '/UI/Common/OpenImage.aspx?batchNo=' + batchID,
                buttons: [
                    { text: '关闭', onclick: function (item, dialog) { 
                        //dialog.hide(); 
                        dialog.close(); 
                    } }
                ]
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="navigation_wrap">
            <h3 class="nav_left">我的位置： <span id="navigation">Flex+后台-->宿舍报修</span>
            </h3>
        </div>
        <div class="wrapper">
            <div class="content">
                <div class="searchbar">
                    <table>
                        <tr>
                            <th>设备类型：</th>
                            <td>
                                <asp:DropDownList ID="ddlDeviceType" runat="server"></asp:DropDownList>
                            </td>
                            <th>状态：
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>
                            </td>
                            <th style="width:120px">提交日期开始：
                            </th>
                            <td>
                                <asp:TextBox ID="txtSubmitDayBegin" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                            </td>
                            <th>结束：
                            </th>
                            <td>
                                <asp:TextBox ID="txtSubmitDayEnd" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                            </td>
                            <th></th>
                            <td>
                                <asp:Button ID="btnSearch" Text="查 询" runat="server" class="publicBtn leftOff" TabIndex="4" OnClick="btnSearch_Click"></asp:Button>
                                <asp:Button ID="btnHandle" Text="审 批" runat="server" class="publicBtn leftOff" TabIndex="5" OnClientClick="return DoApply();"></asp:Button>
                                <asp:Button ID="btnExport" Text="导 出" runat="server" class="publicBtn leftOff" TabIndex="6" OnClick="btnExport_Click" ></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>

                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="loadSearch" class="loading">
                            <div id="searchResult">
                                <div class="viewlist">
                                    <cc1:GridView ID="GridView1" runat="server" CssClass="form" EmptyDataText="<无结果显示>"
                                        AutoGenerateColumns="False" EnableEmptyContentRender="True" DataKeyNames="ID" OnRowDataBound="GridView1_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle Width="20px" />
                                                <ItemTemplate>
                                                    <input type="radio" id="chkLeftSingle" value="<%#Eval("ID") %>" name="chkSelect" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CName" HeaderText="姓名"/>
                                            <asp:BoundField DataField="EmployeeNo" HeaderText="工号" />
                                            <asp:BoundField DataField="MobileNo" HeaderText="手机号" />
                                            <asp:BoundField DataField="DormAddress" HeaderText="宿舍地址" />
                                            <asp:BoundField DataField="RepairTime" HeaderText="预约时间" DataFormatString="{0:yyyy-MM-dd HH:mm}"/>
                                            <asp:BoundField DataField="DeviceType" HeaderText="待修设备类型" />
                                            <asp:BoundField DataField="RequireDesc" HeaderText="描述" />
                                            <asp:BoundField DataField="CreateDate" HeaderText="提交时间" DataFormatString="{0:yyyy-MM-dd HH:mm}"/>
                                            <asp:TemplateField HeaderText="状态">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltlStatus" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ModifyUserID" HeaderText="处理者" />
                                            <asp:TemplateField>
                                               <ItemTemplate>
                                                   <input id="btnView" name="<%#Eval("ID") %>" type="button" value="查看" class="publicBtn"
                                                       onclick="ViewRow(this, '<%#Eval("ID")%>')" />
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                        </Columns>                                        
                                    </cc1:GridView>
                                </div>
                                <div class="pagerbar">
                                    <table>
                                        <tr>
                                            <td class="pageright">
                                                <cc1:Pager ID="Pager1" runat="server" OnCommand="pagerList_Command" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Pager1" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
