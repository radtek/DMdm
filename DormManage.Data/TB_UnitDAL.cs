﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using DormManage.Common;
using DormManage.Framework;
using DormManage.Framework.Enum;
using DormManage.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Web;

namespace DormManage.Data.DAL
{
    public class TB_UnitDAL
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Create(TB_Unit info)
        {
            try
            {
                Database db = DBO.CreateDatabase();
                return this.Create(info, (DbTransaction)null, db);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 事务添加
        /// </summary>
        /// <param name="info"></param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int Create(TB_Unit info, DbTransaction tran, Database db)
        {
            DbCommand dbCommandWrapper = null;
            int intId;
            string strInsertSql = "INSERT INTO TB_Unit (SiteID,DormAreaID,BuildingID,Name,Creator) VALUES(@SiteID,@DormAreaID,@BuildingID,@Name,@Creator)";
            string strSelectIdSql = ";SELECT SCOPE_IDENTITY()";
            try
            {
                dbCommandWrapper = db.GetSqlStringCommand(strInsertSql + strSelectIdSql);
                #region Add parameters
                db.AddInParameter(dbCommandWrapper, "@SiteID", DbType.Int32, info.SiteID);
                db.AddInParameter(dbCommandWrapper, "@DormAreaID", DbType.Int32, info.DormAreaID);
                db.AddInParameter(dbCommandWrapper, "@BuildingID", DbType.Int32, info.BuildingID);
                db.AddInParameter(dbCommandWrapper, "@Name", DbType.String, SetNullValue(string.IsNullOrEmpty(info.Name), info.Name));
                db.AddInParameter(dbCommandWrapper, "@Creator", DbType.String, SetNullValue(string.IsNullOrEmpty(info.Creator), info.Creator));
                #endregion
                if (tran == null)
                    intId = Convert.ToInt32(db.ExecuteScalar(dbCommandWrapper));
                else
                    intId = Convert.ToInt32(db.ExecuteScalar(dbCommandWrapper, tran));
                return intId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbCommandWrapper != null)
                {
                    dbCommandWrapper = null;
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Edit(TB_Unit info)
        {
            try
            {
                Database db = DBO.CreateDatabase();
                return this.Edit(info, (DbTransaction)null, db);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 事务更新
        /// </summary>
        /// <param name="info"></param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int Edit(TB_Unit info, DbTransaction tran, Database db)
        {
            DbCommand dbCommandWrapper = null;
            string strUpdateSql = @"UPDATE TB_Unit SET DormAreaID=@DormAreaID,BuildingID=@BuildingID, Name=@Name,UpdateBy=@UpdateBy,UpdateDate=@UpdateDate WHERE ID=@ID";
            try
            {
                dbCommandWrapper = db.GetSqlStringCommand(strUpdateSql);

                #region Add parameters
                db.AddInParameter(dbCommandWrapper, "@ID", DbType.Int32, info.ID);
                db.AddInParameter(dbCommandWrapper, "@BuildingID", DbType.Int32, info.BuildingID);
                db.AddInParameter(dbCommandWrapper, "@DormAreaID", DbType.Int32, info.DormAreaID);
                db.AddInParameter(dbCommandWrapper, "@Name", DbType.String, SetNullValue(string.IsNullOrEmpty(info.Name), info.Name));
                db.AddInParameter(dbCommandWrapper, "@UpdateBy", DbType.String, SetNullValue(string.IsNullOrEmpty(info.UpdateBy), info.UpdateBy));
                db.AddInParameter(dbCommandWrapper, "@UpdateDate", DbType.DateTime, DateTime.Now);
                #endregion
                if (tran == null)
                    return db.ExecuteNonQuery(dbCommandWrapper);
                else
                    return db.ExecuteNonQuery(dbCommandWrapper, tran);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbCommandWrapper != null)
                {
                    dbCommandWrapper = null;
                }
            }
        }

        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        /// <param name="intID"></param>
        /// <returns></returns>
        public TB_Unit Get(int intID)
        {
            DataTable dt = null;
            DbCommand dbCommandWrapper = null;
            TB_Unit mTB_Unit = null;
            try
            {
                string strSQL = @"select * from TB_Unit where 1=1";
                StringBuilder strBuilder = new StringBuilder(strSQL);
                Database db = DBO.GetInstance();
                dbCommandWrapper = db.DbProviderFactory.CreateCommand();
                dbCommandWrapper.CommandType = CommandType.Text;
                strBuilder.AppendLine(" AND ID = @ID");
                db.AddInParameter(dbCommandWrapper, "@ID", DbType.Int32, intID);
                dbCommandWrapper.CommandText = strBuilder.ToString();
                dt = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    mTB_Unit = new TB_Unit()
                    {
                        DormAreaID = Convert.ToInt32(dt.Rows[0][TB_Unit.col_DormAreaID]),
                        BuildingID=Convert.ToInt32(dt.Rows[0][TB_Unit.col_BuildingID]),
                        Name = Convert.ToString(dt.Rows[0][TB_Unit.col_Name]),
                    };
                }
                return mTB_Unit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbCommandWrapper != null)
                {
                    dbCommandWrapper = null;
                }
            }
        }


        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="info"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public DataTable GetTable(TB_Unit info, ref Pager pager)
        {
            DataTable dt = null;
            DbCommand dbCommandWrapper = null;
            try
            {
                string strSQL = @"SELECT A.*,B.Name AS BuildingName,C.Name AS DormAreaName
                                    FROM [TB_Unit] as A
                                    inner join TB_Building As B
                                    on A.BuildingID=B.ID
                                    inner join TB_DormArea AS C
                                    on A.DormAreaID=C.ID ";
                StringBuilder strBuilder = new StringBuilder(strSQL);
                Database db = DBO.GetInstance();
                dbCommandWrapper = db.DbProviderFactory.CreateCommand();
                dbCommandWrapper.CommandType = CommandType.Text;
                #region 拼接条件

                if (null != SessionHelper.Get(HttpContext.Current, TypeManager.User))
                {
                    strBuilder.AppendLine(@"inner join [TB_UserConnectDormArea] AS D
                                            on C.ID=D.[DormAreaID]
                                            where 1=1");
                    strBuilder.AppendLine(" AND D.[UserID] = @UserID");
                    db.AddInParameter(dbCommandWrapper, "@UserID", DbType.Int32, ((TB_User)SessionHelper.Get(HttpContext.Current, TypeManager.User)).ID);
                }
                else
                {
                    strBuilder.AppendLine(" where 1=1");
                }

                if (info.SiteID > 0)
                {
                    strBuilder.AppendLine(" AND A.SiteID = @SiteID");
                    db.AddInParameter(dbCommandWrapper, "@SiteID", DbType.Int32, info.SiteID);
                }
                if (!string.IsNullOrEmpty(info.Name))
                {
                    strBuilder.AppendLine(" AND A.NAME LIKE @NAME");
                    db.AddInParameter(dbCommandWrapper, "@NAME", DbType.String, "%" + info.Name + "%");
                }
                if (info.DormAreaID > 0)
                {
                    strBuilder.AppendLine(" AND A.DormAreaID = @DormAreaID");
                    db.AddInParameter(dbCommandWrapper, "@DormAreaID", DbType.String, info.DormAreaID);
                }
                if (info.BuildingID > 0)
                {
                    strBuilder.AppendLine(" AND A.BuildingID = @BuildingID");
                    db.AddInParameter(dbCommandWrapper, "@BuildingID", DbType.String, info.BuildingID);
                }
                #endregion
                if (pager != null && !pager.IsNull)
                {
                    strSQL = pager.GetPagerSql4Count(strBuilder.ToString());
                    dbCommandWrapper.CommandText = strSQL;
                    dt = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                    pager.TotalRecord = Convert.ToInt32(dt.Rows[0][0]);
                    dbCommandWrapper.CommandText = pager.GetPagerSql4Data(strBuilder.ToString(), DataBaseTypeEnum.sqlserver);
                }
                else
                {
                    dbCommandWrapper.CommandText = strBuilder.ToString();
                }
                dt = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbCommandWrapper != null)
                {
                    dbCommandWrapper = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public DataTable GetTable(TB_Unit info)
        {
            DataTable dt = null;
            DbCommand dbCommandWrapper = null;
            try
            {
                string strSQL = @"select * from TB_Unit where 1=1";
                StringBuilder strBuilder = new StringBuilder(strSQL);
                Database db = DBO.GetInstance();
                dbCommandWrapper = db.DbProviderFactory.CreateCommand();
                dbCommandWrapper.CommandType = CommandType.Text;
                if (info.ID > 0)
                {
                    strBuilder.AppendLine(" AND ID <> @ID");
                    db.AddInParameter(dbCommandWrapper, "@ID", DbType.Int32, info.ID);
                }
                strBuilder.AppendLine(" AND SiteID = @SiteID");
                db.AddInParameter(dbCommandWrapper, "@SiteID", DbType.String, info.SiteID);
                strBuilder.AppendLine(" AND NAME = @NAME");
                db.AddInParameter(dbCommandWrapper, "@NAME", DbType.String, info.Name);
                strBuilder.AppendLine(" AND DormAreaID = @DormAreaID");
                db.AddInParameter(dbCommandWrapper, "@DormAreaID", DbType.String, info.DormAreaID);
                strBuilder.AppendLine(" AND BuildingID = @BuildingID");
                db.AddInParameter(dbCommandWrapper, "@BuildingID", DbType.String, info.BuildingID);
                dbCommandWrapper.CommandText = strBuilder.ToString();
                dt = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbCommandWrapper != null)
                {
                    dbCommandWrapper = null;
                }
            }
        }

        private object SetNullValue(bool isNullValue, object value)
        {
            if (isNullValue)
                return DBNull.Value;
            else
                return value;
        }


        /// <summary>
        /// 获取site的所有单元信息
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public DataTable GetTableBySite(int siteID)
        {
            DataTable dt = null;
            DbCommand dbCommandWrapper = null;
            try
            {
                StringBuilder strBuilder = new StringBuilder(@"SELECT A.*,B.Name AS BuildingName,C.Name AS DormAreaName
                                    FROM [TB_Unit] as A
                                    inner join TB_Building As B
                                    on A.BuildingID=B.ID
                                    inner join TB_DormArea AS C
                                    on A.DormAreaID=C.ID");
                Database db = DBO.GetInstance();
                dbCommandWrapper = db.DbProviderFactory.CreateCommand();
                dbCommandWrapper.CommandType = CommandType.Text;
                strBuilder.AppendLine(" AND A.SiteID = @SiteID");
                db.AddInParameter(dbCommandWrapper, "@SiteID", DbType.Int32, siteID);
                dbCommandWrapper.CommandText = strBuilder.ToString();
                dt = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbCommandWrapper != null)
                {
                    dbCommandWrapper = null;
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strID"></param>
        public int Delete(string strID, DbTransaction tran, Database db)
        {
            if (string.IsNullOrEmpty(strID))
                return 0;
            DbCommand dbCommandWrapper = null;
            string strSql = @"DELETE FROM [TB_Unit] WHERE ID in (" + strID + ")";
            try
            {
                dbCommandWrapper = db.GetSqlStringCommand(strSql);
                if (tran == null)
                    return db.ExecuteNonQuery(dbCommandWrapper);
                else
                    return db.ExecuteNonQuery(dbCommandWrapper, tran);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbCommandWrapper != null)
                {
                    dbCommandWrapper = null;
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strID"></param>
        public int Delete(string strID)
        {
            try
            {
                Database db = DBO.CreateDatabase();
                return this.Delete(strID, (DbTransaction)null, db);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
