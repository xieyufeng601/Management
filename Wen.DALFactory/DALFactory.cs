using System.Reflection;
using Wen.Common;
using Wen.IDAL;
namespace Wen.DALFactory 
{
    public sealed class DataAccess
    {

        private static readonly string AssemblyPath = PubConstant.DalJk;
        public DataAccess() { }

        #region CreateObject 

        //不使用缓存
        private static object? CreateObjectNoCache(string AssemblyPath, string classNamespace)
        {
            try
            {
                object? objType = Assembly.Load(AssemblyPath).CreateInstance(classNamespace);
                return objType;
            }
            catch (System.Exception ex)
            {
                string str = ex.Message;// 记录错误日志
                                        //LogHelper.Error(ex.Message);
                return null;
            }

        }
        //使用缓存
        private static object? CreateObject(string AssemblyPath, string classNamespace)
        {
            object? objType = DataCache.GetCache(classNamespace);
            if (objType == null)
            {
                try
                {
                    objType = Assembly.Load(AssemblyPath).CreateInstance(classNamespace);
                    if (objType != null)
                    {
                        DataCache.SetCache(classNamespace, objType); // 写入缓存  
                    }
                }
                catch (System.Exception ex)
                {

                    string str = ex.Message; // 记录错误日志  
                }
            }
            return objType;
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ISysUserManager? CreatebaseSysUser()
        {
            string ClassNamespace = AssemblyPath + ".SysUserManager";

            object? objType = CreateObject(AssemblyPath, ClassNamespace);

            if (objType is ISysUserManager sysUserManager)
            {
                return sysUserManager;
            }
            else
            {

                return null;
            }
        }
        public static ISysRoleManager? CreatebaseSysRole()
        {
            string ClassNamespace = AssemblyPath + ".SysRoleManager"; object? objType = CreateObject(AssemblyPath, ClassNamespace); if (objType is ISysRoleManager _SysRoleManager) { return _SysRoleManager; } else { return null; }
        }
        public static ISysRoleMenuManager? CreatebaseSysRoleMenu()
        {
            string ClassNamespace = AssemblyPath + ".SysRoleMenuManager"; object? objType = CreateObject(AssemblyPath, ClassNamespace); if (objType is ISysRoleMenuManager _SysRoleMenuManager) { return _SysRoleMenuManager; } else { return null; }
        }

        public static ISysUserRoleManager? CreatebaseSysUserRole()
        {
            string ClassNamespace = AssemblyPath + ".SysUserRoleManager"; object? objType = CreateObject(AssemblyPath, ClassNamespace); if (objType is ISysUserRoleManager _SysUserRoleManager) { return _SysUserRoleManager; } else { return null; }
        }


        public static ISysDictManager? CreatebaseSysDict()
        {
            string ClassNamespace = AssemblyPath + ".SysDictManager"; object? objType = CreateObject(AssemblyPath, ClassNamespace); if (objType is ISysDictManager _SysDictManager) { return _SysDictManager; } else { return null; }
        }
        public static ISysFileManager? CreatebaseSysFile()
        {
            string ClassNamespace = AssemblyPath + ".SysFileManager"; object? objType = CreateObject(AssemblyPath, ClassNamespace); if (objType is ISysFileManager _SysFileManager) { return _SysFileManager; } else { return null; }
        }
        public static ISysMenuManager? CreatebaseSysMenu()
        {
            string ClassNamespace = AssemblyPath + ".SysMenuManager"; object? objType = CreateObject(AssemblyPath, ClassNamespace); if (objType is ISysMenuManager _SysMenuManager) { return _SysMenuManager; } else { return null; }


        }


    }

}