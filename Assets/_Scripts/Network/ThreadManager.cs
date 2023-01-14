using System;
using System.Collections.Generic;
using UnityEngine;
using Overworld;
using System.Linq;

namespace Network
{
    class ThreadManager : MonoBehaviour
    {
        private static readonly List<(Action, DateTime)> executeOnMainThread = new List<(Action, DateTime)>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static bool actionToExecuteOnMainThread = false;
        private static NetcodeMenuView _netcodeMenu;

        private void Update()
        {
            _netcodeMenu = NetcodeMenuView.Instance;
            UpdateMain();
        }

        public static void ExecuteOnMainThread(Action _action)
        {
            if (_action == null)
            {
                Debug.Log("No action to execute on main thread!");
                return;
            }

            lock (executeOnMainThread)
            {
                executeOnMainThread.Add((_action, DateTime.Now));
                actionToExecuteOnMainThread = true;
            }
        }

        public static void UpdateMain()
        {
            if (actionToExecuteOnMainThread)
            {
                executeCopiedOnMainThread.Clear();
                lock (executeOnMainThread)
                {
                    if (_netcodeMenu == null)
                    {
                        executeCopiedOnMainThread.AddRange(executeOnMainThread.Select(a => a.Item1));
                        executeOnMainThread.Clear();
                        actionToExecuteOnMainThread = false;
                    }
                    else
                    {
                        for(int i = executeOnMainThread.Count - 1; i >= 0; i--)
                        {
                            if(executeOnMainThread[i].Item2.AddMilliseconds(_netcodeMenu.MsDelay) <= DateTime.Now)
                            {
                                executeCopiedOnMainThread.Add(executeOnMainThread[i].Item1);
                                executeOnMainThread.RemoveAt(i);
                            }
                        }
                    }
                }

                for (int i = 0; i < executeCopiedOnMainThread.Count; i++)
                {
                    executeCopiedOnMainThread[i]();
                }
            }
        }
    }
}
