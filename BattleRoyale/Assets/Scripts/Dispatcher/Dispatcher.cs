using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dispatcher : MonoBehaviour
{
    private int _lock;
    private bool _run;
    private Queue<Action> _jobs = new Queue<Action>();

    public void BeginInvoke(Action action)
    {
        while (true)
        {
            if (0 == Interlocked.Exchange(ref _lock, 1))
            {
                _jobs.Enqueue(action);
                _run = true;
                Interlocked.Exchange(ref _lock, 0);
                break;
            }
        }
    }

    private void Update()
    {
        if (_run)
        {
            Queue<Action> execute = null;
            if (0 == Interlocked.Exchange(ref _lock, 1))
            {
                execute = new Queue<Action>(_jobs.Count);
                while (_jobs.Count != 0)
                {
                    Action job = _jobs.Dequeue();
                    execute.Enqueue(job);
                }
                _run = false;
                Interlocked.Exchange(ref _lock, 0);
            }

            if (execute != null)
            {
                while (execute.Count != 0)
                {
                    Action job = execute.Dequeue();
                    job();
                }
            }
        }
    }
}