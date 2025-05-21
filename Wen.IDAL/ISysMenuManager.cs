using Wen.Model;
using System.Linq.Expressions;
using Wen.Models.Entity;
namespace Wen.IDAL 
{
 public interface ISysMenuManager 
 {
        
        #region 方法
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SysMenu _GetById(object id);
        /// <summary>
        /// 获取所有实体
        /// </summary>
        List<SysMenu> _GetAll();
        /// <summary>
        /// 插入实体
        /// </summary>
        int _Insert(SysMenu entity);
        /// <summary>
        /// 批量插入
        /// </summary>
        int _InsertRange(List<SysMenu> entities);
        /// <summary>
        /// 更新实体
        /// </summary>
        bool _Update(SysMenu entity);
        /// <summary>
        /// 批量更新
        /// </summary>
        bool _UpdateRange(List<SysMenu> entities);
        /// <summary>
        /// 删除实体
        /// </summary>
        bool _Delete(object id);
        /// <summary>
        /// 批量删除 - 根据主键集合
        /// </summary>
        int _DeleteByIds(object[] ids);
        /// <summary>
        /// 批量删除 - 根据条件
        /// </summary>
        int _Delete(Expression<Func<SysMenu, bool>> whereExpression);
        /// <summary>
        /// 分页查询
        /// </summary>
        List<SysMenu> _GetPageList(Expression<Func<SysMenu, bool>> whereExpression, int pageIndex, int pageSize,
           string orderByFields = "");
        /// <summary>
        /// 分页查询 - 返回总记录数
        /// </summary>
        List<SysMenu> _GetPageList(Expression<Func<SysMenu, bool>> whereExpression, int pageIndex, int pageSize,
            ref int totalCount, string orderByFields = "");
        /// <summary>
        /// 查询单条记录
        /// </summary>
        SysMenu _GetSingle(Expression<Func<SysMenu, bool>> whereExpression);
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        bool _Any(Expression<Func<SysMenu, bool>> whereExpression);
        /// <summary>
        /// 统计记录数
        /// </summary>
        int _Count(Expression<Func<SysMenu, bool>> whereExpression);

        #endregion
   

 }
 
}