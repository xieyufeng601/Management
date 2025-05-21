
using Wen.IDAL;
using Wen.DALFactory;
using System.Linq.Expressions;
using Wen.Models;
namespace Wen.BusinessLogicLayer 
{

    public partial class SysUserBll
    {
        private readonly ISysUserManager dal;

        public SysUserBll()
        {
            dal = DataAccess.CreatebaseSysUser() ?? throw new ArgumentNullException(nameof(dal));
        }

        #region

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual SysUser _GetById(object id)
        {
            return dal._GetById(id);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        public virtual List<SysUser> _GetAll()
        {
            return dal._GetAll();
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        public virtual int _Insert(SysUser entity)
        {
            return dal._Insert(entity);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        public virtual int _InsertRange(List<SysUser> entities)
        {
            return dal._InsertRange(entities);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        public virtual bool _Update(SysUser entity)
        {
            return dal._Update(entity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        public virtual bool _UpdateRange(List<SysUser> entities)
        {
            return dal._UpdateRange(entities);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        public virtual bool _Delete(object id)
        {
            return dal._Delete(id);
        }

        /// <summary>
        /// 批量删除 - 根据主键集合
        /// </summary>
        public virtual int _DeleteByIds(object[] ids)
        {
            return dal._DeleteByIds(ids);
        }

        /// <summary>
        /// 批量删除 - 根据条件
        /// </summary>
        public virtual int _Delete(Expression<Func<SysUser, bool>> whereExpression)
        {
            return dal._Delete(whereExpression);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public virtual List<SysUser> _GetPageList(Expression<Func<SysUser, bool>> whereExpression, int pageIndex, int pageSize,
            string orderByFields = "")
        {
            return _GetPageList(whereExpression, pageIndex, pageSize, orderByFields);
        }

        /// <summary>
        /// 分页查询 - 返回总记录数
        /// </summary>
        public virtual List<SysUser> _GetPageList(Expression<Func<SysUser, bool>> whereExpression, int pageIndex, int pageSize,
           ref int totalCount, string orderByFields = "")
        {
            return dal._GetPageList(whereExpression, pageIndex, pageSize, ref totalCount, orderByFields);
        }

        /// <summary>
        /// 查询单条记录
        /// </summary>
        public virtual SysUser _GetSingle(Expression<Func<SysUser, bool>> whereExpression)
        {
            return dal._GetSingle(whereExpression);
        }

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        public virtual bool _Any(Expression<Func<SysUser, bool>> whereExpression)
        {
            return dal._Any(whereExpression);
        }

        /// <summary>
        /// 统计记录数
        /// </summary>
        public virtual int _Count(Expression<Func<SysUser, bool>> whereExpression)
        {
            return dal._Count(whereExpression);
        }

        #endregion
    }
}