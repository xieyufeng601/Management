using Renci.SshNet;
using SqlSugar;
using System.Linq.Expressions;
using Wen.Common;
using Wen.Model;

namespace Wen.DbCore
{
    /// <summary>
    /// 数据库操作助手，提供安全执行、心跳检测和批量操作等功能
    /// </summary>
    public class DatabaseHelper : IDisposable
    {
        #region 字段和属性
        // 单例实现
        private static readonly Lazy<DatabaseHelper> lazy = new Lazy<DatabaseHelper>(() => new DatabaseHelper());
        public static DatabaseHelper Instance => lazy.Value;

        // 数据库连接配置
        private readonly string _connectionString;
        private readonly SqlSugar.DbType _dbType;
        /// <summary>
        /// 重连次数
        /// </summary>
        private readonly int _retryCount = 3;
        private readonly int _retryDelay = 1000; // 毫秒
        private readonly int _heartbeatInterval = 30000; // 心跳间隔(毫秒)

        // SSH配置
        private readonly SshConfig? _sshConfig;
        private SshClient? _sshClient;
        private ForwardedPortLocal? _forwardedPort;

        // 数据库上下文
        private SqlSugarClient _db;
        private System.Timers.Timer _heartbeatTimer;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly object _lockObject = new object();
        private bool _isDisposed = false;
        private bool _isSshConnected = false;

        private DbCoreDb dbCoreDb = new DbCoreDb();
        #endregion
        #region 构造函数和初始化
        // 私有构造函数
        private DatabaseHelper()
        {
            dbCoreDb = PubConstant.ConnectionString();
            //数据库类型
            _dbType = GetDatabaseType();

            // 从配置文件或环境变量获取连接信息
            _connectionString = GetConnectionString();

            _sshConfig = GetSshConfig();

            Console.WriteLine($"初始化数据库连接: {_dbType}");

            // 初始化数据库上下文
            _db = InitializeDbContext();

            // 启动心跳检测
            _heartbeatTimer = new System.Timers.Timer(_heartbeatInterval);
            StartHeartbeatTimer();
        }

