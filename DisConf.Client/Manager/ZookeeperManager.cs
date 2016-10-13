using System;
using System.Threading;
using DisConf.Utility.Path;
using ZooKeeperNet;

namespace DisConf.Client.Manager
{
    internal class ZookeeperManager : BaseManager, IWatcher, IDisposable
    {
        #region Event

        public delegate void NodeValueChanged(string app, string env, string config);
        /// <summary>
        /// 配置变更事件
        /// </summary>
        public event NodeValueChanged NodeValueChangedHandler;

        public delegate void ZookeeperConnected(object sender);
        /// <summary>
        /// Zookeeper连接成功
        /// </summary>
        public event ZookeeperConnected ZookeeperConnectedHandler;

        public delegate void ZookeeperDisconnected(object sender);
        /// <summary>
        /// Zookeeper连接失败
        /// </summary>
        public event ZookeeperDisconnected ZookeeperDisconnectedHandler;

        public delegate void NodeChanged(object sender, string nodeName);

        public event NodeChanged NodeAdded;
        public event NodeChanged NodeRemove;

        #endregion


        private const int RetryPeriodSeconds = 2;

        /// <summary>
        /// zookeeper会话超时
        /// </summary>
        private static readonly TimeSpan SessionTimeOutSpan = new TimeSpan(0, 0, 10);
        /// <summary>
        /// zookeeper连接超时
        /// </summary>
        private static readonly TimeSpan ConnectionTimeOutSpan = new TimeSpan(0, 0, 3);

        private static readonly object LockObj = new object();
        private static ZookeeperManager _manager;
        public static ZookeeperManager Instance
        {
            get
            {
                if (_manager == null)
                {
                    lock (LockObj)
                    {
                        if (_manager == null)
                        {
                            _manager = new ZookeeperManager();
                        }
                    }
                }
                return _manager;
            }
        }

        private string _host, _app, _env;
        private string _addPath, _delPath;

        private CountdownEvent _countdown;

        private ZooKeeper _zk;

        private bool _initlized;

        private ZookeeperManager()
        {
            this._initlized = false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="host">Zookeeper Host</param>
        /// <param name="app">APP名称</param>
        /// <param name="env">ENV名称</param>
        /// <returns></returns>
        public bool Initialize(string host, string app, string env)
        {
            this._host = host;
            this._app = app;
            this._env = env;

            this._addPath = ZooPathManager.GetAddPath(app, env);
            this._delPath = ZooPathManager.GetDelPath(app, env);

            this._initlized = true;

            Connect();

            return true;
        }

        /// <summary>
        /// 添加监控节点
        /// </summary>
        /// <param name="path"></param>
        public void Watch(string path)
        {
            if (!this._initlized)
            {
                throw new InvalidOperationException();
            }

            this._zk.Exists(path, this);
        }

        /// <summary>
        /// 连接Zookeeper
        /// </summary>
        private void Connect()
        {
            this.Dispose();

            this._zk = new ZooKeeper(this._host, SessionTimeOutSpan, this);

            this._countdown = new CountdownEvent(1);
            this._countdown.Wait(ConnectionTimeOutSpan);
        }

        /// <summary>
        /// 重链接
        /// </summary>
        private void ReConnect()
        {
            lock (LockObj)
            {
                while (true)
                {
                    try
                    {
                        if (!this._zk.State.Equals(ZooKeeper.States.CLOSED))
                        {
                            break;
                        }

                        Dispose();

                        Connect();
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(RetryPeriodSeconds * 1000);
                    }
                }
            }
        }

        public void Process(WatchedEvent @event)
        {
            Log.InfoFormat("监控变化 {0}", @event.ToString());

            if (@event.State == KeeperState.SyncConnected && @event.Type == EventType.None)
            {
                if (this._countdown.CurrentCount != 0)
                {
                    this._countdown.Signal();
                }
                if (this.ZookeeperConnectedHandler != null)
                {
                    this.ZookeeperConnectedHandler(this);
                }
            }
            else if (@event.State == KeeperState.SyncConnected && @event.Type == EventType.NodeDataChanged)
            {
                if (@event.Path.Equals(this._addPath))
                {
                    if (this.NodeAdded != null)
                    {
                        var data = _zk.GetData(this._addPath, false, null);
                        this.NodeAdded(this, System.Text.Encoding.Default.GetString(data));
                    }
                }
                else if (@event.Path.Equals(this._delPath))
                {
                    if (this.NodeRemove != null)
                    {
                        var data = _zk.GetData(this._delPath, false, null);
                        this.NodeRemove(this, System.Text.Encoding.Default.GetString(data));
                    }
                }
                else if (this.NodeValueChangedHandler != null)
                {
                    string app = null, env = null, name = null;

                    if (ZooPathManager.TryGetInfo(@event.Path, out app, out env, out name))
                    {
                        this.NodeValueChangedHandler(app, env, name);
                    }
                }
            }
            else if (@event.State == KeeperState.Disconnected)
            {
                if (this.ZookeeperDisconnectedHandler != null)
                {
                    this.ZookeeperDisconnectedHandler(this);
                }
            }
            else if (@event.State == KeeperState.Expired)
            {
                this.ReConnect();
            }

            //重新监视当前节点
            if (!string.IsNullOrEmpty(@event.Path))
            {
                this.Watch(@event.Path);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this._zk != null && _zk.State.Equals(ZooKeeper.States.CONNECTED))
            {
                this._zk.Dispose();
                this._zk = null;
            }

            if (this._countdown != null)
            {
                this._countdown.Dispose();
                this._countdown = null;
            }
        }
    }
}
