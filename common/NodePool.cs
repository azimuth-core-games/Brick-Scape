using System;
using System.Collections.Generic;
using Godot;

namespace brickpong2D.scenes.common;

public partial class NodePool<T>(PackedScene scene, Node parent, Action<T> onCreate = null)
	where T : Node, IPoolable
{
	private readonly Stack<T> _available = new();
	private readonly List<T> _all = [];

	public int TotalCreated => _all.Count;
	public int AvailableCount => _available.Count;

	public T Acquire()
	{
		T instance;
		if (_available.Count > 0)
		{
			instance = _available.Pop();	//reuse node already existing
		}
		else
		{
			instance = scene.Instantiate<T>();	//if none exists, create new
			parent.AddChild(instance);
			_all.Add(instance);
			onCreate?.Invoke(instance);
		}
		
		instance.OnAcquire();
		return instance;
	}

	public void Release(T instance)
	{
		instance.OnRelease();
		_available.Push(instance);
	}

	public void ReleaseAll()
	{
		_available.Clear();
		foreach (var instance in _all)
		{
			instance.OnRelease();
			_available.Push(instance);
		}
	}
	
}