        // 获取数据库类型
        private SqlSugar.DbType GetDatabaseType()
        {
            // 从配置中读取数据库类型

            string? dbTypeConfig = GetConfigValue("DatabaseType")?.ToLower();
            if (dbTypeConfig == null)
            {
                throw new InvalidOperationException("DatabaseType 配置值不能为空。");
            }
            // 根据配置映射到SqlSugar的DbType枚举
            return dbTypeConfig switch
            {
                "mysql" => SqlSugar.DbType.MySql,
                "sqlserver" => SqlSugar.DbType.SqlServer,
                "oracle" => SqlSugar.DbType.Oracle,
                "sqlite" => SqlSugar.DbType.Sqlite,
                "postgresql" => SqlSugar.DbType.PostgreSQL,
                _ => throw new NotSupportedException($"不支持的数据库类型: {dbTypeConfig}")
            };
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private string GetConnectionString()
        {
            // 根据数据库类型生成不同的连接字符串
            if (_sshConfig != null && !string.IsNullOrEmpty(_sshConfig.Host))
            {
                // 使用SSH隧道的连接字符串
                return _dbType switch
                {
                    SqlSugar.DbType.MySql =>
                        $"Server={_sshConfig.Host};Port={_sshConfig.LocalPort};Database={GetConfigValue("DatabaseName")};Uid={GetConfigValue("DatabaseUser")};Pwd={GetConfigValue("DatabasePassword")};",

                    SqlSugar.DbType.SqlServer =>
                        $"Data Source={_sshConfig.Host},{_sshConfig.LocalPort};Initial Catalog={GetConfigValue("DatabaseName")};User ID={GetConfigValue("DatabaseUser")};Password={GetConfigValue("DatabasePassword")};",

                    SqlSugar.DbType.Oracle =>
                        $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT={_sshConfig.LocalPort}))(CONNECT_DATA=(SERVICE_NAME={GetConfigValue("DatabaseName")})));User Id={GetConfigValue("DatabaseUser")};Password={GetConfigValue("DatabasePassword")};",

                    SqlSugar.DbType.PostgreSQL =>
                        $"Host={_sshConfig.Host};Port={_sshConfig.LocalPort};Username={GetConfigValue("DatabaseUser")};Password={GetConfigValue("DatabasePassword")};Database={GetConfigValue("DatabaseName")};",

                    _ => throw new NotSupportedException($"不支持的数据库类型: {_dbType}")
                };
            }
            else
            {
                // 直接连接的连接字符串
                return _dbType switch
                {

                    SqlSugar.DbType.Sqlite =>
                        $"Data Source={GetConfigValue("DatabaseHost")};Version=3;Pooling=True;Max Pool Size=100;",
                    SqlSugar.DbType.MySql =>
                        $"Server={GetConfigValue("DatabaseHost")};Port={GetConfigValue("DatabasePort")};Database={GetConfigValue("DatabaseName")};Uid={GetConfigValue("DatabaseUser")};Pwd={GetConfigValue("DatabasePassword")};",

                    SqlSugar.DbType.SqlServer =>
                        $"SERVER={GetConfigValue("DatabaseHost")},{GetConfigValue("DatabasePort")};DATABASE={GetConfigValue("DatabaseName")};UID={GetConfigValue("DatabaseUser")};PWD={GetConfigValue("DatabasePassword")};Connect Timeout=15;Encrypt=True;TrustServerCertificate=True;",

                    SqlSugar.DbType.Oracle =>
                        $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={GetConfigValue("DatabaseHost")})(PORT={GetConfigValue("DatabasePort")}))(CONNECT_DATA=(SERVICE_NAME={GetConfigValue("DatabaseName")})));User Id={GetConfigValue("DatabaseUser")};Password={GetConfigValue("DatabasePassword")};",

                    SqlSugar.DbType.PostgreSQL =>
                        $"Host={GetConfigValue("DatabaseHost")};Port={GetConfigValue("DatabasePort")};Username={GetConfigValue("DatabaseUser")};Password={GetConfigValue("DatabasePassword")};Database={GetConfigValue("DatabaseName")};",

                    _ => throw new NotSupportedException($"不支持的数据库类型: {_dbType}")
                };
            }
        }

        // 从配置源获取值的辅助方法
        private string? GetConfigValue(string key)
        {
            // 实际应用中应从配置文件、环境变量或配置服务获取
            // 这里使用简化实现
            var config = new Dictionary<string, string?>
        {
            { "DatabaseType",dbCoreDb.DatabaseType},
            { "DatabaseHost",dbCoreDb. DatabaseHost},
            { "DatabasePort", dbCoreDb.DatabasePort},
            { "DatabaseName",dbCoreDb.DatabaseName },
            { "DatabaseUser",dbCoreDb.DatabaseUser },
            { "DatabasePassword", dbCoreDb.DatabasePassword},
            { "SshHost",dbCoreDb.SshHost }, // 默认不使用SSH
            { "SshPort", dbCoreDb.SshPort },
            { "SshUser", dbCoreDb.SshUser },
            { "SshPassword",dbCoreDb.SshPassword },
            { "SshLocalPort", dbCoreDb.SshLocalPort }
        };



            return config.TryGetValue(key, out var value) ? value : null;
        }

        // 获取SSH配置
        private SshConfig? GetSshConfig()
        {
            string? sshHost = GetConfigValue("SshHost");

            if (string.IsNullOrEmpty(sshHost))
                return null;

            return new SshConfig
            {
                Host = sshHost!,
                Port = int.TryParse(GetConfigValue("SshPort"), out int port) ? port : 22,
                Username = GetConfigValue("SshUser")!,
                Password = GetConfigValue("SshPassword")!,
                PrivateKeyPath = GetConfigValue("SshPrivateKeyPath"),
                PrivateKeyPassphrase = GetConfigValue("SshPrivateKeyPassphrase"),
                DatabaseHost = GetConfigValue("DatabaseHost")!,
                DatabasePort = int.TryParse(GetConfigValue("DatabasePort"), out int dbPort) ? dbPort : 3306,
                LocalPort = int.TryParse(GetConfigValue("SshLocalPort"), out int localPort) ? localPort : 3307
            };
        }

        // 初始化数据库上下文
        private SqlSugarClient InitializeDbContext()
        {
            // 如果配置了SSH，则建立SSH隧道
            if (_sshConfig != null && !string.IsNullOrEmpty(_sshConfig.Host))
            {
                ConnectWithSsh();
            }

            var db = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings
                {
                    IsWithNoLockQuery = true,
                    IsAutoRemoveDataCache = true
                },
                AopEvents = new AopEvents
                {
                    OnLogExecuting = (sql, pars) =>
                    {
                        Console.WriteLine($"SQL: {sql}");
                    },
                    OnError = ex =>
                    {
                        Console.WriteLine($"SQL执行错误: {ex.Message}");
                    }
                }
            });

            db.Ado.IsEnableLogEvent = true;
            Console.WriteLine($"数据库连接初始化完成: {_dbType}");

            return db;
        }

        // 通过SSH连接数据库
        private void ConnectWithSsh()
        {
            try
            {
                Console.WriteLine($"尝试建立SSH隧道: {_sshConfig!.Username}@{_sshConfig.Host}:{_sshConfig.Port}");

                // 创建SSH客户端
                if (!string.IsNullOrEmpty(_sshConfig.PrivateKeyPath))
                {
                    // 使用私钥认证
                    var keyFile = new PrivateKeyFile(_sshConfig.PrivateKeyPath, _sshConfig.Password);
                    var keyFiles = new[] { keyFile };
                    _sshClient = new SshClient(_sshConfig.Host, _sshConfig.Port, _sshConfig.Username, keyFiles);
                }
                else
                {
                    // 使用密码认证
                    _sshClient = new SshClient(_sshConfig.Host, _sshConfig.Port, _sshConfig.Username, _sshConfig.Password);
                }

                _sshClient.Connect();

                // 创建本地端口转发
                _forwardedPort = new ForwardedPortLocal("127.0.0.1", (uint)_sshConfig.LocalPort, _sshConfig.DatabaseHost, (uint)_sshConfig.DatabasePort);
                _sshClient.AddForwardedPort(_forwardedPort);

                // 启动转发端口
                if (!_forwardedPort.IsStarted)
                    _forwardedPort.Start();

                _isSshConnected = true;
                Console.WriteLine($"SSH隧道已建立: 本地端口 {_sshConfig.LocalPort} -> {_sshConfig.DatabaseHost}:{_sshConfig.DatabasePort}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SSH连接失败: {ex.Message}");
                _isSshConnected = false;

                // 释放资源
                if (_forwardedPort != null && _forwardedPort.IsStarted)
                {
                    try { _forwardedPort.Stop(); } catch { }
                    _forwardedPort = null;
                }

                if (_sshClient != null && _sshClient.IsConnected)
                {
                    try { _sshClient.Disconnect(); } catch { }
                    _sshClient.Dispose();
                    _sshClient = null;
                }

                throw;
            }
        }

        // 启动心跳检测定时器
        private void StartHeartbeatTimer()
        {
            _heartbeatTimer.Elapsed += (sender, e) => CheckConnection();
            _heartbeatTimer.AutoReset = true;
            _heartbeatTimer.Enabled = true;
            Console.WriteLine($"心跳检测已启动，间隔: {_heartbeatInterval / 1000}秒");
        }

        // 检查数据库连接
        private void CheckConnection()
        {
            try
            {
                SafeExecute(db => db.Ado.ExecuteCommand("SELECT 1"));
                Console.WriteLine("心跳检测成功，数据库连接正常");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"心跳检测失败: {ex.Message}，尝试重新连接");

                // 如果使用SSH连接，重新建立隧道
                if (_isSshConnected)
                {
                    ReconnectSsh();
                }

                Reconnect();
            }
        }

        // 重新连接SSH
        private void ReconnectSsh()
        {
            lock (_lockObject)
            {
                try
                {
                    Console.WriteLine("尝试重新建立SSH隧道...");

                    // 关闭现有连接
                    if (_forwardedPort != null && _forwardedPort.IsStarted)
                    {
                        _forwardedPort.Stop();
                    }

                    if (_sshClient != null && _sshClient.IsConnected)
                    {
                        _sshClient.Disconnect();
                        _sshClient.Dispose();
                    }

                    // 重新连接
                    ConnectWithSsh();
                    Console.WriteLine("SSH隧道重新连接成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SSH重新连接失败: {ex.Message}");
                    _isSshConnected = false;
                }
            }
        }

        // 重新连接数据库
        private void Reconnect()
        {
            lock (_lockObject)
            {
                try
                {
                    // 关闭现有连接
                    if (_db != null)
                    {
                        try
                        {
                            _db.Ado.Close();
                        }
                        catch { }
                    }

                    // 重新初始化数据库上下文
                    _db = InitializeDbContext();
                    Console.WriteLine("数据库重新连接成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"数据库重新连接失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 安全执行数据库操作，包含异常重试机制
        /// </summary>
        public T SafeExecute<T>(Func<SqlSugarClient, T> action)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    // 使用锁确保线程安全
                    lock (_lockObject)
                    {
                        return action(_db);
                    }
                }
                catch (Exception ex)
                {
                    attempt++;
                    Console.WriteLine($"数据库操作失败，尝试 {attempt}/{_retryCount}: {ex.Message}");

                    if (attempt >= _retryCount)
                    {
                        Console.WriteLine("达到最大重试次数，操作失败");

                        // 如果使用SSH连接，重新建立隧道
                        if (_isSshConnected)
                        {
                            ReconnectSsh();
                        }

                        Reconnect();
                        throw;
                    }

                    if (IsConnectionException(ex))
                    {
                        // 如果使用SSH连接，重新建立隧道
                        if (_isSshConnected)
                        {
                            ReconnectSsh();
                        }

                        Reconnect();
                    }

                    Thread.Sleep(_retryDelay);
                }
            }
        }

        /// <summary>
        /// 安全执行数据库操作（异步），包含异常重试机制
        /// </summary>
        public async Task<T> SafeExecuteAsync<T>(Func<SqlSugarClient, Task<T>> action)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    // 使用SemaphoreSlim替代lock，支持异步等待
                    await _semaphore.WaitAsync();

                    try
                    {
                        return await action(_db);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
                catch (Exception ex)
                {
                    attempt++;
                    Console.WriteLine($"数据库操作失败，尝试 {attempt}/{_retryCount}: {ex.Message}");

                    if (attempt >= _retryCount)
                    {
                        Console.WriteLine("达到最大重试次数，操作失败");

                        // 如果使用SSH连接，重新建立隧道
                        if (_isSshConnected)
                        {
                            ReconnectSsh();
                        }

                        Reconnect();
                        throw;
                    }

                    if (IsConnectionException(ex))
                    {
                        // 如果使用SSH连接，重新建立隧道
                        if (_isSshConnected)
                        {
                            ReconnectSsh();
                        }

                        Reconnect();
                    }

                    await Task.Delay(_retryDelay);
                }
            }
        }

        // 判断是否是连接相关异常
        private bool IsConnectionException(Exception ex)
        {
            string[] connectionErrorKeywords = new[]
            {
            "closed", "connection", "timeout", "disposed", "broken",
            "terminated", "failed to connect", "lost connection",
            "ssh", "tunnel", "forward", "port"
        };

            string errorMessage = ex.Message.ToLower();
            return connectionErrorKeywords.Any(keyword => errorMessage.Contains(keyword)) ||
                   ex.InnerException != null && IsConnectionException(ex.InnerException);
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        public void BulkInsert<T>(List<T> entities) where T : class, new()
        {
            SafeExecute(db =>
            {
                db.Insertable(entities).ExecuteCommand();
                return true;
            });
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        public void BulkUpdate<T>(List<T> entities) where T : class, new()
        {
            SafeExecute(db =>
            {
                db.Updateable(entities).ExecuteCommand();
                return true;
            });
        }

        /// <summary>
        /// 开始一个事务
        /// </summary>
        public TransactionScope BeginTransaction()
        {
            return new TransactionScope(_db);
        }

        /// <summary>
        /// 事务管理类
        /// </summary>
        public class TransactionScope : IDisposable
        {
            private readonly SqlSugarClient _db;
            private bool _isCommitted = false;

            public TransactionScope(SqlSugarClient db)
            {
                _db = db;
                _db.Ado.BeginTran();
            }

            public void Commit()
            {
                _db.Ado.CommitTran();
                _isCommitted = true;
            }

            public void Rollback()
            {
                if (!_isCommitted)
                {
                    _db.Ado.RollbackTran();
                }
            }

            public void Dispose()
            {
                if (!_isCommitted)
                {
                    Rollback();
                }
            }
        }

        #endregion
        #region 通用数据操作基类

        /// <summary>
        /// 通用数据操作基类
        /// </summary>
        public abstract class RepositoryBase<T> where T : class, new()
        {
            protected readonly DatabaseHelper _dbHelper = DatabaseHelper.Instance;

            /// <summary>
            /// 获取单个实体
            /// </summary>
            public virtual T GetById(object id)
            {
                return _dbHelper.SafeExecute(db => db.Queryable<T>().InSingle(id));
            }

            /// <summary>
            /// 获取所有实体
            /// </summary>
            public virtual List<T> GetAll()
            {
                return _dbHelper.SafeExecute(db => db.Queryable<T>().ToList());
            }

            /// <summary>
            /// 插入实体
            /// </summary>
            public virtual int Insert(T entity)
            {
                return _dbHelper.SafeExecute(db => db.Insertable(entity).ExecuteReturnIdentity());
            }
            /// <summary>
            /// 批量插入
            /// </summary>
            public virtual int InsertRange(List<T> entities)
            {
                return _dbHelper.SafeExecute(db => db.Insertable(entities).ExecuteCommand());
            }
            /// <summary>
            /// 更新实体
            /// </summary>
            public virtual bool Update(T entity)
            {
                return _dbHelper.SafeExecute(db => db.Updateable(entity).ExecuteCommand() > 0);
            }
            /// <summary>
            /// 批量更新
            /// </summary>
            public virtual bool UpdateRange(List<T> entities)
            {
                return _dbHelper.SafeExecute(db => db.Updateable(entities).ExecuteCommand() > 0);
            }
            /// <summary>
            /// 删除实体
            /// </summary>
            public virtual bool Delete(object id)
            {
                return _dbHelper.SafeExecute(db => db.Deleteable<T>(id).ExecuteCommand() > 0);
            }
            /// <summary>
            /// 批量删除 - 根据主键集合
            /// </summary>
            public virtual int DeleteByIds(object[] ids)
            {
                return _dbHelper.SafeExecute(db => db.Deleteable<T>().In(ids).ExecuteCommand());
            }
            /// <summary>
            /// 批量删除 - 根据条件
            /// </summary>
            public virtual int Delete(Expression<Func<T, bool>> whereExpression)
            {
                return _dbHelper.SafeExecute(db => db.Deleteable(whereExpression).ExecuteCommand());
            }
            /// <summary>
            /// 分页查询
            /// </summary>
            public virtual List<T> GetPageList(Expression<Func<T, bool>> whereExpression, int pageIndex, int pageSize,
                string orderByFields = "")
            {
                return _dbHelper.SafeExecute(db =>
                {
                    var query = db.Queryable<T>().Where(whereExpression);
                    if (!string.IsNullOrEmpty(orderByFields))
                    {
                        query = query.OrderBy(orderByFields);
                    }
                    return query.ToPageList(pageIndex, pageSize);
                });
            }


            /// <summary>
            /// 分页查询 - 返回总记录数
            /// </summary>
            public virtual List<T> GetPageList(Expression<Func<T, bool>> whereExpression, int pageIndex, int pageSize,
               ref int totalCount, string orderByFields = "")
            {
                int localTotalCount = 0; // 使用局部变量存储总记录数
                var result = _dbHelper.SafeExecute(db =>
                {
                    var query = db.Queryable<T>().Where(whereExpression);
                    if (!string.IsNullOrEmpty(orderByFields))
                    {
                        query = query.OrderBy(orderByFields);
                    }
                    return query.ToPageList(pageIndex, pageSize, ref localTotalCount);
                });
                totalCount = localTotalCount; // 将局部变量的值赋回 ref 参数
                return result;
            }
            /// <summary>
            /// 查询单条记录
            /// </summary>
            public virtual T GetSingle(Expression<Func<T, bool>> whereExpression)
            {
                return _dbHelper.SafeExecute(db => db.Queryable<T>().Single(whereExpression));
            }

            /// <summary>
            /// 判断记录是否存在
            /// </summary>
            public virtual bool Any(Expression<Func<T, bool>> whereExpression)
            {
                return _dbHelper.SafeExecute(db => db.Queryable<T>().Any(whereExpression));
            }

            /// <summary>
            /// 统计记录数
            /// </summary>
            public virtual int Count(Expression<Func<T, bool>> whereExpression)
            {
                return _dbHelper.SafeExecute(db => db.Queryable<T>().Count(whereExpression));
            }
        }
        #endregion
        #region IDisposable实现
        // 实现IDisposable接口
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // 释放SSH资源
                    if (_forwardedPort != null && _forwardedPort.IsStarted)
                    {
                        try
                        {
                            _forwardedPort.Stop();
                        }
                        catch { }
                        _forwardedPort = null;
                    }

                    if (_sshClient != null && _sshClient.IsConnected)
                    {
                        try
                        {
                            _sshClient.Disconnect();
                        }
                        catch { }
                        _sshClient.Dispose();
                        _sshClient = null;
                    }

                    // 释放其他资源
                    if (_heartbeatTimer != null)
                    {
                        _heartbeatTimer.Stop();
                        _heartbeatTimer.Dispose();
                        // _heartbeatTimer = null;
                    }

                    if (_semaphore != null)
                    {
                        _semaphore.Dispose();
                    }

                    if (_db != null)
                    {
                        try
                        {
                            _db.Ado.Close();
                        }
                        catch { }
                        //  _db = null;
                    }
                }

                _isDisposed = true;
            }
        }

        ~DatabaseHelper()
        {
            Dispose(false);
        }
        #endregion
    }

}