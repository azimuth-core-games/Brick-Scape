using Godot;
using System;
using System.Collections.Generic;

public interface IPoolable
{
	void OnAcquire();
	void OnRelease();
}

public class NodePool<T> where T : Node, IPoolable
{
	private readonly PackedScene _scene;
	private readonly Node _parent;
	private readonly Action<T> _onCreate;
	private readonly Stack<T> _available = new();
	private readonly List<T> _all = new();

	public int TotalCreated => _all.Count;
	public int AvailableCount => _available.Count;

	public NodePool(PackedScene scene, Node parent, Action<T> onCreate = null)
	{
		_scene = scene;
		_parent = parent;
		_onCreate = onCreate;
	}

	public T Acquire()
	{
		T instance;
		if (_available.Count > 0)
		{
			instance = _available.Pop();
		}
		else
		{
			instance = _scene.Instantiate<T>();
			_parent.AddChild(instance);
			_all.Add(instance);
			_onCreate?.Invoke(instance);
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
