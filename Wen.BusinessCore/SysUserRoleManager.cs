using Wen.Model;
using Wen.DbCore;
using Wen.IDAL;
using System.Linq.Expressions;
using Wen.Models.Entity;

namespace Wen.BusinessCore
{

  public class SysUserRoleManager :DatabaseHelper.RepositoryBase<SysUserRole>, ISysUserRoleManager
   {
        #region 方法
       /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  SysUserRole _GetById(object id)
        {
            return  GetById(id);
        }
        /// <summary>
        /// 获取所有实体
        /// </summary>
        public virtual List<SysUserRole> _GetAll()
        {
            return GetAll();
        }
        /// <summary>
        /// 插入实体
        /// </summary>
        public virtual int _Insert(SysUserRole entity)
        {
            return Insert(entity);
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        public virtual int _InsertRange(List<SysUserRole> entities)
        {
            return InsertRange(entities);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        public virtual bool _Update(SysUserRole entity)
        {
            return Update(entity);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        public virtual bool _UpdateRange(List<SysUserRole> entities)
        {
            return UpdateRange(entities);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        public virtual bool _Delete(object id)
        {
            return Delete(id);
        }
        /// <summary>
        /// 批量删除 - 根据主键集合
        /// </summary>
        public virtual int _DeleteByIds(object[] ids)
        {
            return DeleteByIds(ids);
        }
        /// <summary>
        /// 批量删除 - 根据条件
        /// </summary>
        public virtual int _Delete(Expression<Func<SysUserRole, bool>> whereExpression)
        {
            return Delete(whereExpression);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        public virtual List<SysUserRole> _GetPageList(Expression<Func<SysUserRole, bool>> whereExpression, int pageIndex, int pageSize,
            string orderByFields = "")
        {
            return GetPageList(whereExpression, pageIndex, pageSize, orderByFields);
        }
        /// <summary>
        /// 分页查询 - 返回总记录数
        /// </summary>
        public virtual List<SysUserRole> _GetPageList(Expression<Func<SysUserRole, bool>> whereExpression, int pageIndex, int pageSize,
           ref int totalCount, string orderByFields = "")
        {
           
            return GetPageList(whereExpression, pageIndex, pageSize,ref totalCount , orderByFields);
        }
        /// <summary>
        /// 查询单条记录
        /// </summary>
        public virtual SysUserRole _GetSingle(Expression<Func<SysUserRole, bool>> whereExpression)
        {
            return GetSingle(whereExpression);
        }
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        public virtual bool _Any(Expression<Func<SysUserRole, bool>> whereExpression)
        {
            return Any(whereExpression);
        }
        /// <summary>
        /// 统计记录数
        /// </summary>
        public virtual int _Count(Expression<Func<SysUserRole, bool>> whereExpression)
        {
            return Count(whereExpression);
        }
        #endregion
   }
}