using System;
using System.Collections.Generic;
using Godot;

namespace brickpong2D.scenes.common;

public interface IPoolable
{
	void OnSpawn();
	void OnDespawn();
}

public partial class NodePool<T> where T : Node, IPoolable
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

	public T Spawn()
	{
		T instance;
		if (_available.Count > 0)
		{
			instance = _available.Pop();	//reuse node already existing
		}
		else
		{
			instance = _scene.Instantiate<T>();	//if none exists, create new
			_parent.AddChild(instance);
			_all.Add(instance);
			_onCreate?.Invoke(instance);
		}
		
		instance.OnSpawn();
		return instance;
	}

	public void Despawn(T instance)
	{
		instance.OnDespawn();
		_available.Push(instance);
	}

	public void DespawnAll()
	{
		_available.Clear();
		foreach (var instance in _all)
		{
			instance.OnDespawn();
			_available.Push(instance);
		}
	}
	
}